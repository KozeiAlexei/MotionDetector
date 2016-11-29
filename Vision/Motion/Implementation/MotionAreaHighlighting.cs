using System.Drawing;
using System.Drawing.Imaging;

using MotionDetector.Imaging;

namespace MotionDetector.Vision.Motion
{
    public class MotionAreaHighlighting : IMotionProcessing
    {
        private Color highlightColor = Color.Red;

        public Color HighlightColor
        {
            get { return highlightColor; }
            set { highlightColor = value; }
        }

        public MotionAreaHighlighting() { }

        public MotionAreaHighlighting(Color highlightColor)
        {
            this.highlightColor = highlightColor;
        }

        public unsafe void ProcessFrame(UnmanagedImage videoFrame, UnmanagedImage motionFrame)
        {
            if (motionFrame.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new InvalidImagePropertiesException("Motion frame must be 8 bpp image.");
            }

            if ((videoFrame.PixelFormat != PixelFormat.Format8bppIndexed) &&
                 (videoFrame.PixelFormat != PixelFormat.Format24bppRgb) &&
                 (videoFrame.PixelFormat != PixelFormat.Format32bppRgb) &&
                 (videoFrame.PixelFormat != PixelFormat.Format32bppArgb))
            {
                throw new UnsupportedImageFormatException("Video frame must be 8 bpp grayscale image or 24/32 bpp color image.");
            }

            int width = videoFrame.Width;
            int height = videoFrame.Height;
            int pixelSize = Bitmap.GetPixelFormatSize(videoFrame.PixelFormat) / 8;

            if ((motionFrame.Width != width) || (motionFrame.Height != height))
                return;

            byte* src = (byte*)videoFrame.ImageData.ToPointer();
            byte* motion = (byte*)motionFrame.ImageData.ToPointer();

            int srcOffset = videoFrame.Stride - width * pixelSize;
            int motionOffset = motionFrame.Stride - width;

            if (pixelSize == 1)
            {
                byte fillG = (byte)(0.2125 * highlightColor.R +
                                      0.7154 * highlightColor.G +
                                      0.0721 * highlightColor.B);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++, motion++, src++)
                    {
                        if ((*motion != 0) && (((x + y) & 1) == 0))
                        {
                            *src = fillG;
                        }
                    }
                    src += srcOffset;
                    motion += motionOffset;
                }
            }
            else
            {
                byte fillR = highlightColor.R;
                byte fillG = highlightColor.G;
                byte fillB = highlightColor.B;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++, motion++, src += pixelSize)
                    {
                        if ((*motion != 0) && (((x + y) & 1) == 0))
                        {
                            src[RGB.R] = fillR;
                            src[RGB.G] = fillG;
                            src[RGB.B] = fillB;
                        }
                    }
                    src += srcOffset;
                    motion += motionOffset;
                }
            }
        }

        public void Reset()
        {
        }
    }
}
