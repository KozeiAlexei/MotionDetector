






namespace MotionDetector.Imaging.Filters
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public interface IInPlacePartialFilter
    {
        
        
        
        
        
        
        
        
        
        void ApplyInPlace( Bitmap image, Rectangle rect );

        
        
        
        
        
        
        
        
        
        void ApplyInPlace( BitmapData imageData, Rectangle rect );

        
        
        
        
        
        
        
        
        
        void ApplyInPlace( UnmanagedImage image, Rectangle rect );
    }
}
