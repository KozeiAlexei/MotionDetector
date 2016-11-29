namespace MotionDetector.Video.DirectShow.Internals
{
    using System;
    using System.Runtime.InteropServices;

    
    
    
    
    
    [ComImport,
    Guid( "56A86893-0AD4-11CE-B03A-0020AF0BA770" ),
    InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    internal interface IEnumFilters
    {
        
        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int Next( [In] int cFilters,
            [Out, MarshalAs( UnmanagedType.LPArray, SizeParamIndex = 0 )] IBaseFilter[] filters,
            [Out] out int filtersFetched );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int Skip( [In] int cFilters );

        
        
        
        
        
        
        [PreserveSig]
        int Reset( );

        
        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int Clone( [Out] out IEnumFilters enumFilters );
    }
}
