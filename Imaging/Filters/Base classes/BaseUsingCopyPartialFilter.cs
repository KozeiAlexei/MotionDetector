using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MotionDetector.Imaging.Filters
{
    public abstract class BaseUsingCopyPartialFilter : IFilter, IInPlaceFilter, IInPlacePartialFilter, IFilterInformation
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
            CheckSourceFormat(imageData.PixelFormat);

            int width = imageData.Width;
            int height = imageData.Height;

            PixelFormat dstPixelFormat = FormatTranslations[imageData.PixelFormat];

            Bitmap dstImage = (dstPixelFormat == PixelFormat.Format8bppIndexed) ?
                MotionDetector.Imaging.Image.CreateGrayscaleImage(width, height) :
                new Bitmap(width, height, dstPixelFormat);

            BitmapData dstData = dstImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, dstPixelFormat);

            try
            {
                ProcessFilter(new UnmanagedImage(imageData), new UnmanagedImage(dstData), new Rectangle(0, 0, width, height));
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

            UnmanagedImage dstImage = UnmanagedImage.Create(image.Width, image.Height, FormatTranslations[image.PixelFormat]);

            ProcessFilter(image, dstImage, new Rectangle(0, 0, image.Width, image.Height));

            return dstImage;
        }

        public void Apply(UnmanagedImage sourceImage, UnmanagedImage destinationImage)
        {
            CheckSourceFormat(sourceImage.PixelFormat);

            if (destinationImage.PixelFormat != FormatTranslations[sourceImage.PixelFormat])
            {
                throw new InvalidImagePropertiesException("Destination pixel format is specified incorrectly.");
            }

            if ((destinationImage.Width != sourceImage.Width) || (destinationImage.Height != sourceImage.Height))
            {
                throw new InvalidImagePropertiesException("Destination image must have the same width and height as source image.");
            }

            ProcessFilter(sourceImage, destinationImage, new Rectangle(0, 0, sourceImage.Width, sourceImage.Height));
        }

        public void ApplyInPlace(Bitmap image)
        {
            ApplyInPlace(image, new Rectangle(0, 0, image.Width, image.Height));
        }

        public void ApplyInPlace(BitmapData imageData)
        {
            ApplyInPlace(new UnmanagedImage(imageData), new Rectangle(0, 0, imageData.Width, imageData.Height));
        }

        public void ApplyInPlace(UnmanagedImage image)
        {
            ApplyInPlace(image, new Rectangle(0, 0, image.Width, image.Height));
        }

        public void ApplyInPlace(Bitmap image, Rectangle rect)
        {
            BitmapData data = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadWrite, image.PixelFormat);

            try
            {
                ApplyInPlace(new UnmanagedImage(data), rect);
            }
            finally
            {
                image.UnlockBits(data);
            }
        }

        public void ApplyInPlace(BitmapData imageData, Rectangle rect)
        {
            ApplyInPlace(new UnmanagedImage(imageData), rect);
        }

        public void ApplyInPlace(UnmanagedImage image, Rectangle rect)
        {
            CheckSourceFormat(image.PixelFormat);

            rect.Intersect(new Rectangle(0, 0, image.Width, image.Height));

            if ((rect.Width | rect.Height) != 0)
            {
                int size = image.Stride * image.Height;

                IntPtr imageCopy = MemoryManager.Alloc(size);
                MotionDetector.SystemTools.CopyUnmanagedMemory(imageCopy, image.ImageData, size);

                ProcessFilter(
                    new UnmanagedImage(imageCopy, image.Width, image.Height, image.Stride, image.PixelFormat),
                    image, rect);

                MemoryManager.Free(imageCopy);
            }
        }

        protected abstract unsafe void ProcessFilter(UnmanagedImage sourceData, UnmanagedImage destinationData, Rectangle rect);

        private void CheckSourceFormat(PixelFormat pixelFormat)
        {
            if (!FormatTranslations.ContainsKey(pixelFormat))
                throw new UnsupportedImageFormatException("Source pixel format is not supported by the filter.");
        }
    }
}
