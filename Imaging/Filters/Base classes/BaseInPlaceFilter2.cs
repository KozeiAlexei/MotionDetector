using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MotionDetector.Imaging.Filters
{
    public abstract class BaseInPlaceFilter2 : BaseInPlaceFilter
    {
        private Bitmap overlayImage;
        private UnmanagedImage unmanagedOverlayImage;

        public Bitmap OverlayImage
        {
            get { return overlayImage; }
            set
            {
                overlayImage = value;

                if (value != null)
                    unmanagedOverlayImage = null;
            }
        }

        public UnmanagedImage UnmanagedOverlayImage
        {
            get { return unmanagedOverlayImage; }
            set
            {
                unmanagedOverlayImage = value;

                if (value != null)
                    overlayImage = null;
            }
        }

        protected BaseInPlaceFilter2() { }

        protected BaseInPlaceFilter2(Bitmap overlayImage)
        {
            this.overlayImage = overlayImage;
        }

        protected BaseInPlaceFilter2(UnmanagedImage unmanagedOverlayImage)
        {
            this.unmanagedOverlayImage = unmanagedOverlayImage;
        }

        protected override unsafe void ProcessFilter(UnmanagedImage image)
        {
            PixelFormat pixelFormat = image.PixelFormat;

            int width = image.Width;
            int height = image.Height;


            if (overlayImage != null)
            {
                if (pixelFormat != overlayImage.PixelFormat)
                    throw new InvalidImagePropertiesException("Source and overlay images must have same pixel format.");

                if ((width != overlayImage.Width) || (height != overlayImage.Height))
                    throw new InvalidImagePropertiesException("Overlay image size must be equal to source image size.");

                BitmapData ovrData = overlayImage.LockBits(
                    new Rectangle(0, 0, width, height),
                    ImageLockMode.ReadOnly, pixelFormat);

                try
                {
                    ProcessFilter(image, new UnmanagedImage(ovrData));
                }
                finally
                {
                    overlayImage.UnlockBits(ovrData);
                }
            }
            else if (unmanagedOverlayImage != null)
            {
                if (pixelFormat != unmanagedOverlayImage.PixelFormat)
                    throw new InvalidImagePropertiesException("Source and overlay images must have same pixel format.");

                if ((width != unmanagedOverlayImage.Width) || (height != unmanagedOverlayImage.Height))
                    throw new InvalidImagePropertiesException("Overlay image size must be equal to source image size.");

                ProcessFilter(image, unmanagedOverlayImage);
            }
            else
            {
                throw new NullReferenceException("Overlay image is not set.");
            }
        }

        protected abstract unsafe void ProcessFilter(UnmanagedImage image, UnmanagedImage overlay);
    }
}
