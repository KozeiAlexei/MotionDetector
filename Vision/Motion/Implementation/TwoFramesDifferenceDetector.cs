using System;
using System.Drawing.Imaging;

using MotionDetector.Imaging;
using MotionDetector.Imaging.Filters;

namespace MotionDetector.Vision.Motion
{
    public class TwoFramesDifferenceDetector : IMotionDetector
    {
        private int width;
        private int height;
        private int frameSize;

        private UnmanagedImage previousFrame;

        private UnmanagedImage motionFrame;

        private UnmanagedImage tempFrame;

        private int pixelsChanged;

        private bool suppressNoise = true;

        private int differenceThreshold = 15;
        private int differenceThresholdNeg = -15;

        private BinaryErosion3x3 erosionFilter = new BinaryErosion3x3();

        private object sync = new object();

        public int DifferenceThreshold
        {
            get { return differenceThreshold; }
            set
            {
                lock (sync)
                {
                    differenceThreshold = Math.Max(1, Math.Min(255, value));
                    differenceThresholdNeg = -differenceThreshold;
                }
            }
        }

        public float MotionLevel
        {
            get
            {
                lock (sync)
                {
                    return (float)pixelsChanged / (width * height);
                }
            }
        }

        public UnmanagedImage MotionFrame
        {
            get
            {
                lock (sync)
                {
                    return motionFrame;
                }
            }
        }

        public bool SuppressNoise
        {
            get { return suppressNoise; }
            set
            {
                lock (sync)
                {
                    suppressNoise = value;

                    if ((suppressNoise) && (tempFrame == null) && (motionFrame != null))
                    {
                        tempFrame = UnmanagedImage.Create(width, height, PixelFormat.Format8bppIndexed);
                    }


                    if ((!suppressNoise) && (tempFrame != null))
                    {
                        tempFrame.Dispose();
                        tempFrame = null;
                    }
                }
            }
        }

        public TwoFramesDifferenceDetector() { }

        public TwoFramesDifferenceDetector(bool suppressNoise)
        {
            this.suppressNoise = suppressNoise;
        }

        public unsafe void ProcessFrame(UnmanagedImage videoFrame)
        {
            lock (sync)
            {
                if (previousFrame == null)
                {
                    width = videoFrame.Width;
                    height = videoFrame.Height;

                    previousFrame = UnmanagedImage.Create(width, height, PixelFormat.Format8bppIndexed);
                    motionFrame = UnmanagedImage.Create(width, height, PixelFormat.Format8bppIndexed);

                    frameSize = motionFrame.Stride * height;

                    if (suppressNoise)
                    {
                        tempFrame = UnmanagedImage.Create(width, height, PixelFormat.Format8bppIndexed);
                    }

                    Tools.ConvertToGrayscale(videoFrame, previousFrame);

                    return;
                }

                if ((videoFrame.Width != width) || (videoFrame.Height != height))
                    return;

                Tools.ConvertToGrayscale(videoFrame, motionFrame);

                byte* prevFrame = (byte*)previousFrame.ImageData.ToPointer();
                byte* currFrame = (byte*)motionFrame.ImageData.ToPointer();

                int diff;

                for (int i = 0; i < frameSize; i++, prevFrame++, currFrame++)
                {
                    diff = (int)*currFrame - (int)*prevFrame;
                    *prevFrame = *currFrame;

                    *currFrame = ((diff >= differenceThreshold) || (diff <= differenceThresholdNeg)) ? (byte)255 : (byte)0;
                }

                if (suppressNoise)
                {
                    SystemTools.CopyUnmanagedMemory(tempFrame.ImageData, motionFrame.ImageData, frameSize);
                    erosionFilter.Apply(tempFrame, motionFrame);
                }

                pixelsChanged = 0;
                byte* motion = (byte*)motionFrame.ImageData.ToPointer();

                for (int i = 0; i < frameSize; i++, motion++)
                {
                    pixelsChanged += (*motion & 1);
                }
            }
        }

        public void Reset()
        {
            lock (sync)
            {
                if (previousFrame != null)
                {
                    previousFrame.Dispose();
                    previousFrame = null;
                }

                if (motionFrame != null)
                {
                    motionFrame.Dispose();
                    motionFrame = null;
                }

                if (tempFrame != null)
                {
                    tempFrame.Dispose();
                    tempFrame = null;
                }
            }
        }
    }
}