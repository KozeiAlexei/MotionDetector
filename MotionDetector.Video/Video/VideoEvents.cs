
using System;

namespace MotionDetector.Video
{
    public delegate void NewFrameEventHandler(object sender, NewFrameEventArgs eventArgs);

    public delegate void VideoSourceErrorEventHandler(object sender, VideoSourceErrorEventArgs eventArgs);

    public delegate void PlayingFinishedEventHandler(object sender, ReasonToFinishPlaying reason);

    public enum ReasonToFinishPlaying
    {
        
        
        
        EndOfStreamReached,
        
        
        
        StoppedByUser,
        
        
        
        DeviceLost,
        
        
        
        
        VideoSourceError
    }

    public class NewFrameEventArgs : EventArgs
    {
        private System.Drawing.Bitmap frame;

        public NewFrameEventArgs(System.Drawing.Bitmap frame)
        {
            this.frame = frame;
        }

        public System.Drawing.Bitmap Frame
        {
            get { return frame; }
        }
    }

    public class VideoSourceErrorEventArgs : EventArgs
    {
        private string description;

        public VideoSourceErrorEventArgs(string description)
        {
            this.description = description;
        }

        public string Description
        {
            get { return description; }
        }
    }
}
