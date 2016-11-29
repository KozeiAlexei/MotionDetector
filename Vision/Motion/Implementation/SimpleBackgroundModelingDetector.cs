using System;
using System.Drawing.Imaging;

using MotionDetector.Imaging;
using MotionDetector.Imaging.Filters;

namespace MotionDetector.Vision.Motion
{
    public class SimpleBackgroundModelingDetector : IMotionDetector
    {
        private int width;
        private int height;
        private int frameSize;

        private UnmanagedImage backgroundFrame;

        private UnmanagedImage motionFrame;

        private UnmanagedImage tempFrame;

        private int pixelsChanged;

        private bool suppressNoise = true;
        private bool keepObjectEdges = false;

        private int differenceThreshold = 15;
        private int differenceThresholdNeg = -15;

        private int framesPerBackgroundUpdate = 2;
        private int framesCounter = 0;

        private int millisecondsPerBackgroundUpdate = 0;
        private int millisecondsLeftUnprocessed = 0;
        private DateTime lastTimeMeasurment;

        private BinaryErosion3x3 erosionFilter = new BinaryErosion3x3();

        private BinaryDilatation3x3 dilatationFilter = new BinaryDilatation3x3();

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

        public bool KeepObjectsEdges
        {
            get { return keepObjectEdges; }
            set
            {
                lock (sync)
                {
                    keepObjectEdges = value;
                }
            }
        }

        public int FramesPerBackgroundUpdate
        {
            get { return framesPerBackgroundUpdate; }
            set { framesPerBackgroundUpdate = Math.Max(1, Math.Min(50, value)); }
        }

        public int MillisecondsPerBackgroundUpdate
        {
            get { return millisecondsPerBackgroundUpdate; }
            set { millisecondsPerBackgroundUpdate = Math.Max(0, Math.Min(5000, value)); }
        }

        public SimpleBackgroundModelingDetector() { }

        public SimpleBackgroundModelingDetector(bool suppressNoise)
        {
            this.suppressNoise = suppressNoise;
        }

        public SimpleBackgroundModelingDetector(bool suppressNoise, bool keepObjectEdges)
        {
            this.suppressNoise = suppressNoise;
            this.keepObjectEdges = keepObjectEdges;
        }

        public unsafe void ProcessFrame(UnmanagedImage videoFrame)
        {
            lock (sync)
            {
                if (backgroundFrame == null)
                {
                    lastTimeMeasurment = DateTime.Now;

                    width = videoFrame.Width;
                    height = videoFrame.Height;

                    backgroundFrame = UnmanagedImage.Create(width, height, PixelFormat.Format8bppIndexed);
                    motionFrame = UnmanagedImage.Create(width, height, PixelFormat.Format8bppIndexed);

                    frameSize = motionFrame.Stride * height;

                    if (suppressNoise)
                    {
                        tempFrame = UnmanagedImage.Create(width, height, PixelFormat.Format8bppIndexed);
                    }

                    Tools.ConvertToGrayscale(videoFrame, backgroundFrame);

                    return;
                }

                if ((videoFrame.Width != width) || (videoFrame.Height != height))
                    return;

                Tools.ConvertToGrayscale(videoFrame, motionFrame);

                byte* backFrame;
                byte* currFrame;
                int diff;

                if (millisecondsPerBackgroundUpdate == 0)
                {
                    if (++framesCounter == framesPerBackgroundUpdate)
                    {
                        framesCounter = 0;

                        backFrame = (byte*)backgroundFrame.ImageData.ToPointer();
                        currFrame = (byte*)motionFrame.ImageData.ToPointer();

                        for (int i = 0; i < frameSize; i++, backFrame++, currFrame++)
                        {
                            diff = *currFrame - *backFrame;
                            if (diff > 0)
                            {
                                (*backFrame)++;
                            }
                            else if (diff < 0)
                            {
                                (*backFrame)--;
                            }
                        }
                    }
                }
                else
                {
                    DateTime currentTime = DateTime.Now;
                    TimeSpan timeDff = currentTime - lastTimeMeasurment;

                    lastTimeMeasurment = currentTime;

                    int millisonds = (int)timeDff.TotalMilliseconds + millisecondsLeftUnprocessed;

                    millisecondsLeftUnprocessed = millisonds % millisecondsPerBackgroundUpdate;

                    int updateAmount = (int)(millisonds / millisecondsPerBackgroundUpdate);

                    backFrame = (byte*)backgroundFrame.ImageData.ToPointer();
                    currFrame = (byte*)motionFrame.ImageData.ToPointer();

                    for (int i = 0; i < frameSize; i++, backFrame++, currFrame++)
                    {
                        diff = *currFrame - *backFrame;
                        if (diff > 0)
                        {
                            (*backFrame) += (byte)((diff < updateAmount) ? diff : updateAmount);
                        }
                        else if (diff < 0)
                        {
                            (*backFrame) += (byte)((-diff < updateAmount) ? diff : -updateAmount);
                        }
                    }
                }

                backFrame = (byte*)backgroundFrame.ImageData.ToPointer();
                currFrame = (byte*)motionFrame.ImageData.ToPointer();

                for (int i = 0; i < frameSize; i++, backFrame++, currFrame++)
                {
                    diff = (int)*currFrame - (int)*backFrame;

                    *currFrame = ((diff >= differenceThreshold) || (diff <= differenceThresholdNeg)) ? (byte)255 : (byte)0;
                }

                if (suppressNoise)
                {

                    SystemTools.CopyUnmanagedMemory(tempFrame.ImageData, motionFrame.ImageData, frameSize);
                    erosionFilter.Apply(tempFrame, motionFrame);

                    if (keepObjectEdges)
                    {
                        SystemTools.CopyUnmanagedMemory(tempFrame.ImageData, motionFrame.ImageData, frameSize);
                        dilatationFilter.Apply(tempFrame, motionFrame);
                    }
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
                if (backgroundFrame != null)
                {
                    backgroundFrame.Dispose();
                    backgroundFrame = null;
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

                framesCounter = 0;
            }
        }
    }
}
