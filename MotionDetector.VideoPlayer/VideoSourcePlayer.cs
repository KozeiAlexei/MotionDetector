using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;

using MotionDetector.Video;

namespace MotionDetector.Controls
{
    using Point = System.Drawing.Point;

    public partial class VideoSourcePlayer : Control
    {
        private IVideoSource videoSource = null;

        private Bitmap currentFrame = null;
        private Bitmap convertedFrame = null;

        private string lastMessage = null;

        private Color borderColor = Color.Black;

        private Size frameSize = new Size(320, 240);

        private bool autoSize = false;
        private bool keepRatio = false;
        private bool needSizeUpdate = false;
        private bool firstFrameNotProcessed = true;

        private volatile bool requestedToStop = false;

        private Control parent = null;

        private object sync_context = new object();

        private void CallActionsLinear(Action preback, Action postback)
        {
            preback();
            postback();
        }

        private TProperty CalActionAndFuncLinear<TProperty>(Action preback, Func<TProperty> postback)
        {
            preback();
            return postback();
        }

        [DefaultValue(false)]
        public bool AutoSizeControl
        {
            get
            {
                return autoSize;
            }
            set
            {
                CallActionsLinear(() => autoSize = value, () => UpdatePosition());
            }
        }

        [DefaultValue(false)]
        public bool KeepAspectRatio
        {
            get { return keepRatio; }
            set
            {
                CallActionsLinear(() => keepRatio = value, () => Invalidate());
            }
        }

        [DefaultValue(typeof(Color), "Black")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                CallActionsLinear(() => borderColor = value, () => Invalidate());
            }
        }

       
        [Browsable(false)]
        public IVideoSource VideoSource
        {
            get { return videoSource; }
            set
            {
                CheckForCrossThreadAccess();

                if (videoSource != null)
                {
                    videoSource.NewFrame -= new NewFrameEventHandler(VideoSourceNewFrame);
                    videoSource.PlayingFinished -= new PlayingFinishedEventHandler(VideoSourcePlayingFinished);
                    videoSource.VideoSourceError -= new VideoSourceErrorEventHandler(VideoSourceVideoSourceError);
                }

                lock (sync_context)
                {
                    if (currentFrame != null)
                    {
                        currentFrame.Dispose();
                        currentFrame = null;
                    }
                }

                videoSource = value;

                if (videoSource != null)
                {
                    videoSource.NewFrame += new NewFrameEventHandler(VideoSourceNewFrame);
                    videoSource.PlayingFinished += new PlayingFinishedEventHandler(VideoSourcePlayingFinished);
                    videoSource.VideoSourceError += new VideoSourceErrorEventHandler(VideoSourceVideoSourceError);
                }
                else
                    frameSize = new Size(320, 240);

                lastMessage = null;
                needSizeUpdate = true;
                firstFrameNotProcessed = true;

                Invalidate();
            }
        }

        [Browsable(false)]
        public bool IsRunning
        {
            get
            {
                return CalActionAndFuncLinear(() => CheckForCrossThreadAccess(), () => (videoSource != null) ? videoSource.IsRunning : false);
            }
        }

        public delegate void NewFrameHandler(object sender, ref Bitmap image);

        public event NewFrameHandler NewFrame;
        public event PlayingFinishedEventHandler PlayingFinished;

