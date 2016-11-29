






namespace MotionDetector.Imaging.Filters
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public interface IInPlaceFilter
    {
        
        
        
        
        
        
        
        
        void ApplyInPlace( Bitmap image );

        
        
        
        
        
        
        
        
        void ApplyInPlace( BitmapData imageData );

        
        
        
        
        
        
        
        
        void ApplyInPlace( UnmanagedImage image );
    }
}

