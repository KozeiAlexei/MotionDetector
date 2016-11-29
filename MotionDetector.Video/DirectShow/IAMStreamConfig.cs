namespace MotionDetector.Video.DirectShow.Internals
{
    using System;
    using System.Runtime.InteropServices;

    
    
    
    
    
    [ComImport,
    Guid( "C6E13340-30AC-11d0-A18C-00A0C9118956" ),
    InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    internal interface IAMStreamConfig
    {
        
        
        
        
        
        
        
        
        [PreserveSig]
        int SetFormat( [In, MarshalAs( UnmanagedType.LPStruct )] AMMediaType mediaType );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int GetFormat( [Out, MarshalAs( UnmanagedType.LPStruct )] out AMMediaType mediaType );

        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int GetNumberOfCapabilities( out int count, out int size );

        
        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int GetStreamCaps(
            [In] int index,
            [Out, MarshalAs( UnmanagedType.LPStruct )] out AMMediaType mediaType,
            [In, MarshalAs( UnmanagedType.LPStruct )] VideoStreamConfigCaps streamConfigCaps
            );
    }
}
