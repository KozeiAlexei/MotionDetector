namespace MotionDetector.Video.DirectShow
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;

    using MotionDetector.Video;
    using MotionDetector.Video.DirectShow.Internals;

    
    
    
    public class VideoCapabilities
    {
        
        
        
        public readonly Size FrameSize;

        
        
        
        
        
        
        
        
        [Obsolete( "No longer supported. Use AverageFrameRate instead." )]
        public int FrameRate
        {
            get { return AverageFrameRate; }
        }

        
        
        
        public readonly int AverageFrameRate;

        
        
        
        public readonly int MaximumFrameRate;

        
        
        
        public readonly int BitCount;

        internal VideoCapabilities( ) { }

        
        static internal VideoCapabilities[] FromStreamConfig( IAMStreamConfig videoStreamConfig )
        {
            if ( videoStreamConfig == null )
                throw new ArgumentNullException( "videoStreamConfig" );

            
            int count, size;
            int hr = videoStreamConfig.GetNumberOfCapabilities( out count, out size );

            if ( hr != 0 )
                Marshal.ThrowExceptionForHR( hr );

            if ( count <= 0 )
                throw new NotSupportedException( "This video device does not report capabilities." );

            if ( size > Marshal.SizeOf( typeof( VideoStreamConfigCaps ) ) )
                throw new NotSupportedException( "Unable to retrieve video device capabilities. This video device requires a larger VideoStreamConfigCaps structure." );

            
            Dictionary<uint, VideoCapabilities> videocapsList = new Dictionary<uint, VideoCapabilities>( );

            for ( int i = 0; i < count; i++ )
            {
                try
                {
                    VideoCapabilities vc = new VideoCapabilities( videoStreamConfig, i );

                    uint key = ( ( (uint) vc.FrameSize.Height ) << 32 ) |
                               ( ( (uint) vc.FrameSize.Width ) << 16 );

                    if ( !videocapsList.ContainsKey( key ) )
                    {
                        videocapsList.Add( key, vc );
                    }
                    else
                    {
                        if ( vc.BitCount > videocapsList[key].BitCount )
                        {
                            videocapsList[key] = vc;
                        }
                    }
                }
                catch
                {
                }
            }

            VideoCapabilities[] videocaps = new VideoCapabilities[videocapsList.Count];
            videocapsList.Values.CopyTo( videocaps, 0 );

            return videocaps;
        }

        
        internal VideoCapabilities( IAMStreamConfig videoStreamConfig, int index )
        {
            AMMediaType mediaType = null;
            VideoStreamConfigCaps caps = new VideoStreamConfigCaps( );

            try
            {
                
                int hr = videoStreamConfig.GetStreamCaps( index, out mediaType, caps );

                if ( hr != 0 )
                    Marshal.ThrowExceptionForHR( hr );

                if ( mediaType.FormatType == FormatType.VideoInfo )
                {
                    VideoInfoHeader videoInfo = (VideoInfoHeader) Marshal.PtrToStructure( mediaType.FormatPtr, typeof( VideoInfoHeader ) );

                    FrameSize = new Size( videoInfo.BmiHeader.Width, videoInfo.BmiHeader.Height );
                    BitCount = videoInfo.BmiHeader.BitCount;
                    AverageFrameRate = (int) ( 10000000 / videoInfo.AverageTimePerFrame );
                    MaximumFrameRate = (int) ( 10000000 / caps.MinFrameInterval );
                }
                else if ( mediaType.FormatType == FormatType.VideoInfo2 )
                {
                    VideoInfoHeader2 videoInfo = (VideoInfoHeader2) Marshal.PtrToStructure( mediaType.FormatPtr, typeof( VideoInfoHeader2 ) );

                    FrameSize = new Size( videoInfo.BmiHeader.Width, videoInfo.BmiHeader.Height );
                    BitCount = videoInfo.BmiHeader.BitCount;
                    AverageFrameRate = (int) ( 10000000 / videoInfo.AverageTimePerFrame );
                    MaximumFrameRate = (int) ( 10000000 / caps.MinFrameInterval );
                }
                else
                {
                    throw new ApplicationException( "Unsupported format found." );
                }

                
                
                
                if ( BitCount <= 12 )
                {
                    throw new ApplicationException( "Unsupported format found." );
                }
            }
            finally
            {
                if ( mediaType != null )
                    mediaType.Dispose( );
            }
        }

        
        
        
        
        
        
        
        
        public override bool Equals( object obj )
        {
            return Equals( obj as VideoCapabilities );
        }

        
        
        
        
        
        
        
        
        public bool Equals( VideoCapabilities vc2 )
        {
            if ( (object) vc2 == null )
            {
                return false;
            }

            return ( ( FrameSize == vc2.FrameSize ) && ( BitCount == vc2.BitCount ) );
        }

        
        
        
        
        
        public override int GetHashCode( )
        {
            return FrameSize.GetHashCode( ) ^ BitCount;
        }

        
        
        
        
        
        
        
        
        public static bool operator ==( VideoCapabilities a, VideoCapabilities b )
        {
            
            if ( object.ReferenceEquals( a, b ) )
            {
                return true;
            }

            
            if ( ( (object) a == null ) || ( (object) b == null ) )
            {
                return false;
            }

            return a.Equals( b );
        }

        
        
        
        
        
        
        
        
        public static bool operator !=( VideoCapabilities a, VideoCapabilities b )
        {
            return !( a == b );
        }
    }
}