        public VideoSourcePlayer()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
        }

        private void CheckForCrossThreadAccess()
        {
            if (!IsHandleCreated)
            {
                CreateControl();

                if (!IsHandleCreated)
                    CreateHandle();
            }

            if (InvokeRequired)
                throw new InvalidOperationException("Cross thread access to the control is not allowed.");
        }

        public void Start()
        {
            CheckForCrossThreadAccess();

            requestedToStop = false;

            if (videoSource != null)
            {
                firstFrameNotProcessed = true;

                videoSource.Start();
                Invalidate();
            }
        }

        public void Stop()
        {
            CheckForCrossThreadAccess();

            requestedToStop = true;

            if (videoSource != null)
            {
                videoSource.Stop();

                if (currentFrame != null)
                {
                    currentFrame.Dispose();
                    currentFrame = null;
                }

                Invalidate();
            }
        }

        public void SignalToStop()
        {
            CheckForCrossThreadAccess();

            requestedToStop = true;

            if (videoSource != null)
                videoSource.SignalToStop();
        }

        public void WaitForStop()
        {
            CheckForCrossThreadAccess();

            if (!requestedToStop)
                SignalToStop();

            if (videoSource != null)
            {
                videoSource.WaitForStop();

                if (currentFrame != null)
                {
                    currentFrame.Dispose();
                    currentFrame = null;
                }

                Invalidate();
            }
        }

        public Bitmap GetCurrentVideoFrame()
        {
            lock (sync_context)
            {
                return (currentFrame == null) ? null : Imaging.Image.Clone(currentFrame);
            }
        }

        private void VideoSourcePlayer_Paint(object sender, PaintEventArgs e)
        {
            if (!Visible)
                return;

            if (needSizeUpdate || firstFrameNotProcessed)
                CallActionsLinear(() => UpdatePosition(), () => needSizeUpdate = false);

            lock (sync_context)
            {
                var pen = new Pen(borderColor, 1);
                var graphics = e.Graphics;
                var rectangle = ClientRectangle;

                graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);

                if (videoSource != null)
                {
                    if ((currentFrame != null) && (lastMessage == null))
                    {
                        var frame = (convertedFrame != null) ? convertedFrame : currentFrame;

                        if (keepRatio)
                        {
                            var ratio = (double)frame.Width / frame.Height;
                            var newRectangle = rectangle;

                            if (rectangle.Width < rectangle.Height * ratio)
                                newRectangle.Height = (int)(rectangle.Width / ratio);
                            else
                                newRectangle.Width = (int)(rectangle.Height * ratio);

                            newRectangle.X = (rectangle.Width - newRectangle.Width) / 2;
                            newRectangle.Y = (rectangle.Height - newRectangle.Height) / 2;

                            graphics.DrawImage(frame, newRectangle.X + 1, newRectangle.Y + 1, newRectangle.Width - 2, newRectangle.Height - 2);
                        }
                        else
                            graphics.DrawImage(frame, rectangle.X + 1, rectangle.Y + 1, rectangle.Width - 2, rectangle.Height - 2);

                        firstFrameNotProcessed = false;
                    }
                    else
                    {
                        var drawBrush = new SolidBrush(ForeColor);

                        var message = (lastMessage == null) ? "Подключение к источнику видео..." : lastMessage;
                        graphics.DrawString(message, Font, drawBrush, new PointF(5, 5));

                        drawBrush.Dispose();
                    }
                }

                pen.Dispose();
            }
        }

        private void UpdatePosition()
        {
            if ((autoSize) && (Dock != DockStyle.Fill) && (Parent != null))
            {
                var width = frameSize.Width;
                var height = frameSize.Height;

                var rectangle = Parent.ClientRectangle;

                SuspendLayout();

                Size = new Size(width + 2, height + 2);
                Location = new Point((rectangle.Width - width - 2) / 2, (rectangle.Height - height - 2) / 2);

                this.ResumeLayout();
            }
        }

        private void VideoSourceNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (!requestedToStop)
            {
                var newFrame = (Bitmap)eventArgs.Frame.Clone();

                NewFrame?.Invoke(this, ref newFrame);
                lock (sync_context)
                {
                    if (currentFrame != null)
                    {
                        if (currentFrame.Size != eventArgs.Frame.Size)
                            needSizeUpdate = true;

                        currentFrame.Dispose();
                        currentFrame = null;
                    }
                    if (convertedFrame != null)
                    {
                        convertedFrame.Dispose();
                        convertedFrame = null;
                    }

                    currentFrame = newFrame;
                    frameSize = currentFrame.Size;

                    lastMessage = null;

                    if ((currentFrame.PixelFormat == PixelFormat.Format16bppGrayScale) || (currentFrame.PixelFormat == PixelFormat.Format48bppRgb) || (currentFrame.PixelFormat == PixelFormat.Format64bppArgb))
                        convertedFrame = Imaging.Image.Convert16bppTo8bpp(currentFrame);
                }

                Invalidate();
            }
        }

        private void VideoSourceVideoSourceError(object sender, VideoSourceErrorEventArgs eventArgs) =>
            CallActionsLinear(() => lastMessage = eventArgs.Description, () => Invalidate());

        private void VideoSourcePlayingFinished(object sender, ReasonToFinishPlaying reason)
        {
            switch (reason)
            {
                case ReasonToFinishPlaying.EndOfStreamReached:
                    lastMessage = "Показ видео окончен!";
                    break;

                case ReasonToFinishPlaying.StoppedByUser:
                    lastMessage = "Показ видео остановлен!";
                    break;

                case ReasonToFinishPlaying.DeviceLost:
                    lastMessage = "Устройство отключено!";
                    break;

                case ReasonToFinishPlaying.VideoSourceError:
                    lastMessage = "Ошибка источника видео!";
                    break;

                default:
                    lastMessage = "Неизвестная ошибка!";
                    break;
            }
            Invalidate();

            PlayingFinished?.Invoke(this, reason);
        }

        private void VideoSourcePlayer_ParentChanged(object sender, EventArgs e)
        {
            if (parent != null)
                parent.SizeChanged -= new EventHandler(ParentSizeChanged);

            parent = Parent;

            if (parent != null)
                parent.SizeChanged += new EventHandler(ParentSizeChanged);
        }

        private void ParentSizeChanged(object sender, EventArgs e) =>
            UpdatePosition();
    }
}
