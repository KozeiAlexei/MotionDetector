







namespace MotionDetector.Imaging.Filters
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    
    
    
    
    
    
    
    
    
    
    
    
    public interface IFilter
    {
        
        
        
        
        
        
        
        
        
        
        
        
        Bitmap Apply( Bitmap image );

        
        
        
        
        
        
        
        
        
        
        
        
        
        Bitmap Apply( BitmapData imageData );

        
        
        
        
        
        
        
        
        
        
        
        
        UnmanagedImage Apply( UnmanagedImage image );

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        void Apply( UnmanagedImage sourceImage, UnmanagedImage destinationImage );
    }
}
