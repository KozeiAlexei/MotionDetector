namespace MotionDetector.Video.DirectShow.Internals
{
    using System;
    using System.Runtime.InteropServices;

    
    
    
    
    
    
    
    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid( "56a86897-0ad4-11ce-b03a-0020af0ba770" ),
    InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    internal interface IReferenceClock
    {
        
        
        
        
        
        
        
        
        [PreserveSig]
        int GetTime( [Out] out long pTime );

        
        
        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int AdviseTime(
            [In] long baseTime,
            [In] long streamTime,
            [In] IntPtr hEvent,
            [Out] out int pdwAdviseCookie );

        
        
        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int AdvisePeriodic(
            [In] long startTime,
            [In] long periodTime,
            [In] IntPtr hSemaphore,
            [Out] out int pdwAdviseCookie );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int Unadvise( [In] int dwAdviseCookie );
    }
}
