







namespace MotionDetector.Imaging
{
    using System;
    using System.IO;
    using System.Drawing;
    using System.Drawing.Imaging;
    using MotionDetector;

    
    
    
    
    
    
    
    public static class Image
    {
        
        
        
        
        
        
        
        
        
        
        
        
        
        public static bool IsGrayscale( Bitmap image )
        {
            bool ret = false;

            
            if ( image.PixelFormat == PixelFormat.Format8bppIndexed )
            {
                ret = true;
                
                ColorPalette cp = image.Palette;
                Color c;
                
                for ( int i = 0; i < 256; i++ )
                {
                    c = cp.Entries[i];
                    if ( ( c.R != i ) || ( c.G != i ) || ( c.B != i ) )
                    {
                        ret = false;
                        break;
                    }
                }
            }
            return ret;
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public static Bitmap CreateGrayscaleImage( int width, int height )
        {
            
            Bitmap image = new Bitmap( width, height, PixelFormat.Format8bppIndexed );
            
            SetGrayscalePalette( image );
            
            return image;
        }

        
        
        
        
        
        
        
        
        
        
        
        
        public static void SetGrayscalePalette( Bitmap image )
        {
            
            if ( image.PixelFormat != PixelFormat.Format8bppIndexed )
                throw new UnsupportedImageFormatException( "Source image is not 8 bpp image." );

            
            ColorPalette cp = image.Palette;
            
            for ( int i = 0; i < 256; i++ )
            {
                cp.Entries[i] = Color.FromArgb( i, i, i );
            }
            
            image.Palette = cp;
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public static Bitmap Clone( Bitmap source, PixelFormat format )
        {
            
            if ( source.PixelFormat == format )
                return Clone( source );

            int width = source.Width;
            int height = source.Height;

            
            Bitmap bitmap = new Bitmap( width, height, format );

            
            Graphics g = Graphics.FromImage( bitmap );
            g.DrawImage( source, 0, 0, width, height );
            g.Dispose( );

            return bitmap;
        }

        
        
        
        
        
        
        
        
        
        
        
        
        public static Bitmap Clone( Bitmap source )
        {
            
            BitmapData sourceData = source.LockBits(
                new Rectangle( 0, 0, source.Width, source.Height ),
                ImageLockMode.ReadOnly, source.PixelFormat );

            
            Bitmap destination = Clone( sourceData );

            
            source.UnlockBits( sourceData );

            
            if (
                ( source.PixelFormat == PixelFormat.Format1bppIndexed ) ||
                ( source.PixelFormat == PixelFormat.Format4bppIndexed ) ||
                ( source.PixelFormat == PixelFormat.Format8bppIndexed ) ||
                ( source.PixelFormat == PixelFormat.Indexed ) )
            {
                ColorPalette srcPalette = source.Palette;
                ColorPalette dstPalette = destination.Palette;

                int n = srcPalette.Entries.Length;

                
                for ( int i = 0; i < n; i++ )
                {
                    dstPalette.Entries[i] = srcPalette.Entries[i];
                }

                destination.Palette = dstPalette;
            }

            return destination;
        }

        
        
        
        
        
        
        
        
        
        public static Bitmap Clone( BitmapData sourceData )
        {
            
            int width = sourceData.Width;
            int height = sourceData.Height;

            
            Bitmap destination = new Bitmap( width, height, sourceData.PixelFormat );

            
            BitmapData destinationData = destination.LockBits(
                new Rectangle( 0, 0, width, height ),
                ImageLockMode.ReadWrite, destination.PixelFormat );

            MotionDetector.SystemTools.CopyUnmanagedMemory( destinationData.Scan0, sourceData.Scan0, height * sourceData.Stride );

            
            destination.UnlockBits( destinationData );

            return destination;
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        [Obsolete( "Use Clone(Bitmap, PixelFormat) method instead and specify desired pixel format" )]
        public static void FormatImage( ref Bitmap image )
        {
            if (
                ( image.PixelFormat != PixelFormat.Format24bppRgb ) &&
                ( image.PixelFormat != PixelFormat.Format32bppRgb ) &&
                ( image.PixelFormat != PixelFormat.Format32bppArgb ) &&
                ( image.PixelFormat != PixelFormat.Format48bppRgb ) &&
                ( image.PixelFormat != PixelFormat.Format64bppArgb ) &&
                ( image.PixelFormat != PixelFormat.Format16bppGrayScale ) &&
                ( IsGrayscale( image ) == false )
                )
            {
                Bitmap tmp = image;
                
                image = Clone( tmp, PixelFormat.Format24bppRgb );
                
                tmp.Dispose( );
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public static System.Drawing.Bitmap FromFile( string fileName )
        {
            Bitmap loadedImage = null;
            FileStream stream = null;

            try
            {
                
                stream = File.OpenRead( fileName );
                MemoryStream memoryStream = new MemoryStream( );

                byte[] buffer = new byte[10000];
                while ( true )
                {
                    int read = stream.Read( buffer, 0, 10000 );

                    if ( read == 0 )
                        break;

                    memoryStream.Write( buffer, 0, read );
                }

                loadedImage = (Bitmap) Bitmap.FromStream( memoryStream );
            }
            finally
            {
                if ( stream != null )
                {
                    stream.Close( );
                    stream.Dispose( );
                }
            }

            return loadedImage;
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public static Bitmap Convert16bppTo8bpp( Bitmap bimap )
        {
            Bitmap newImage = null;
            int layers = 0;

            
            int width  = bimap.Width;
            int height = bimap.Height;

            
            switch ( bimap.PixelFormat )
            {
                case PixelFormat.Format16bppGrayScale:
                    
                    newImage = CreateGrayscaleImage( width, height );
                    layers = 1;
                    break;

                case PixelFormat.Format48bppRgb:
                    
                    newImage = new Bitmap( width, height, PixelFormat.Format24bppRgb );
                    layers = 3;
                    break;

                case PixelFormat.Format64bppArgb:
                    
                    newImage = new Bitmap( width, height, PixelFormat.Format32bppArgb );
                    layers = 4;
                    break;

                case PixelFormat.Format64bppPArgb:
                    
                    newImage = new Bitmap( width, height, PixelFormat.Format32bppPArgb );
                    layers = 4;
                    break;

                default:
                    throw new UnsupportedImageFormatException( "Invalid pixel format of the source image." );
            }

            
            BitmapData sourceData = bimap.LockBits( new Rectangle( 0, 0, width, height ),
                ImageLockMode.ReadOnly, bimap.PixelFormat );
            BitmapData newData = newImage.LockBits( new Rectangle( 0, 0, width, height ),
                ImageLockMode.ReadWrite, newImage.PixelFormat );

            unsafe
            {
                
                byte* sourceBasePtr = (byte*) sourceData.Scan0.ToPointer( );
                byte* newBasePtr    = (byte*) newData.Scan0.ToPointer( );
                
                int sourceStride = sourceData.Stride;
                int newStride = newData.Stride;

                for ( int y = 0; y < height; y++ )
                {
                    ushort* sourcePtr = (ushort*) ( sourceBasePtr + y * sourceStride );
                    byte* newPtr = (byte*) ( newBasePtr + y * newStride );

                    for ( int x = 0, lineSize = width * layers; x < lineSize; x++, sourcePtr++, newPtr++ )
                    {
                        *newPtr = (byte) ( *sourcePtr >> 8 );
                    }
                }
            }

            
            bimap.UnlockBits( sourceData );
            newImage.UnlockBits( newData );

            return newImage;
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public static Bitmap Convert8bppTo16bpp( Bitmap bimap )
        {
            Bitmap newImage = null;
            int layers = 0;

            
            int width  = bimap.Width;
            int height = bimap.Height;

            
            switch ( bimap.PixelFormat )
            {
                case PixelFormat.Format8bppIndexed:
                    
                    newImage = new Bitmap( width, height, PixelFormat.Format16bppGrayScale );
                    layers = 1;
                    break;

                case PixelFormat.Format24bppRgb:
                    
                    newImage = new Bitmap( width, height, PixelFormat.Format48bppRgb );
                    layers = 3;
                    break;

                case PixelFormat.Format32bppArgb:
                    
                    newImage = new Bitmap( width, height, PixelFormat.Format64bppArgb );
                    layers = 4;
                    break;

                case PixelFormat.Format32bppPArgb:
                    
                    newImage = new Bitmap( width, height, PixelFormat.Format64bppPArgb );
                    layers = 4;
                    break;

                default:
                    throw new UnsupportedImageFormatException( "Invalid pixel format of the source image." );
            }

            
            BitmapData sourceData = bimap.LockBits( new Rectangle( 0, 0, width, height ),
                ImageLockMode.ReadOnly, bimap.PixelFormat );
            BitmapData newData = newImage.LockBits( new Rectangle( 0, 0, width, height ),
                ImageLockMode.ReadWrite, newImage.PixelFormat );

            unsafe
            {
                
                byte* sourceBasePtr = (byte*) sourceData.Scan0.ToPointer( );
                byte* newBasePtr    = (byte*) newData.Scan0.ToPointer( );
                
                int sourceStride = sourceData.Stride;
                int newStride = newData.Stride;

                for ( int y = 0; y < height; y++ )
                {
                    byte* sourcePtr = (byte*) ( sourceBasePtr + y * sourceStride );
                    ushort* newPtr  = (ushort*) ( newBasePtr + y * newStride );

                    for ( int x = 0, lineSize = width * layers; x < lineSize; x++, sourcePtr++, newPtr++ )
                    {
                        *newPtr = (ushort) ( *sourcePtr << 8 );
                    }
                }
            }

            
            bimap.UnlockBits( sourceData );
            newImage.UnlockBits( newData );

            return newImage;
        }
    }
}
