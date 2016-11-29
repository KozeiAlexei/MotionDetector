namespace MotionDetector.Video.DirectShow.Internals
{
    using System;
    using System.Runtime.InteropServices;

    
    
    
    
    
    [ComImport,
    Guid( "55272A00-42CB-11CE-8135-00AA004BB851" ),
    InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    internal interface IPropertyBag
    {
        
        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int Read(
            [In, MarshalAs( UnmanagedType.LPWStr )] string propertyName,
            [In, Out, MarshalAs( UnmanagedType.Struct )] ref object pVar,
            [In] IntPtr pErrorLog );

        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int Write(
            [In, MarshalAs( UnmanagedType.LPWStr )] string propertyName,
            [In, MarshalAs( UnmanagedType.Struct )] ref object pVar );
    }
}
