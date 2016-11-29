using MotionDetector.Imaging;

namespace MotionDetector.Vision.Motion
{
    public interface IMotionDetector
    {
        float MotionLevel { get; }

        UnmanagedImage MotionFrame { get; }

        void ProcessFrame(UnmanagedImage videoFrame);

        void Reset();
    }
}
