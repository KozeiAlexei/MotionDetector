namespace MotionDetector.Video.DirectShow.Internals
{
    using System;
    using System.Runtime.InteropServices;
    
    
    
    
    
    
    [ComImport,
    Guid( "56A86895-0AD4-11CE-B03A-0020AF0BA770" ),
    InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    internal interface IBaseFilter
    {
        

        
        
        
        
        
        
        
        
        [PreserveSig]
        int GetClassID( [Out] out Guid ClassID );

        

        
        
        
        
        
        
        [PreserveSig]
        int Stop( );

        
        
        
        
        
        
        [PreserveSig]
        int Pause( );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int Run( long start );

        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int GetState( int milliSecsTimeout, [Out] out int filterState );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int SetSyncSource( [In] IntPtr clock );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int GetSyncSource( [Out] out IntPtr clock );

        

        
        
        
        
        
        
        
        
        [PreserveSig]
        int EnumPins( [Out] out IEnumPins enumPins );

        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int FindPin( [In, MarshalAs( UnmanagedType.LPWStr )] string id, [Out] out IPin pin );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int QueryFilterInfo( [Out] out FilterInfo filterInfo );

        
        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int JoinFilterGraph( [In] IFilterGraph graph, [In, MarshalAs( UnmanagedType.LPWStr )] string name );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int QueryVendorInfo( [Out, MarshalAs( UnmanagedType.LPWStr )] out string vendorInfo );
    }
}
