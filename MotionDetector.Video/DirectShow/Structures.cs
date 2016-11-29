namespace MotionDetector.Video.DirectShow.Internals
{
    using System;
    using System.Runtime.InteropServices;
    using System.Drawing;

    

    
    
    
    
    [ComVisible( false )]
    internal enum PinDirection
    {
        
        
        
        Input,

        
        
        
        Output
    }

    

    
    
    
    
    [ComVisible( false ),
    StructLayout( LayoutKind.Sequential )]
    internal class AMMediaType : IDisposable
    {
        
        
        
        public Guid MajorType;

        
        
        
        public Guid SubType;

        
        
        
        [MarshalAs( UnmanagedType.Bool )]
        public bool FixedSizeSamples = true;

        
        
        
        [MarshalAs( UnmanagedType.Bool )]
        public bool TemporalCompression;

        
        
        
        public int SampleSize = 1;

        
        
        
        public Guid FormatType;

        
        
        
        public IntPtr unkPtr;

        
        
        
        public int FormatSize;

        
        
        
        public IntPtr FormatPtr;

        
        
        
        
        ~AMMediaType( )
        {
            Dispose( false );
        }

        
        
        
        
        public void Dispose( )
        {
            Dispose( true );
            
            GC.SuppressFinalize( this );
        }

        
        
        
        
        
        
        protected virtual void Dispose( bool disposing )
        {
            if ( ( FormatSize != 0 ) && ( FormatPtr != IntPtr.Zero ) )
            {
                Marshal.FreeCoTaskMem( FormatPtr );
                FormatSize = 0;
            }

            if ( unkPtr != IntPtr.Zero )
            {
                Marshal.Release( unkPtr );
                unkPtr = IntPtr.Zero;
            }
        }
    }


    

    
    
    
    
    [ComVisible( false ),
    StructLayout( LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode )]
    internal struct PinInfo
    {
        
        
        
        public IBaseFilter Filter;

        
        
        
        public PinDirection Direction;

        
        
        
        [MarshalAs( UnmanagedType.ByValTStr, SizeConst = 128 )]
        public string Name;
    }

    
    [ComVisible( false ),
    StructLayout( LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode )]
    internal struct FilterInfo
    {
        
        
        
        [MarshalAs( UnmanagedType.ByValTStr, SizeConst = 128 )]
        public string Name;

        
        
        
        public IFilterGraph FilterGraph;
    }

    

    
    
    
    
    [ComVisible( false ),
    StructLayout( LayoutKind.Sequential )]
    internal struct VideoInfoHeader
    {
        
        
        
        public RECT SrcRect;

        
        
        
        public RECT TargetRect;

        
        
        
        public int BitRate;

        
        
        
        public int BitErrorRate;

        
        
        
        public long AverageTimePerFrame;

        
        
        
        public BitmapInfoHeader BmiHeader;
    }

    

    
    
    
    
    [ComVisible( false ),
    StructLayout( LayoutKind.Sequential )]
    internal struct VideoInfoHeader2
    {
        
        
        
        public RECT SrcRect;

        
        
        
        public RECT TargetRect;

        
        
        
        public int BitRate;

        
        
        
        public int BitErrorRate;

        
        
        
        public long AverageTimePerFrame;

        
        
        
        public int InterlaceFlags;

        
        
        
        public int CopyProtectFlags;

        
        
        
        public int PictAspectRatioX;

        
        
        
        public int PictAspectRatioY;

        
        
        
        public int Reserved1;

        
        
        
        public int Reserved2;

        
        
        
        public BitmapInfoHeader BmiHeader;
    }

    
    
    
    
    [ComVisible( false ),
    StructLayout( LayoutKind.Sequential, Pack = 2 )]
    internal struct BitmapInfoHeader
    {
        
        
        
        public int Size;

        
        
        
        public int Width;

        
        
        
        public int Height;

        
        
        
        public short Planes;

        
        
        
        public short BitCount;

        
        
        
        public int Compression;

        
        
        
        public int ImageSize;

        
        
        
        public int XPelsPerMeter;

        
        
        
        public int YPelsPerMeter;

        
        
        
        public int ColorsUsed;

        
        
        
        public int ColorsImportant;
    }

    

    
    
    
    
    [ComVisible( false ),
    StructLayout( LayoutKind.Sequential )]
    internal struct RECT
    {
        
        
        
        public int Left;

        
        
        
        public int Top;

        
        
        
        public int Right;

        
        
        
        public int Bottom;
    }

    

    
    
    
    
    [ComVisible( false ),
    StructLayout( LayoutKind.Sequential )]
    internal struct CAUUID
    {
        
        
        
        public int cElems;

        
        
        
        public IntPtr pElems;

        
        
        
        
        
        
        public Guid[] ToGuidArray( )
        {
            Guid[] retval = new Guid[cElems];

            for ( int i = 0; i < cElems; i++ )
            {
                IntPtr ptr = new IntPtr( pElems.ToInt64( ) + i * Marshal.SizeOf( typeof( Guid ) ) );
                retval[i] = (Guid) Marshal.PtrToStructure( ptr, typeof( Guid ) );
            }

            return retval;
        }
    }

    
    
    
    internal enum DsEvCode
    {
        None,
        Complete   = 0x01,      
        DeviceLost = 0x1F,      
        
    }

    [Flags, ComVisible( false )]
    internal enum AnalogVideoStandard
    {
        None        = 0x00000000,   
        NTSC_M      = 0x00000001,   
        NTSC_M_J    = 0x00000002,   
        NTSC_433    = 0x00000004,
        PAL_B       = 0x00000010,
        PAL_D       = 0x00000020,
        PAL_G       = 0x00000040,
        PAL_H       = 0x00000080,
        PAL_I       = 0x00000100,
        PAL_M       = 0x00000200,
        PAL_N       = 0x00000400,
        PAL_60      = 0x00000800,
        SECAM_B     = 0x00001000,
        SECAM_D     = 0x00002000,
        SECAM_G     = 0x00004000,
        SECAM_H     = 0x00008000,
        SECAM_K     = 0x00010000,
        SECAM_K1    = 0x00020000,
        SECAM_L     = 0x00040000,
        SECAM_L1    = 0x00080000,
        PAL_N_COMBO = 0x00100000    
    }

    [Flags, ComVisible( false )]
    internal enum VideoControlFlags
    {
        FlipHorizontal        = 0x0001,
        FlipVertical          = 0x0002,
        ExternalTriggerEnable = 0x0004,
        Trigger               = 0x0008
    }

    [StructLayout( LayoutKind.Sequential ), ComVisible( false )]
    internal class VideoStreamConfigCaps		
    {
        public Guid                 Guid;
        public AnalogVideoStandard  VideoStandard;
        public Size                 InputSize;
        public Size                 MinCroppingSize;
        public Size                 MaxCroppingSize;
        public int                  CropGranularityX;
        public int                  CropGranularityY;
        public int                  CropAlignX;
        public int                  CropAlignY;
        public Size                 MinOutputSize;
        public Size                 MaxOutputSize;
        public int                  OutputGranularityX;
        public int                  OutputGranularityY;
        public int                  StretchTapsX;
        public int                  StretchTapsY;
        public int                  ShrinkTapsX;
        public int                  ShrinkTapsY;
        public long                 MinFrameInterval;
        public long                 MaxFrameInterval;
        public int                  MinBitsPerSecond;
        public int                  MaxBitsPerSecond;
    }

    
    
    
    internal enum FilterState
    {
        
        
        
        State_Stopped,

        
        
        
        State_Paused,

        
        
        
        State_Running
    }
}
