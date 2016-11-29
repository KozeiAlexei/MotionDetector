using System.Drawing;
using System.Drawing.Imaging;

using MotionDetector.Imaging;

namespace MotionDetector.Vision.Motion
{ 
    public class MotionBorderHighlighting : IMotionProcessing
    {
        private Color highlightColor = Color.Red;

        public Color HighlightColor
        {
            get { return highlightColor; }
            set { highlightColor = value; }
        }

        public MotionBorderHighlighting( ) { }

        public MotionBorderHighlighting( Color highlightColor )
        {
            this.highlightColor = highlightColor;
        }

        public unsafe void ProcessFrame( UnmanagedImage videoFrame, UnmanagedImage motionFrame )
        {
            if ( motionFrame.PixelFormat != PixelFormat.Format8bppIndexed )
            {
                throw new InvalidImagePropertiesException( "Motion frame must be 8 bpp image." );
            }

            if ( ( videoFrame.PixelFormat != PixelFormat.Format8bppIndexed ) &&
                 ( videoFrame.PixelFormat != PixelFormat.Format24bppRgb ) &&
                 ( videoFrame.PixelFormat != PixelFormat.Format32bppRgb ) &&
                 ( videoFrame.PixelFormat != PixelFormat.Format32bppArgb ) )
            {
                throw new UnsupportedImageFormatException( "Video frame must be 8 bpp grayscale image or 24/32 bpp color image." );
            } 

            int width  = videoFrame.Width;
            int height = videoFrame.Height;
            int pixelSize = Bitmap.GetPixelFormatSize( videoFrame.PixelFormat ) / 8; 

            if ( ( motionFrame.Width != width ) || ( motionFrame.Height != height ) )
                return;

            byte* src    = (byte*) videoFrame.ImageData.ToPointer( );
            byte* motion = (byte*) motionFrame.ImageData.ToPointer( );

            int srcOffset    = videoFrame.Stride  - ( width - 2 ) * pixelSize;
            int motionOffset = motionFrame.Stride - ( width - 2 );

            src    += videoFrame.Stride + pixelSize;
            motion += motionFrame.Stride + 1;

            int widthM1  = width - 1;
            int heightM1 = height - 1;

            if ( pixelSize == 1 )
            {
                byte fillG = (byte) ( 0.2125 * highlightColor.R +
                                      0.7154 * highlightColor.G +
                                      0.0721 * highlightColor.B );

                for ( int y = 1; y < heightM1; y++ )
                {
                    for ( int x = 1; x < widthM1; x++, motion++, src++ )
                    {
                        if ( 4 * *motion - motion[-width] - motion[width] - motion[1] - motion[-1] != 0 )
                        {
                            *src = fillG;
                        }
                    }

                    motion += motionOffset;
                    src += srcOffset;
                }
            }
            else
            {
                byte fillR = highlightColor.R;
                byte fillG = highlightColor.G;
                byte fillB = highlightColor.B;

                for ( int y = 1; y < heightM1; y++ )
                {
                    for ( int x = 1; x < widthM1; x++, motion++, src += pixelSize )
                    {
                        if ( 4 * *motion - motion[-width] - motion[width] - motion[1] - motion[-1] != 0 )
                        {
                            src[RGB.R] = fillR;
                            src[RGB.G] = fillG;
                            src[RGB.B] = fillB;
                        }
                    }

                    motion += motionOffset;
                    src += srcOffset;
                }
            }
        }

        public void Reset( )
        {
        }
    }
}
