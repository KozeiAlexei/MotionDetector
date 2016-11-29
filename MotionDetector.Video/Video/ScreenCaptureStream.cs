using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace MotionDetector.Video
{
    public class ScreenCaptureStream : IVideoSource
    {
        private Rectangle region;

        private int frameInterval = 100;

        private int framesReceived;

        private Thread thread = null;
        private ManualResetEvent stopEvent = null;

        public event NewFrameEventHandler NewFrame;

        public event VideoSourceErrorEventHandler VideoSourceError;

        public event PlayingFinishedEventHandler PlayingFinished;

        public virtual string Source
        {
            get { return "Screen Capture"; }
        }

        public Rectangle Region
        {
            get { return region; }
            set { region = value; }
        }

        public int FrameInterval
        {
            get { return frameInterval; }
            set { frameInterval = Math.Max(0, value); }
        }

        public int FramesReceived
        {
            get
            {
                int frames = framesReceived;
                framesReceived = 0;
                return frames;
            }
        }

        public long BytesReceived
        {
            get { return 0; }
        }

        public bool IsRunning
        {
            get
            {
                if (thread != null)
                {
                    if (thread.Join(0) == false)
                        return true;

                    Free();
                }
                return false;
            }
        }

        public ScreenCaptureStream(Rectangle region)
        {
            this.region = region;
        }

        public ScreenCaptureStream(Rectangle region, int frameInterval)
        {
            this.region = region;
            this.FrameInterval = frameInterval;
        }

        public void Start()
        {
            if (!IsRunning)
            {
                framesReceived = 0;

                stopEvent = new ManualResetEvent(false);

                thread = new Thread(new ThreadStart(WorkerThread));
                thread.Name = Source;
                thread.Start();
            }
        }

        public void SignalToStop()
        {
            if (thread != null)
            {
                stopEvent.Set();
            }
        }

        public void WaitForStop()
        {
            if (thread != null)
            {
                thread.Join();

                Free();
            }
        }

        public void Stop()
        {
            if (this.IsRunning)
            {
                stopEvent.Set();
                thread.Abort();
                WaitForStop();
            }
        }

        private void Free()
        {
            thread = null;

            stopEvent.Close();
            stopEvent = null;
        }

        private void WorkerThread()
        {
            int width = region.Width;
            int height = region.Height;
            int x = region.Location.X;
            int y = region.Location.Y;
            Size size = region.Size;

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);

            DateTime start;
            TimeSpan span;

            while (!stopEvent.WaitOne(0, false))
            {
                start = DateTime.Now;

                try
                {
                    graphics.CopyFromScreen(x, y, 0, 0, size, CopyPixelOperation.SourceCopy);

                    framesReceived++;

                    NewFrame?.Invoke(this, new NewFrameEventArgs(bitmap));

                    if (frameInterval > 0)
                    {
                        span = DateTime.Now.Subtract(start);

                        int msec = frameInterval - (int)span.TotalMilliseconds;

                        if ((msec > 0) && (stopEvent.WaitOne(msec, false)))
                            break;
                    }
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception exception)
                {
                    VideoSourceError?.Invoke(this, new VideoSourceErrorEventArgs(exception.Message));
                    Thread.Sleep(250);
                }

                if (stopEvent.WaitOne(0, false))
                    break;
            }

            graphics.Dispose();
            bitmap.Dispose();

            PlayingFinished?.Invoke(this, ReasonToFinishPlaying.StoppedByUser);
        }
    }
}
