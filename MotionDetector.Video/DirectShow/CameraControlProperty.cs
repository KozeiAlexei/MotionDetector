namespace MotionDetector.Video.DirectShow
{
    using System;

    
    
    
    public enum CameraControlProperty
    {
        
        
        
        Pan = 0,
        
        
        
        Tilt,
        
        
        
        Roll,
        
        
        
        Zoom,
        
        
        
        Exposure,
        
        
        
        Iris,
        
        
        
        Focus
    }

    
    
    
    [Flags]
    public enum CameraControlFlags
    {
        
        
        
        None = 0x0,
        
        
        
        Auto = 0x0001,
        
        
        
        Manual = 0x0002
    }
}
