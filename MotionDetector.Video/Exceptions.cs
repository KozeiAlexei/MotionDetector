







namespace MotionDetector.Imaging
{
    using System;

    
    
    
    
    
    
    
    
    
    
    public class UnsupportedImageFormatException : ArgumentException
    {
        
        
        
        public UnsupportedImageFormatException( ) { }

        
        
        
        
        
        
        public UnsupportedImageFormatException( string message ) :
            base( message ) { }

        
        
        
        
        
        
        
        public UnsupportedImageFormatException( string message, string paramName ) :
            base( message, paramName ) { }
    }

    
    
    
    
    
    
    
    
    
    
    
    public class InvalidImagePropertiesException : ArgumentException
    {
        
        
        
        public InvalidImagePropertiesException( ) { }

        
        
        
        
        
        
        public InvalidImagePropertiesException( string message ) :
            base( message ) { }

        
        
        
        
        
        
        
        public InvalidImagePropertiesException( string message, string paramName ) :
            base( message, paramName ) { }
    }

    
    internal static class ExceptionMessage
    {
        public const string ColorHistogramException = "Cannot access color histogram since the last processed image was grayscale.";
        public const string GrayHistogramException = "Cannot access gray histogram since the last processed image was color.";
    }
}