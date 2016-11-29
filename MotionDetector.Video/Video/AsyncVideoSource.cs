using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace MotionDetector.Video
{
    public class AsyncVideoSource : IVideoSource
    {
        private readonly IVideoSource nestedVideoSource = null;
        private Bitmap lastVideoFrame = null;

        private Thread imageProcessingThread = null;
        private AutoResetEvent isNewFrameAvailable = null;
        private AutoResetEvent isProcessingThreadAvailable = null;

        private bool skipFramesIfBusy = false;

        private int framesProcessed;

        public event NewFrameEventHandler NewFrame;

        public event VideoSourceErrorEventHandler VideoSourceError
        {
            add { nestedVideoSource.VideoSourceError += value; }
            remove { nestedVideoSource.VideoSourceError -= value; }
        }

        public event PlayingFinishedEventHandler PlayingFinished
        {
            add { nestedVideoSource.PlayingFinished += value; }
            remove { nestedVideoSource.PlayingFinished -= value; }
        }


        public IVideoSource NestedVideoSource
        {
            get { return nestedVideoSource; }
        }

        public bool SkipFramesIfBusy
        {
            get { return skipFramesIfBusy; }
            set { skipFramesIfBusy = value; }
        }

        public string Source
        {
            get { return nestedVideoSource.Source; }
        }

        public int FramesReceived
        {
            get { return nestedVideoSource.FramesReceived; }
        }

        public long BytesReceived
        {
            get { return nestedVideoSource.BytesReceived; }
        }

        public int FramesProcessed
        {
            get
            {
                int frames = framesProcessed;
                framesProcessed = 0;
                return frames;
            }
        }

        public bool IsRunning
        {
            get
            {
                bool isRunning = nestedVideoSource.IsRunning;

                if (!isRunning)
                {
                    Free();
                }

                return isRunning;
            }
        }

        public AsyncVideoSource(IVideoSource nestedVideoSource)
        {
            this.nestedVideoSource = nestedVideoSource;
        }

        public AsyncVideoSource(IVideoSource nestedVideoSource, bool skipFramesIfBusy)
        {
            this.nestedVideoSource = nestedVideoSource;
            this.skipFramesIfBusy = skipFramesIfBusy;
        }

        public void Start()
        {
            if (!IsRunning)
            {
                framesProcessed = 0;

                isNewFrameAvailable = new AutoResetEvent(false);
                isProcessingThreadAvailable = new AutoResetEvent(true);

                imageProcessingThread = new Thread(new ThreadStart(imageProcessingThread_Worker));
                imageProcessingThread.Start();

                nestedVideoSource.NewFrame += new NewFrameEventHandler(nestedVideoSource_NewFrame);
                nestedVideoSource.Start();
            }
        }

        public void SignalToStop()
        {
            nestedVideoSource.SignalToStop();
        }

        public void WaitForStop()
        {
            nestedVideoSource.WaitForStop();
            Free();
        }

        public void Stop()
        {
            nestedVideoSource.Stop();
            Free();
        }

        private void Free()
        {
            if (imageProcessingThread != null)
            {
                nestedVideoSource.NewFrame -= new NewFrameEventHandler(nestedVideoSource_NewFrame);

                isProcessingThreadAvailable.WaitOne();

                lastVideoFrame = null;
                isNewFrameAvailable.Set();
                imageProcessingThread.Join();
                imageProcessingThread = null;

                isNewFrameAvailable.Close();
                isNewFrameAvailable = null;

                isProcessingThreadAvailable.Close();
                isProcessingThreadAvailable = null;
            }
        }

        private void nestedVideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (NewFrame == null)
                return;

            if (skipFramesIfBusy)
            {
                if (!isProcessingThreadAvailable.WaitOne(0, false))
                {
                    return;
                }
            }
            else
            {
                isProcessingThreadAvailable.WaitOne();
            }

            lastVideoFrame = CloneImage(eventArgs.Frame);
            isNewFrameAvailable.Set();
        }

        private void imageProcessingThread_Worker()
        {
            while (true)
            {
                isNewFrameAvailable.WaitOne();

                if (lastVideoFrame == null)
                {
                    break;
                }

                if (NewFrame != null)
                {
                    NewFrame(this, new NewFrameEventArgs(lastVideoFrame));
                }

                lastVideoFrame.Dispose();
                lastVideoFrame = null;
                framesProcessed++;

                isProcessingThreadAvailable.Set();
            }
        }

        private static Bitmap CloneImage(Bitmap source)
        {
            BitmapData sourceData = source.LockBits(
                new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly, source.PixelFormat);

            Bitmap destination = CloneImage(sourceData);

            source.UnlockBits(sourceData);

            if (
                (source.PixelFormat == PixelFormat.Format1bppIndexed) ||
                (source.PixelFormat == PixelFormat.Format4bppIndexed) ||
                (source.PixelFormat == PixelFormat.Format8bppIndexed) ||
                (source.PixelFormat == PixelFormat.Indexed))
            {
                ColorPalette srcPalette = source.Palette;
                ColorPalette dstPalette = destination.Palette;

                int n = srcPalette.Entries.Length;

                for (int i = 0; i < n; i++)
                {
                    dstPalette.Entries[i] = srcPalette.Entries[i];
                }

                destination.Palette = dstPalette;
            }

            return destination;
        }

        private static Bitmap CloneImage(BitmapData sourceData)
        {
            int width = sourceData.Width;
            int height = sourceData.Height;

            Bitmap destination = new Bitmap(width, height, sourceData.PixelFormat);

            BitmapData destinationData = destination.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, destination.PixelFormat);

            SystemTools.CopyUnmanagedMemory(destinationData.Scan0, sourceData.Scan0, height * sourceData.Stride);

            destination.UnlockBits(destinationData);

            return destination;
        }
    }
}
