using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MotionDetector.Imaging.Filters
{
    public sealed class Subtract : BaseInPlaceFilter2
    {
        private Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>( );

        public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
        {
            get { return formatTranslations; }
        }

        public Subtract( )
        {
            InitFormatTranslations( );
        }

        public Subtract( Bitmap overlayImage )
            : base( overlayImage )
        {
            InitFormatTranslations( );
        }

        public Subtract( UnmanagedImage unmanagedOverlayImage )
            : base( unmanagedOverlayImage )
        {
            InitFormatTranslations( );
        }
    
        private void InitFormatTranslations( )
        {
            formatTranslations[PixelFormat.Format8bppIndexed]    = PixelFormat.Format8bppIndexed;
            formatTranslations[PixelFormat.Format24bppRgb]       = PixelFormat.Format24bppRgb;
            formatTranslations[PixelFormat.Format32bppRgb]       = PixelFormat.Format32bppRgb;
            formatTranslations[PixelFormat.Format32bppArgb]      = PixelFormat.Format32bppArgb;
            formatTranslations[PixelFormat.Format16bppGrayScale] = PixelFormat.Format16bppGrayScale;
            formatTranslations[PixelFormat.Format48bppRgb]       = PixelFormat.Format48bppRgb;
            formatTranslations[PixelFormat.Format64bppArgb]      = PixelFormat.Format64bppArgb;
        }

        protected override unsafe void ProcessFilter( UnmanagedImage image, UnmanagedImage overlay )
        {
            PixelFormat pixelFormat = image.PixelFormat;
  
            int width  = image.Width;
            int height = image.Height;

            if (
                ( pixelFormat == PixelFormat.Format8bppIndexed ) ||
                ( pixelFormat == PixelFormat.Format24bppRgb ) ||
                ( pixelFormat == PixelFormat.Format32bppRgb ) ||
                ( pixelFormat == PixelFormat.Format32bppArgb ) )
            {

                int pixelSize = ( pixelFormat == PixelFormat.Format8bppIndexed ) ? 1 :
                    ( pixelFormat == PixelFormat.Format24bppRgb ) ? 3 : 4;
                int lineSize  = width * pixelSize;
                int srcOffset = image.Stride - lineSize;
                int ovrOffset = overlay.Stride - lineSize;

                int v;

                byte * ptr = (byte*) image.ImageData.ToPointer( );
                byte * ovr = (byte*) overlay.ImageData.ToPointer( );

                for ( int y = 0; y < height; y++ )
                {

                    for ( int x = 0; x < lineSize; x++, ptr++, ovr++ )
                    {
                        v = (int) *ptr - (int) *ovr;
                        *ptr = ( v < 0 ) ? (byte) 0 : (byte) v;
                    }
                    ptr += srcOffset;
                    ovr += ovrOffset;
                }
            }
            else
            {
                int pixelSize = ( pixelFormat == PixelFormat.Format16bppGrayScale ) ? 1 :
                    ( pixelFormat == PixelFormat.Format48bppRgb ) ? 3 : 4;
                int lineSize  = width * pixelSize;
                int srcStride = image.Stride;
                int ovrStride = overlay.Stride;

                int v;

                byte* basePtr = (byte*) image.ImageData.ToPointer( );
                byte* baseOvr = (byte*) overlay.ImageData.ToPointer( );

                for ( int y = 0; y < height; y++ )
                {
                    ushort * ptr = (ushort*) ( basePtr + y * srcStride );
                    ushort * ovr = (ushort*) ( baseOvr + y * ovrStride );

                    for ( int x = 0; x < lineSize; x++, ptr++, ovr++ )
                    {
                        v = (int) *ptr - (int) *ovr;
                        *ptr = ( v < 0 ) ? (ushort) 0 : (ushort) v;
                    }
                }
            }
        }
    }
}
