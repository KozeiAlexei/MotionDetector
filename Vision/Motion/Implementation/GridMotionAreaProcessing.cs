using System;
using System.Drawing;
using System.Drawing.Imaging;

using MotionDetector.Imaging;

namespace MotionDetector.Vision.Motion
{
    public class GridMotionAreaProcessing : IMotionProcessing
    {
        private Color highlightColor = Color.Red;

        private bool highlightMotionGrid = true;

        private float motionAmountToHighlight = 0.15f;

        private int gridWidth = 16;
        private int gridHeight = 16;

        private float[,] motionGrid = null;

        public Color HighlightColor
        {
            get { return highlightColor; }
            set { highlightColor = value; }
        }

        public bool HighlightMotionGrid
        {
            get { return highlightMotionGrid; }
            set { highlightMotionGrid = value; }
        }

        public float MotionAmountToHighlight
        {
            get { return motionAmountToHighlight; }
            set { motionAmountToHighlight = value; }
        }

        public float[,] MotionGrid
        {
            get { return motionGrid; }
        }

        public int GridWidth
        {
            get { return gridWidth; }
            set
            {
                gridWidth = Math.Min(64, Math.Max(2, value));
                motionGrid = new float[gridHeight, gridWidth];
            }
        }

        public int GridHeight
        {
            get { return gridHeight; }
            set
            {
                gridHeight = Math.Min(64, Math.Max(2, value));
                motionGrid = new float[gridHeight, gridWidth];
            }
        }

        public GridMotionAreaProcessing() : this(16, 16) { }

        public GridMotionAreaProcessing(int gridWidth, int gridHeight) : this(gridWidth, gridWidth, true) { }

        public GridMotionAreaProcessing(int gridWidth, int gridHeight, bool highlightMotionGrid)
            : this(gridWidth, gridHeight, highlightMotionGrid, 0.15f) { }

        public GridMotionAreaProcessing(int gridWidth, int gridHeight, bool highlightMotionGrid, float motionAmountToHighlight)
        {
            this.gridWidth = Math.Min(64, Math.Max(2, gridWidth));
            this.gridHeight = Math.Min(64, Math.Max(2, gridHeight));

            motionGrid = new float[gridHeight, gridWidth];

            this.highlightMotionGrid = highlightMotionGrid;
            this.motionAmountToHighlight = motionAmountToHighlight;
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

            int cellWidth = width / gridWidth;
            int cellHeight = height / gridHeight;

            int xCell, yCell;

            byte* motion = (byte*)motionFrame.ImageData.ToPointer();
            int motionOffset = motionFrame.Stride - width;

            for (int y = 0; y < height; y++)
            {
                yCell = y / cellHeight;

                if (yCell >= gridHeight)
                    yCell = gridHeight - 1;

                for (int x = 0; x < width; x++, motion++)
                {
                    if (*motion != 0)
                    {
                        xCell = x / cellWidth;

                        if (xCell >= gridWidth)
                            xCell = gridWidth - 1;

                        motionGrid[yCell, xCell]++;
                    }
                }
                motion += motionOffset;
            }

            int gridHeightM1 = gridHeight - 1;
            int gridWidthM1 = gridWidth - 1;

            int lastRowHeight = height - cellHeight * gridHeightM1;
            int lastColumnWidth = width - cellWidth * gridWidthM1;

            for (int y = 0; y < gridHeight; y++)
            {
                int ch = (y != gridHeightM1) ? cellHeight : lastRowHeight;

                for (int x = 0; x < gridWidth; x++)
                {
                    int cw = (x != gridWidthM1) ? cellWidth : lastColumnWidth;

                    motionGrid[y, x] /= (cw * ch);
                }
            }

            if (highlightMotionGrid)
            {
                byte* src = (byte*)videoFrame.ImageData.ToPointer();
                int srcOffset = videoFrame.Stride - width * pixelSize;

                if (pixelSize == 1)
                {
                    byte fillG = (byte)(0.2125 * highlightColor.R +
                                          0.7154 * highlightColor.G +
                                          0.0721 * highlightColor.B);

                    for (int y = 0; y < height; y++)
                    {
                        yCell = y / cellHeight;
                        if (yCell >= gridHeight)
                            yCell = gridHeight - 1;

                        for (int x = 0; x < width; x++, src++)
                        {
                            xCell = x / cellWidth;
                            if (xCell >= gridWidth)
                                xCell = gridWidth - 1;

                            if ((motionGrid[yCell, xCell] > motionAmountToHighlight) && (((x + y) & 1) == 0))
                            {
                                *src = fillG;
                            }
                        }
                        src += srcOffset;
                    }
                }
                else
                {
                    byte fillR = highlightColor.R;
                    byte fillG = highlightColor.G;
                    byte fillB = highlightColor.B;

                    for (int y = 0; y < height; y++)
                    {
                        yCell = y / cellHeight;
                        if (yCell >= gridHeight)
                            yCell = gridHeight - 1;

                        for (int x = 0; x < width; x++, src += pixelSize)
                        {
                            xCell = x / cellWidth;
                            if (xCell >= gridWidth)
                                xCell = gridWidth - 1;

                            if ((motionGrid[yCell, xCell] > motionAmountToHighlight) && (((x + y) & 1) == 0))
                            {
                                src[RGB.R] = fillR;
                                src[RGB.G] = fillG;
                                src[RGB.B] = fillB;
                            }
                        }
                        src += srcOffset;
                    }
                }
            }
        }

        public void Reset()
        {
        }
    }
}
