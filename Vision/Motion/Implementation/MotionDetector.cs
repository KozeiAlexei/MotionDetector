







namespace MotionDetector.Vision.Motion
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    using global::MotionDetector.Imaging;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public class MotionDetector
    {
        private IMotionDetector detector;
        private IMotionProcessing processor;

        private Rectangle[] motionZones = null;

        private UnmanagedImage zonesFrame;

        private int videoWidth, videoHeight;

        private object sync = new object();

        public IMotionDetector MotionDetectionAlgorithm
        {
            get { return detector; }
            set
            {
                lock (sync)
                {
                    detector = value;
                }
            }
        }

        public IMotionProcessing MotionProcessingAlgorithm
        {
            get { return processor; }
            set
            {
                lock (sync)
                {
                    processor = value;
                }
            }
        }

        public Rectangle[] MotionZones
        {
            get { return motionZones; }
            set
            {
                motionZones = value;
                CreateMotionZonesFrame();
            }
        }

        public MotionDetector(IMotionDetector detector) : this(detector, null) { }

        public MotionDetector(IMotionDetector detector, IMotionProcessing processor)
        {
            this.detector = detector;
            this.processor = processor;
        }

        public float ProcessFrame(Bitmap videoFrame)
        {
            float motionLevel = 0;

            BitmapData videoData = videoFrame.LockBits(
                new Rectangle(0, 0, videoFrame.Width, videoFrame.Height),
                ImageLockMode.ReadWrite, videoFrame.PixelFormat);

            try
            {
                motionLevel = ProcessFrame(new UnmanagedImage(videoData));
            }
            finally
            {
                videoFrame.UnlockBits(videoData);
            }

            return motionLevel;
        }

        public float ProcessFrame(BitmapData videoFrame)
        {
            return ProcessFrame(new UnmanagedImage(videoFrame));
        }

        public float ProcessFrame(UnmanagedImage videoFrame)
        {
            lock (sync)
            {
                if (detector == null)
                    return 0;

                videoWidth = videoFrame.Width;
                videoHeight = videoFrame.Height;

                float motionLevel = 0;

                detector.ProcessFrame(videoFrame);
                motionLevel = detector.MotionLevel;

                if (motionZones != null)
                {
                    if (zonesFrame == null)
                    {
                        CreateMotionZonesFrame();
                    }

                    if ((videoWidth == zonesFrame.Width) && (videoHeight == zonesFrame.Height))
                    {
                        unsafe
                        {
                            byte* zonesPtr = (byte*)zonesFrame.ImageData.ToPointer();
                            byte* motionPtr = (byte*)detector.MotionFrame.ImageData.ToPointer();

                            motionLevel = 0;

                            for (int i = 0, frameSize = zonesFrame.Stride * videoHeight; i < frameSize; i++, zonesPtr++, motionPtr++)
                            {
                                *motionPtr &= *zonesPtr;
                                motionLevel += (*motionPtr & 1);
                            }
                            motionLevel /= (videoWidth * videoHeight);
                        }
                    }
                }

                if ((processor != null) && (detector.MotionFrame != null))
                {
                    processor.ProcessFrame(videoFrame, detector.MotionFrame);
                }

                return motionLevel;
            }
        }

        public void Reset()
        {
            lock (sync)
            {
                if (detector != null)
                {
                    detector.Reset();
                }
                if (processor != null)
                {
                    processor.Reset();
                }

                videoWidth = 0;
                videoHeight = 0;

                if (zonesFrame != null)
                {
                    zonesFrame.Dispose();
                    zonesFrame = null;
                }
            }
        }

        private unsafe void CreateMotionZonesFrame()
        {
            lock (sync)
            {
                if (zonesFrame != null)
                {
                    zonesFrame.Dispose();
                    zonesFrame = null;
                }

                if ((motionZones != null) && (motionZones.Length != 0) && (videoWidth != 0))
                {
                    zonesFrame = UnmanagedImage.Create(videoWidth, videoHeight, PixelFormat.Format8bppIndexed);

                    Rectangle imageRect = new Rectangle(0, 0, videoWidth, videoHeight);

                    foreach (Rectangle rect in motionZones)
                    {
                        rect.Intersect(imageRect);

                        int rectWidth = rect.Width;
                        int rectHeight = rect.Height;

                        int stride = zonesFrame.Stride;
                        byte* ptr = (byte*)zonesFrame.ImageData.ToPointer() + rect.Y * stride + rect.X;

                        for (int y = 0; y < rectHeight; y++)
                        {
                            SystemTools.SetUnmanagedMemory(ptr, 255, rectWidth);
                            ptr += stride;
                        }
                    }
                }
            }
        }
    }
}
