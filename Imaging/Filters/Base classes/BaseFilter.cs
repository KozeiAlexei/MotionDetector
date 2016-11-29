using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;


namespace MotionDetector.Imaging.Filters
{
    public abstract class BaseFilter : IFilter, IFilterInformation
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
                ProcessFilter(new UnmanagedImage(imageData), new UnmanagedImage(dstData));
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
            ProcessFilter(image, dstImage);

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

            ProcessFilter(sourceImage, destinationImage);
        }

        protected abstract unsafe void ProcessFilter(UnmanagedImage sourceData, UnmanagedImage destinationData);

        private void CheckSourceFormat(PixelFormat pixelFormat)
        {
            if (!FormatTranslations.ContainsKey(pixelFormat))
                throw new UnsupportedImageFormatException("Source pixel format is not supported by the filter.");
        }
    }
}
