namespace MotionDetector.Video.DirectShow.Internals
{
    using System;
    using System.Runtime.InteropServices;

	
	
	
    [ComVisible( false )]
    static internal class Clsid
    {
        
        
        
        
        
        
        public static readonly Guid SystemDeviceEnum =
            new Guid( 0x62BE5D10, 0x60EB, 0x11D0, 0xBD, 0x3B, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86 );

        
        
        
        
        
        
        public static readonly Guid FilterGraph =
            new Guid( 0xE436EBB3, 0x524F, 0x11CE, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70 );

        
        
        
        
        
        
        public static readonly Guid SampleGrabber =
            new Guid( 0xC1F400A0, 0x3F08, 0x11D3, 0x9F, 0x0B, 0x00, 0x60, 0x08, 0x03, 0x9E, 0x37 );

        
        
        
        
        
        
        public static readonly Guid CaptureGraphBuilder2 =
            new Guid( 0xBF87B6E1, 0x8C27, 0x11D0, 0xB3, 0xF0, 0x00, 0xAA, 0x00, 0x37, 0x61, 0xC5 );

        
        
        
        
        
        
        public static readonly Guid AsyncReader =
            new Guid( 0xE436EBB5, 0x524F, 0x11CE, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70 );
    }

    
    
    
    
    [ComVisible( false )]
    static internal class FormatType
    {
        
        
        
        
        
        
        public static readonly Guid VideoInfo =
            new Guid( 0x05589F80, 0xC356, 0x11CE, 0xBF, 0x01, 0x00, 0xAA, 0x00, 0x55, 0x59, 0x5A );

        
        
        
        
        
        
        public static readonly Guid VideoInfo2 =
            new Guid( 0xf72A76A0, 0xEB0A, 0x11D0, 0xAC, 0xE4, 0x00, 0x00, 0xC0, 0xCC, 0x16, 0xBA );
    }

    [ComVisible(false)]
    public static class FilterCategory
    {
        
        
        
        
        
        
        public static readonly Guid AudioInputDevice =
            new Guid(0x33D9A762, 0x90C8, 0x11D0, 0xBD, 0x43, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);

        
        
        
        
        
        
        public static readonly Guid VideoInputDevice =
            new Guid(0x860BB310, 0x5D01, 0x11D0, 0xBD, 0x3B, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);

        
        
        
        
        
        
        public static readonly Guid VideoCompressorCategory =
            new Guid(0x33D9A760, 0x90C8, 0x11D0, 0xBD, 0x43, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);

        
        
        
        
        
        
        public static readonly Guid AudioCompressorCategory =
            new Guid(0x33D9A761, 0x90C8, 0x11D0, 0xBD, 0x43, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);
    }

    
    
    
    
    [ComVisible( false )]
    static internal class MediaType
    {
        
        
        
        
        
        
        public static readonly Guid Video =
            new Guid( 0x73646976, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71 );

        
        
        
        
        
        
        public static readonly Guid Interleaved =
            new Guid( 0x73766169, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71 );

        
        
        
        
        
        
        public static readonly Guid Audio =
            new Guid( 0x73647561, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71 );

        
        
        
        
        
        
        public static readonly Guid Text =
            new Guid( 0x73747874, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71 );

        
        
        
        
        
        
        public static readonly Guid Stream =
            new Guid( 0xE436EB83, 0x524F, 0x11CE, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70 );
    }

    
    
    
    
    [ComVisible( false )]
    static internal class MediaSubType
    {
        
        
        
        
        
        
        public static readonly Guid YUYV =
            new Guid( 0x56595559, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71 );

        
        
        
        
        
        
        public static readonly Guid IYUV =
            new Guid( 0x56555949, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71 );

        
        
        
        
        
        
        public static readonly Guid DVSD =
            new Guid( 0x44535644, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71 );

        
        
        
        
        
        
        public static readonly Guid RGB1 =
            new Guid( 0xE436EB78, 0x524F, 0x11CE, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70 );

        
        
        
        
        
        
        public static readonly Guid RGB4 =
            new Guid( 0xE436EB79, 0x524F, 0x11CE, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70 );

        
        
        
        
        
        
        public static readonly Guid RGB8 =
            new Guid( 0xE436EB7A, 0x524F, 0x11CE, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70 );

        
        
        
        
        
        
        public static readonly Guid RGB565 =
            new Guid( 0xE436EB7B, 0x524F, 0x11CE, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70 );

        
        
        
        
        
        
        public static readonly Guid RGB555 =
            new Guid( 0xE436EB7C, 0x524F, 0x11CE, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70 );

        
        
        
        
        
        
        public static readonly Guid RGB24 =
            new Guid( 0xE436Eb7D, 0x524F, 0x11CE, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70 );

        
        
        
        
        
        
        public static readonly Guid RGB32 =
            new Guid( 0xE436EB7E, 0x524F, 0x11CE, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70 );

        
        
        
        
        
        
        public static readonly Guid Avi =
            new Guid( 0xE436EB88, 0x524F, 0x11CE, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70 );

        
        
        
        
        
        
        public static readonly Guid Asf =
            new Guid( 0x3DB80F90, 0x9412, 0x11D1, 0xAD, 0xED, 0x00, 0x00, 0xF8, 0x75, 0x4B, 0x99 );
    }

    
    
    
    
    [ComVisible( false )]
    static internal class PinCategory
    {
        
        
        
        
        
        
        public static readonly Guid Capture =
            new Guid( 0xFB6C4281, 0x0353, 0x11D1, 0x90, 0x5F, 0x00, 0x00, 0xC0, 0xCC, 0x16, 0xBA );

        
        
        
        
        
        
        public static readonly Guid StillImage =
            new Guid( 0xFB6C428A, 0x0353, 0x11D1, 0x90, 0x5F, 0x00, 0x00, 0xC0, 0xCC, 0x16, 0xBA );
    }

    
    [ComVisible( false )]
    static internal class FindDirection
    {
        
        public static readonly Guid UpstreamOnly =
            new Guid( 0xAC798BE0, 0x98E3, 0x11D1, 0xB3, 0xF1, 0x00, 0xAA, 0x00, 0x37, 0x61, 0xC5 );

        
        public static readonly Guid DownstreamOnly =
            new Guid( 0xAC798BE1, 0x98E3, 0x11D1, 0xB3, 0xF1, 0x00, 0xAA, 0x00, 0x37, 0x61, 0xC5 );
    }
}
