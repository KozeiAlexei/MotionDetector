using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MotionDetector.Imaging.Filters
{
    public sealed class GrayscaleToRGB : BaseFilter
    {
        private Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

        public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
        {
            get { return formatTranslations; }
        }

        public GrayscaleToRGB()
        {
            formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format24bppRgb;
        }

        protected override unsafe void ProcessFilter(UnmanagedImage sourceData, UnmanagedImage destinationData)
        {
            int width = sourceData.Width;
            int height = sourceData.Height;

            int srcOffset = sourceData.Stride - width;
            int dstOffset = destinationData.Stride - width * 3;

            byte* src = (byte*)sourceData.ImageData.ToPointer();
            byte* dst = (byte*)destinationData.ImageData.ToPointer();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++, src++, dst += 3)
                {
                    dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = *src;
                }
                src += srcOffset;
                dst += dstOffset;
            }
        }
    }
}
