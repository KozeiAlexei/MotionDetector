using MotionDetector;
using MotionDetector.Imaging;
using MotionDetector.Video;
using MotionDetector.Video.DirectShow;
using MotionDetector.Video.DirectShow.Internals;
using MotionDetector.Vision.Motion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MotionDetectorN
{
    public partial class MainForm : Form
    {
        private object sync_context = new object();

        private MotionDetector.Vision.Motion.MotionDetector detector = new MotionDetector.Vision.Motion.MotionDetector(new TwoFramesDifferenceDetector(), new MotionAreaHighlighting());

        
        private const int statLength = 15;
        
        private int statIndex = 0;
        
        private int statReady = 0;
        
        private int[] statCount = new int[statLength];

        
        private float motionAlarmLevel = 0.015f; float fps = 1.0f;

        private List<float> motionHistory = new List<float>();

        private string SelectedVideoSource { get; set; }
        private FilterInfoCollection VideoSource { get; } = new FilterInfoCollection(FilterCategory.VideoInputDevice);
       
        public MainForm()
        {
            InitializeComponent();

            try
            {
                if (VideoSource.Count == 0)
                    throw new InvalidOperationException("Не найден ни один источник видео!");

                foreach (FilterInfo device in VideoSource)
                    VideoSourceChanger.Items.Add(device.Name);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SettingsGroupBox.Enabled = false;
            }

            VideoSourceChanger.SelectedIndex = 0;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (VideoSourceChanger.SelectedIndex >= 0)
            {
                StatusProgressBar.Style = ProgressBarStyle.Marquee;
                SelectedVideoSource = VideoSource[VideoSourceChanger.SelectedIndex].MonikerString;
            
                OpenVideoSource(new VideoCaptureDevice(SelectedVideoSource));
            }
            else
                MessageBox.Show("Выберите источник видео!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DrawMotionHistory(Bitmap image)
        {
            var redColor = Color.FromArgb(128, 255, 0, 0);
            var greenColor = Color.FromArgb(128, 0, 255, 0);
            var yellowColor = Color.FromArgb(128, 255, 255, 0);

            var bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

            var threshold1 = (int)(motionAlarmLevel * 500);
            var threshold2 = (int)(0.075 * 500);

            for (int i = 1, n = motionHistory.Count; i <= n; i++)
            {
                int motionBarLength = (int)(motionHistory[n - i] * 500);

                if (motionBarLength == 0)
                    continue;

                if (motionBarLength > 50)
                    motionBarLength = 50;

                Drawing.Line(bitmapData, new IntPoint(image.Width - i, image.Height - 1), 
                                         new IntPoint(image.Width - i, image.Height - 1 - motionBarLength), greenColor);

                if (motionBarLength > threshold1)
                {
                    Drawing.Line(bitmapData, new IntPoint(image.Width - i, image.Height - 1 - threshold1),
                                             new IntPoint(image.Width - i, image.Height - 1 - motionBarLength), yellowColor);
                }

                if (motionBarLength > threshold2)
                {
                    Drawing.Line(bitmapData, new IntPoint(image.Width - i, image.Height - 1 - threshold2),
                                             new IntPoint(image.Width - i, image.Height - 1 - motionBarLength), redColor);
                }
            }

            image.UnlockBits(bitmapData);
        }

        private void OpenVideoSource(IVideoSource source)
        {
            Cursor = Cursors.WaitCursor;

            CloseVideoSource();

            FPSTimer.Start();

            VideoPlayer.VideoSource = new AsyncVideoSource(source);
            VideoPlayer.Start();

            Cursor = Cursors.Default;
        }

        private void SetMotionDetectionAlgorithm(IMotionDetector detectionAlgorithm)
        {
            lock (sync_context)
            {
                motionHistory.Clear();

                detector.MotionDetectionAlgorithm = detectionAlgorithm;

                if (detectionAlgorithm is TwoFramesDifferenceDetector)
                {
                    if (detector.MotionProcessingAlgorithm is MotionBorderHighlighting)
                        SetMotionProcessingAlgorithm(new MotionAreaHighlighting());
                }
            }
        }

        private void SetMotionProcessingAlgorithm(IMotionProcessing processingAlgorithm)
        {
            lock (sync_context)
            {
                detector.MotionProcessingAlgorithm = processingAlgorithm;
            }
        }

        private void MotionSignalize(float motionLevel) => Task.Factory.StartNew(() => 
            Console.Beep(Convert.ToInt32(motionLevel * 10000) % 30000 + 40, (int)Math.Abs(1000.0f / (fps + 1))));

        private float ClaimMotionLevel(float real) => (real < 0.001f) ? 0.01f : real;

        private void VideoPlayer_NewFrame(object sender, ref Bitmap image)
        {
            lock (sync_context)
            {
                if (detector != null)
                {
                    var motionLevel = ClaimMotionLevel(detector.ProcessFrame(image));

                    if (motionLevel > motionAlarmLevel)
                    {
                        if (SoundNotificationCheckBox.Checked)
                            MotionSignalize(motionLevel);
                    }

                    UpdateHistory(motionLevel, ref image);
                }
            }
        }

        private void UpdateHistory(float motionLevel, ref Bitmap image)
        {
            motionHistory.Add(motionLevel);
            if (motionHistory.Count > 1000)
                motionHistory.RemoveAt(0);

            if (MotionHistoryCheckbox.Checked)
                DrawMotionHistory(image);
        }

        private void CloseVideoSource()
        {
            Cursor = Cursors.WaitCursor;

            VideoPlayer.SignalToStop();

            for (int i = 0; (i < 50) && (VideoPlayer.IsRunning); i++)
                Thread.Sleep(100);

            if (VideoPlayer.IsRunning)
                VideoPlayer.Stop();

            FPSTimer.Stop();

            motionHistory.Clear();

            if (detector != null)
                detector.Reset();

            VideoPlayer.BorderColor = Color.Black;

            Cursor = Cursors.Default;
        }

        private void StopDetectionButton_Click(object sender, EventArgs e)
        {
            CloseVideoSource();
            StatusProgressBar.Style = ProgressBarStyle.Blocks;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseVideoSource();
        }

        private void FPSTimerTick(object sender, EventArgs e)
        {
            var videoSource = VideoPlayer.VideoSource;

            if (videoSource != null)
            {
                statCount[statIndex] = videoSource.FramesReceived;

                if (++statIndex >= statLength)
                    statIndex = 0;

                if (statReady < statLength)
                    statReady++;

                var fpsLoc = 0.0f;

                for (int i = 0; i < statReady; i++)
                    fpsLoc += statCount[i];

                fpsLoc /= statReady;

                fps = fpsLoc;

                statCount[statIndex] = 0;

                FPSLabel.Text = $"FPS: { fps.ToString("F2") }";
            }
        }

        private void NoMotionDetectorRadioButton_CheckedChanged(object sender, EventArgs e) =>
            SetMotionDetectionAlgorithm(null);

        private void FrameDifferenceMotionDetectorRadioButton_CheckedChanged(object sender, EventArgs e) =>
            SetMotionDetectionAlgorithm(new TwoFramesDifferenceDetector());

        private void BackgroundModelingMotionDetectorRadioButton_CheckedChanged(object sender, EventArgs e) =>
            SetMotionDetectionAlgorithm(new SimpleBackgroundModelingDetector(true, true));

        private void NoMotionProcessingRadioButton_CheckedChanged(object sender, EventArgs e) =>
            SetMotionProcessingAlgorithm(null);

        private void AreaHighlightMotionProcessingRadioButton_CheckedChanged(object sender, EventArgs e) =>
            SetMotionProcessingAlgorithm(new MotionAreaHighlighting());

        private void BorderHighlightMotionProcessingRadioButton_CheckedChanged(object sender, EventArgs e) =>
            SetMotionProcessingAlgorithm(new MotionBorderHighlighting());

        private void GridMotionProcessingRadioButton_CheckedChanged(object sender, EventArgs e) =>
            SetMotionProcessingAlgorithm(new GridMotionAreaProcessing(32, 32));
    }
}
