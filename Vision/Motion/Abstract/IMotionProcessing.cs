using MotionDetector.Imaging;

namespace MotionDetector.Vision.Motion
{
    public interface IMotionProcessing
    {
        void ProcessFrame(UnmanagedImage videoFrame, UnmanagedImage motionFrame);

        void Reset();
    }
}
