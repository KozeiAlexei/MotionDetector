using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MotionDetector.Imaging.Filters
{
    public abstract class BaseInPlaceFilter : IFilter, IInPlaceFilter, IFilterInformation
    {
        public abstract Dictionary<PixelFormat, PixelFormat> FormatTranslations { get; }

        public Bitmap Apply(Bitmap image)
        {
            BitmapData srcData = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, image.PixelFormat);

            Bitmap dstImage = null;

            try
            {
                dstImage = Apply(srcData);
                if ((image.HorizontalResolution > 0) && (image.VerticalResolution > 0))
                {
                    dstImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                }
            }
            finally
            {
                image.UnlockBits(srcData);
            }

            return dstImage;
        }

        public Bitmap Apply(BitmapData imageData)
        {
            PixelFormat dstPixelFormat = imageData.PixelFormat;

            CheckSourceFormat(dstPixelFormat);

            int width = imageData.Width;
            int height = imageData.Height;

            Bitmap dstImage = (dstPixelFormat == PixelFormat.Format8bppIndexed) ?
                MotionDetector.Imaging.Image.CreateGrayscaleImage(width, height) :
                new Bitmap(width, height, dstPixelFormat);

            BitmapData dstData = dstImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, dstPixelFormat);

            MotionDetector.SystemTools.CopyUnmanagedMemory(dstData.Scan0, imageData.Scan0, imageData.Stride * height);

            try
            {
                ProcessFilter(new UnmanagedImage(dstData));
            }
            finally
            {
                dstImage.UnlockBits(dstData);
            }

            return dstImage;
        }

        public UnmanagedImage Apply(UnmanagedImage image)
        {
            CheckSourceFormat(image.PixelFormat);

            UnmanagedImage dstImage = UnmanagedImage.Create(image.Width, image.Height, image.PixelFormat);

            Apply(image, dstImage);

            return dstImage;
        }

        public void Apply(UnmanagedImage sourceImage, UnmanagedImage destinationImage)
        {
            CheckSourceFormat(sourceImage.PixelFormat);

            if (destinationImage.PixelFormat != sourceImage.PixelFormat)
            {
                throw new InvalidImagePropertiesException("Destination pixel format must be the same as pixel format of source image.");
            }

            if ((destinationImage.Width != sourceImage.Width) || (destinationImage.Height != sourceImage.Height))
            {
                throw new InvalidImagePropertiesException("Destination image must have the same width and height as source image.");
            }

            int dstStride = destinationImage.Stride;
            int srcStride = sourceImage.Stride;
            int lineSize = System.Math.Min(srcStride, dstStride);

            unsafe
            {
                byte* dst = (byte*)destinationImage.ImageData.ToPointer();
                byte* src = (byte*)sourceImage.ImageData.ToPointer();

                for (int y = 0, height = sourceImage.Height; y < height; y++)
                {
                    MotionDetector.SystemTools.CopyUnmanagedMemory(dst, src, lineSize);
                    dst += dstStride;
                    src += srcStride;
                }
            }

            ProcessFilter(destinationImage);
        }

        public void ApplyInPlace(Bitmap image)
        {
            CheckSourceFormat(image.PixelFormat);

            BitmapData data = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadWrite, image.PixelFormat);

            try
            {
                ProcessFilter(new UnmanagedImage(data));
            }
            finally
            {
                image.UnlockBits(data);
            }
        }

        public void ApplyInPlace(BitmapData imageData)
        {
            CheckSourceFormat(imageData.PixelFormat);
            ProcessFilter(new UnmanagedImage(imageData));
        }

        public void ApplyInPlace(UnmanagedImage image)
        {
            CheckSourceFormat(image.PixelFormat);
            ProcessFilter(image);
        }

        protected abstract unsafe void ProcessFilter(UnmanagedImage image);

        private void CheckSourceFormat(PixelFormat pixelFormat)
        {
            if (!FormatTranslations.ContainsKey(pixelFormat))
                throw new UnsupportedImageFormatException("Source pixel format is not supported by the filter.");
        }
    }
}
