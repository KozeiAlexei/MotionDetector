







namespace MotionDetector
{
    using System;

    
    
    
    
    
    
    
    
    public class ConnectionFailedException : Exception
    {
        
        
        
        
        
        
        public ConnectionFailedException( string message ) :
            base( message ) { }
    }

    
    
    
    
    
    
    
    
    public class ConnectionLostException : Exception
    {
        
        
        
        
        
        
        public ConnectionLostException( string message ) :
            base( message ) { }
    }

    
    
    
    
    
    
    
    
    public class NotConnectedException : Exception
    {
        
        
        
        
        
        
        public NotConnectedException( string message ) :
            base( message ) { }
    }

    
    
    
    
    
    
    
    
    public class DeviceBusyException : Exception
    {
        
        
        
        
        
        
        public DeviceBusyException( string message ) :
            base( message ) { }
    }

    
    
    
    
    
    
    
    public class DeviceErrorException : Exception
    {
        
        
        
        
        
        
        public DeviceErrorException( string message ) :
            base( message ) { }
    }
}
