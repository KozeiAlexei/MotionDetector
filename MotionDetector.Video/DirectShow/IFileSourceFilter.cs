namespace MotionDetector.Video.DirectShow.Internals
{
    using System;
    using System.Runtime.InteropServices;

    
    
    
    
    [ComImport,
    Guid( "56A868A6-0Ad4-11CE-B03A-0020AF0BA770" ),
    InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    internal interface IFileSourceFilter
    {
        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int Load( [In, MarshalAs( UnmanagedType.LPWStr )] string fileName,
            [In, MarshalAs( UnmanagedType.LPStruct )] AMMediaType mediaType );

        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int GetCurFile([Out, MarshalAs( UnmanagedType.LPWStr )] out string fileName,
            [Out, MarshalAs( UnmanagedType.LPStruct )] AMMediaType mediaType );
    }
}
