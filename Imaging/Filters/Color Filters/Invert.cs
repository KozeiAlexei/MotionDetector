using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MotionDetector.Imaging.Filters
{
    public sealed class Invert : BaseInPlacePartialFilter
    {
        private Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

        public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
        {
            get { return formatTranslations; }
        }

        public Invert()
        {
            formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format8bppIndexed;
            formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format24bppRgb;
            formatTranslations[PixelFormat.Format16bppGrayScale] = PixelFormat.Format16bppGrayScale;
            formatTranslations[PixelFormat.Format48bppRgb] = PixelFormat.Format48bppRgb;
        }

        protected override unsafe void ProcessFilter(UnmanagedImage image, Rectangle rect)
        {
            int pixelSize = ((image.PixelFormat == PixelFormat.Format8bppIndexed) ||
                              (image.PixelFormat == PixelFormat.Format16bppGrayScale)) ? 1 : 3;

            int startY = rect.Top;
            int stopY = startY + rect.Height;

            int startX = rect.Left * pixelSize;
            int stopX = startX + rect.Width * pixelSize;

            byte* basePtr = (byte*)image.ImageData.ToPointer();

            if (
                (image.PixelFormat == PixelFormat.Format8bppIndexed) ||
                (image.PixelFormat == PixelFormat.Format24bppRgb))
            {
                int offset = image.Stride - (stopX - startX);

                byte* ptr = basePtr + (startY * image.Stride + rect.Left * pixelSize);

                for (int y = startY; y < stopY; y++)
                {
                    for (int x = startX; x < stopX; x++, ptr++)
                    {
                        *ptr = (byte)(255 - *ptr);
                    }
                    ptr += offset;
                }
            }
            else
            {
                int stride = image.Stride;

                basePtr += (startY * image.Stride + rect.Left * pixelSize * 2);

                for (int y = startY; y < stopY; y++)
                {
                    ushort* ptr = (ushort*)(basePtr);

                    for (int x = startX; x < stopX; x++, ptr++)
                    {
                        *ptr = (ushort)(65535 - *ptr);
                    }
                    basePtr += stride;
                }
            }
        }
    }
}
