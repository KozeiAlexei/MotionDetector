namespace MotionDetector.Video.DirectShow.Internals
{
    using System;
    using System.Runtime.InteropServices;

    
    
    
    
    [ComImport,
    Guid("0000010c-0000-0000-C000-000000000046"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    internal interface IPersist
    {
        
        
        
        
        
        [PreserveSig]
        int GetClassID([Out] out Guid pClassID);
    }
}
