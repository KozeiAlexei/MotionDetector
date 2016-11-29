using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MotionDetector.Imaging.Filters
{
    public class Grayscale : BaseFilter
    {
        public static class CommonAlgorithms
        {
            public static readonly Grayscale BT709 = new Grayscale(0.2125, 0.7154, 0.0721);

            public static readonly Grayscale RMY = new Grayscale(0.5000, 0.4190, 0.0810);

            public static readonly Grayscale Y = new Grayscale(0.2990, 0.5870, 0.1140);
        }

        public readonly double RedCoefficient;
        public readonly double GreenCoefficient;
        public readonly double BlueCoefficient;

        private Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

        public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
        {
            get { return formatTranslations; }
        }

        public Grayscale(double cr, double cg, double cb)
        {
            RedCoefficient = cr;
            GreenCoefficient = cg;
            BlueCoefficient = cb;

            formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format8bppIndexed;
            formatTranslations[PixelFormat.Format32bppRgb] = PixelFormat.Format8bppIndexed;
            formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format8bppIndexed;
            formatTranslations[PixelFormat.Format48bppRgb] = PixelFormat.Format16bppGrayScale;
            formatTranslations[PixelFormat.Format64bppArgb] = PixelFormat.Format16bppGrayScale;
        }

        protected override unsafe void ProcessFilter(UnmanagedImage sourceData, UnmanagedImage destinationData)
        {
            int width = sourceData.Width;
            int height = sourceData.Height;
            PixelFormat srcPixelFormat = sourceData.PixelFormat;

            if (
                (srcPixelFormat == PixelFormat.Format24bppRgb) ||
                (srcPixelFormat == PixelFormat.Format32bppRgb) ||
                (srcPixelFormat == PixelFormat.Format32bppArgb))
            {
                int pixelSize = (srcPixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;
                int srcOffset = sourceData.Stride - width * pixelSize;
                int dstOffset = destinationData.Stride - width;

                int rc = (int)(0x10000 * RedCoefficient);
                int gc = (int)(0x10000 * GreenCoefficient);
                int bc = (int)(0x10000 * BlueCoefficient);

                byte* src = (byte*)sourceData.ImageData.ToPointer();
                byte* dst = (byte*)destinationData.ImageData.ToPointer();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++, src += pixelSize, dst++)
                    {
                        *dst = (byte)((rc * src[RGB.R] + gc * src[RGB.G] + bc * src[RGB.B]) >> 16);
                    }
                    src += srcOffset;
                    dst += dstOffset;
                }
            }
            else
            {
                int pixelSize = (srcPixelFormat == PixelFormat.Format48bppRgb) ? 3 : 4;
                byte* srcBase = (byte*)sourceData.ImageData.ToPointer();
                byte* dstBase = (byte*)destinationData.ImageData.ToPointer();
                int srcStride = sourceData.Stride;
                int dstStride = destinationData.Stride;

                for (int y = 0; y < height; y++)
                {
                    ushort* src = (ushort*)(srcBase + y * srcStride);
                    ushort* dst = (ushort*)(dstBase + y * dstStride);

                    for (int x = 0; x < width; x++, src += pixelSize, dst++)
                    {
                        *dst = (ushort)(RedCoefficient * src[RGB.R] + GreenCoefficient * src[RGB.G] + BlueCoefficient * src[RGB.B]);
                    }
                }
            }
        }
    }
}
