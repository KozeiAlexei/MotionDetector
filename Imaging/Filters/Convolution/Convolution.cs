using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MotionDetector.Imaging.Filters
{
    public class Convolution : BaseUsingCopyPartialFilter
	{
        
        private int[,] kernel;
        
        private int divisor = 1;
        
        private int threshold = 0;
        
        private int size;
        
        private bool dynamicDivisorForEdges = true;
        
        private bool processAlpha = false;

        
        private Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>( );

        
        
        
        public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
        {
            get { return formatTranslations; }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public int[,] Kernel
        {
            get { return kernel; }
            set
            {
                int s = value.GetLength( 0 );

                
                if ( ( s != value.GetLength( 1 ) ) || ( s < 3 ) || ( s > 99 ) || ( s % 2 == 0 ) )
                    throw new ArgumentException( "Invalid kernel size." );

                this.kernel = value;
                this.size = s;
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        public int Divisor
        {
            get { return divisor; }
            set
            {
                if ( value == 0 )
                    throw new ArgumentException( "Divisor can not be equal to zero." );
                divisor = value;
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        public int Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public bool DynamicDivisorForEdges
        {
            get { return dynamicDivisorForEdges; }
            set { dynamicDivisorForEdges = value; }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        public bool ProcessAlpha
        {
            get { return processAlpha; }
            set { processAlpha = value; }
        }

        
        
        
        protected Convolution( )
        {
            formatTranslations[PixelFormat.Format8bppIndexed]    = PixelFormat.Format8bppIndexed;
            formatTranslations[PixelFormat.Format16bppGrayScale] = PixelFormat.Format16bppGrayScale;
            formatTranslations[PixelFormat.Format24bppRgb]       = PixelFormat.Format24bppRgb;
            formatTranslations[PixelFormat.Format32bppRgb]       = PixelFormat.Format32bppRgb;
            formatTranslations[PixelFormat.Format32bppArgb]      = PixelFormat.Format32bppArgb;
            formatTranslations[PixelFormat.Format48bppRgb]       = PixelFormat.Format48bppRgb;
            formatTranslations[PixelFormat.Format64bppArgb]      = PixelFormat.Format64bppArgb;
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public Convolution( int[,] kernel ) : this( )
        {
            Kernel = kernel;

            divisor = 0;

            
            for ( int i = 0, n = kernel.GetLength( 0 ); i < n; i++ )
            {
                for ( int j = 0, k = kernel.GetLength( 1 ); j < k; j++ )
                {
                    divisor += kernel[i, j];
                }
            }
            if ( divisor == 0 )
                divisor = 1;
        }

        
        
        
        
        
        
        
        
        
        
        
        public Convolution( int[,] kernel, int divisor ) : this( )
        {
            Kernel = kernel;
            Divisor = divisor;
        }

        
        
        
        
        
        
        
        
        protected override unsafe void ProcessFilter( UnmanagedImage source, UnmanagedImage destination, Rectangle rect )
        {
            int pixelSize = System.Drawing.Image.GetPixelFormatSize( source.PixelFormat ) / 8;

            
            int startX  = rect.Left;
            int startY  = rect.Top;
            int stopX   = startX + rect.Width;
            int stopY   = startY + rect.Height;

            
            if ( ( pixelSize <= 4 ) && ( pixelSize != 2 ) )
            {
                int srcStride = source.Stride;
                int dstStride = destination.Stride;

                int srcOffset = srcStride - rect.Width * pixelSize;
                int dstOffset = dstStride - rect.Width * pixelSize;

                byte* src = (byte*) source.ImageData.ToPointer( );
                byte* dst = (byte*) destination.ImageData.ToPointer( );

                
                src += ( startY * srcStride + startX * pixelSize );
                dst += ( startY * dstStride + startX * pixelSize );

                
                if ( destination.PixelFormat == PixelFormat.Format8bppIndexed )
                {
                    
                    Process8bppImage( src, dst, srcStride, dstStride, srcOffset, dstOffset, startX, startY, stopX, stopY );
                }
                else
                {
                    
                    if ( ( pixelSize == 3 ) || ( !processAlpha ) )
                    {
                        Process24bppImage( src, dst, srcStride, dstStride, srcOffset, dstOffset, startX, startY, stopX, stopY, pixelSize );
                    }
                    else
                    {
                        Process32bppImage( src, dst, srcStride, dstStride, srcOffset, dstOffset, startX, startY, stopX, stopY );
                    }
                }
            }
            else
            {
                pixelSize /= 2;

                int dstStride = destination.Stride / 2;
                int srcStride = source.Stride / 2;

                
                ushort* baseSrc = (ushort*) source.ImageData.ToPointer( );
                ushort* baseDst = (ushort*) destination.ImageData.ToPointer( );

                
                baseSrc += ( startX * pixelSize );
                baseDst += ( startX * pixelSize );

                if ( source.PixelFormat == PixelFormat.Format16bppGrayScale )
                {
                    
                    Process16bppImage( baseSrc, baseDst, srcStride, dstStride, startX, startY, stopX, stopY );
                }
                else
                {
                    
                    if ( ( pixelSize == 3 ) || ( !processAlpha ) )
                    {
                        Process48bppImage( baseSrc, baseDst, srcStride, dstStride, startX, startY, stopX, stopY, pixelSize );
                    }
                    else
                    {
                        Process64bppImage( baseSrc, baseDst, srcStride, dstStride, startX, startY, stopX, stopY );
                    }
                }
            }
        }

        
        private unsafe void Process8bppImage( byte* src, byte* dst,
                                              int srcStride, int dstStride, int srcOffset, int dstOffset,
                                              int startX, int startY, int stopX, int stopY )
        {
            
            int i, j, t, k, ir, jr;
            
            int radius = size >> 1;
            
            long g, div;

            
            int kernelSize = size * size;
            
            int processedKernelSize;

            
            for ( int y = startY; y < stopY; y++ )
            {
                
                for ( int x = startX; x < stopX; x++, src++, dst++ )
                {
                    g = div = processedKernelSize = 0;

                    
                    for ( i = 0; i < size; i++ )
                    {
                        ir = i - radius;
                        t = y + ir;

                        
                        if ( t < startY )
                            continue;
                        
                        if ( t >= stopY )
                            break;

                        
                        for ( j = 0; j < size; j++ )
                        {
                            jr = j - radius;
                            t = x + jr;

                            
                            if ( t < startX )
                                continue;

                            if ( t < stopX )
                            {
                                k = kernel[i, j];

                                div += k;
                                g += k * src[ir * srcStride + jr];
                                processedKernelSize++;
                            }
                        }
                    }

                    
                    if ( processedKernelSize == kernelSize )
                    {
                        
                        div = divisor;
                    }
                    else
                    {
                        
                        if ( !dynamicDivisorForEdges )
                        {
                            
                            div = divisor;
                        }
                    }

                    
                    if ( div != 0 )
                    {
                        g /= div;
                    }
                    g += threshold;
                    *dst = (byte) ( ( g > 255 ) ? 255 : ( ( g < 0 ) ? 0 : g ) );
                }
                src += srcOffset;
                dst += dstOffset;
            }
        }

        
        private unsafe void Process24bppImage( byte* src, byte* dst,
                                               int srcStride, int dstStride, int srcOffset, int dstOffset,
                                               int startX, int startY, int stopX, int stopY, int pixelSize )
        {
            
            int i, j, t, k, ir, jr;
            
            int radius = size >> 1;
            
            long r, g, b, div;

            
            int kernelSize = size * size;
            
            int processedKernelSize;

            byte* p;

            
            for ( int y = startY; y < stopY; y++ )
            {
                
                for ( int x = startX; x < stopX; x++, src += pixelSize, dst += pixelSize )
                {
                    r = g = b = div = processedKernelSize = 0;

                    
                    for ( i = 0; i < size; i++ )
                    {
                        ir = i - radius;
                        t = y + ir;

                        
                        if ( t < startY )
                            continue;
                        
                        if ( t >= stopY )
                            break;

                        
                        for ( j = 0; j < size; j++ )
                        {
                            jr = j - radius;
                            t = x + jr;

                            
                            if ( t < startX )
                                continue;

                            if ( t < stopX )
                            {
                                k = kernel[i, j];
                                p = &src[ir * srcStride + jr * pixelSize];

                                div += k;

                                r += k * p[RGB.R];
                                g += k * p[RGB.G];
                                b += k * p[RGB.B];

                                processedKernelSize++;
                            }
                        }
                    }

                    
                    if ( processedKernelSize == kernelSize )
                    {
                        
                        div = divisor;
                    }
                    else
                    {
                        
                        if ( !dynamicDivisorForEdges )
                        {
                            
                            div = divisor;
                        }
                    }

                    
                    if ( div != 0 )
                    {
                        r /= div;
                        g /= div;
                        b /= div;
                    }
                    r += threshold;
                    g += threshold;
                    b += threshold;

                    dst[RGB.R] = (byte) ( ( r > 255 ) ? 255 : ( ( r < 0 ) ? 0 : r ) );
                    dst[RGB.G] = (byte) ( ( g > 255 ) ? 255 : ( ( g < 0 ) ? 0 : g ) );
                    dst[RGB.B] = (byte) ( ( b > 255 ) ? 255 : ( ( b < 0 ) ? 0 : b ) );

                    
                    if ( pixelSize == 4 )
                        dst[RGB.A] = src[RGB.A];
                }
                src += srcOffset;
                dst += dstOffset;
            }
        }

        
        private unsafe void Process32bppImage( byte* src, byte* dst,
                                               int srcStride, int dstStride, int srcOffset, int dstOffset,
                                               int startX, int startY, int stopX, int stopY )
        {
            
            int i, j, t, k, ir, jr;
            
            int radius = size >> 1;
            
            long r, g, b, a, div;

            
            int kernelSize = size * size;
            
            int processedKernelSize;

            byte* p;

            
            for ( int y = startY; y < stopY; y++ )
            {
                
                for ( int x = startX; x < stopX; x++, src += 4, dst += 4 )
                {
                    r = g = b = a = div = processedKernelSize = 0;

                    
                    for ( i = 0; i < size; i++ )
                    {
                        ir = i - radius;
                        t = y + ir;

                        
                        if ( t < startY )
                            continue;
                        
                        if ( t >= stopY )
                            break;

                        
                        for ( j = 0; j < size; j++ )
                        {
                            jr = j - radius;
                            t = x + jr;

                            
                            if ( t < startX )
                                continue;

                            if ( t < stopX )
                            {
                                k = kernel[i, j];
                                p = &src[ir * srcStride + jr * 4];

                                div += k;

                                r += k * p[RGB.R];
                                g += k * p[RGB.G];
                                b += k * p[RGB.B];
                                a += k * p[RGB.A];

                                processedKernelSize++;
                            }
                        }
                    }

                    
                    if ( processedKernelSize == kernelSize )
                    {
                        
                        div = divisor;
                    }
                    else
                    {
                        
                        if ( !dynamicDivisorForEdges )
                        {
                            
                            div = divisor;
                        }
                    }

                    
                    if ( div != 0 )
                    {
                        r /= div;
                        g /= div;
                        b /= div;
                        a /= div;
                    }
                    r += threshold;
                    g += threshold;
                    b += threshold;
                    a += threshold;

                    dst[RGB.R] = (byte) ( ( r > 255 ) ? 255 : ( ( r < 0 ) ? 0 : r ) );
                    dst[RGB.G] = (byte) ( ( g > 255 ) ? 255 : ( ( g < 0 ) ? 0 : g ) );
                    dst[RGB.B] = (byte) ( ( b > 255 ) ? 255 : ( ( b < 0 ) ? 0 : b ) );
                    dst[RGB.A] = (byte) ( ( a > 255 ) ? 255 : ( ( a < 0 ) ? 0 : a ) );
                }
                src += srcOffset;
                dst += dstOffset;
            }
        }

        
        private unsafe void Process16bppImage( ushort* baseSrc, ushort* baseDst, int srcStride, int dstStride,
                                               int startX, int startY, int stopX, int stopY )
        {
            
            int i, j, t, k, ir, jr;
            
            int radius = size >> 1;
            
            long g, div;

            
            int kernelSize = size * size;
            
            int processedKernelSize;

            
            for ( int y = startY; y < stopY; y++ )
            {
                ushort* src = baseSrc + y * srcStride;
                ushort* dst = baseDst + y * dstStride;

                
                for ( int x = startX; x < stopX; x++, src++, dst++ )
                {
                    g = div = processedKernelSize = 0;

                    
                    for ( i = 0; i < size; i++ )
                    {
                        ir = i - radius;
                        t = y + ir;

                        
                        if ( t < startY )
                            continue;
                        
                        if ( t >= stopY )
                            break;

                        
                        for ( j = 0; j < size; j++ )
                        {
                            jr = j - radius;
                            t = x + jr;

                            
                            if ( t < startX )
                                continue;

                            if ( t < stopX )
                            {
                                k = kernel[i, j];

                                div += k;
                                g += k * src[ir * srcStride + jr];
                                processedKernelSize++;
                            }
                        }
                    }

                    
                    if ( processedKernelSize == kernelSize )
                    {
                        
                        div = divisor;
                    }
                    else
                    {
                        
                        if ( !dynamicDivisorForEdges )
                        {
                            
                            div = divisor;
                        }
                    }

                    
                    if ( div != 0 )
                    {
                        g /= div;
                    }
                    g += threshold;
                    *dst = (ushort) ( ( g > 65535 ) ? 65535 : ( ( g < 0 ) ? 0 : g ) );
                }
            }
        }

        
        private unsafe void Process48bppImage( ushort* baseSrc, ushort* baseDst, int srcStride, int dstStride,
                                               int startX, int startY, int stopX, int stopY, int pixelSize )
        {
            
            int i, j, t, k, ir, jr;
            
            int radius = size >> 1;
            
            long r, g, b, div;

            
            int kernelSize = size * size;
            
            int processedKernelSize;

            ushort* p;

            
            for ( int y = startY; y < stopY; y++ )
            {
                ushort* src = baseSrc + y * srcStride;
                ushort* dst = baseDst + y * dstStride;

                
                for ( int x = startX; x < stopX; x++, src += pixelSize, dst += pixelSize )
                {
                    r = g = b = div = processedKernelSize = 0;

                    
                    for ( i = 0; i < size; i++ )
                    {
                        ir = i - radius;
                        t = y + ir;

                        
                        if ( t < startY )
                            continue;
                        
                        if ( t >= stopY )
                            break;

                        
                        for ( j = 0; j < size; j++ )
                        {
                            jr = j - radius;
                            t = x + jr;

                            
                            if ( t < startX )
                                continue;

                            if ( t < stopX )
                            {
                                k = kernel[i, j];
                                p = &src[ir * srcStride + jr * pixelSize];

                                div += k;

                                r += k * p[RGB.R];
                                g += k * p[RGB.G];
                                b += k * p[RGB.B];

                                processedKernelSize++;
                            }
                        }
                    }

                    
                    if ( processedKernelSize == kernelSize )
                    {
                        
                        div = divisor;
                    }
                    else
                    {
                        
                        if ( !dynamicDivisorForEdges )
                        {
                            
                            div = divisor;
                        }
                    }

                    
                    if ( div != 0 )
                    {
                        r /= div;
                        g /= div;
                        b /= div;
                    }
                    r += threshold;
                    g += threshold;
                    b += threshold;

                    dst[RGB.R] = (ushort) ( ( r > 65535 ) ? 65535 : ( ( r < 0 ) ? 0 : r ) );
                    dst[RGB.G] = (ushort) ( ( g > 65535 ) ? 65535 : ( ( g < 0 ) ? 0 : g ) );
                    dst[RGB.B] = (ushort) ( ( b > 65535 ) ? 65535 : ( ( b < 0 ) ? 0 : b ) );

                    
                    if ( pixelSize == 4 )
                        dst[RGB.A] = src[RGB.A];
                }
            }
        }

        
        private unsafe void Process64bppImage( ushort* baseSrc, ushort* baseDst, int srcStride, int dstStride,
                                               int startX, int startY, int stopX, int stopY )
        {
            
            int i, j, t, k, ir, jr;
            
            int radius = size >> 1;
            
            long r, g, b, a, div;

            
            int kernelSize = size * size;
            
            int processedKernelSize;

            ushort* p;

            
            for ( int y = startY; y < stopY; y++ )
            {
                ushort* src = baseSrc + y * srcStride;
                ushort* dst = baseDst + y * dstStride;

                
                for ( int x = startX; x < stopX; x++, src += 4, dst += 4 )
                {
                    r = g = b = a = div = processedKernelSize = 0;

                    
                    for ( i = 0; i < size; i++ )
                    {
                        ir = i - radius;
                        t = y + ir;

                        
                        if ( t < startY )
                            continue;
                        
                        if ( t >= stopY )
                            break;

                        
                        for ( j = 0; j < size; j++ )
                        {
                            jr = j - radius;
                            t = x + jr;

                            
                            if ( t < startX )
                                continue;

                            if ( t < stopX )
                            {
                                k = kernel[i, j];
                                p = &src[ir * srcStride + jr * 4];

                                div += k;

                                r += k * p[RGB.R];
                                g += k * p[RGB.G];
                                b += k * p[RGB.B];
                                a += k * p[RGB.A];

                                processedKernelSize++;
                            }
                        }
                    }

                    
                    if ( processedKernelSize == kernelSize )
                    {
                        
                        div = divisor;
                    }
                    else
                    {
                        
                        if ( !dynamicDivisorForEdges )
                        {
                            
                            div = divisor;
                        }
                    }

                    
                    if ( div != 0 )
                    {
                        r /= div;
                        g /= div;
                        b /= div;
                        a /= div;
                    }
                    r += threshold;
                    g += threshold;
                    b += threshold;
                    a += threshold;

                    dst[RGB.R] = (ushort) ( ( r > 65535 ) ? 65535 : ( ( r < 0 ) ? 0 : r ) );
                    dst[RGB.G] = (ushort) ( ( g > 65535 ) ? 65535 : ( ( g < 0 ) ? 0 : g ) );
                    dst[RGB.B] = (ushort) ( ( b > 65535 ) ? 65535 : ( ( b < 0 ) ? 0 : b ) );
                    dst[RGB.A] = (ushort) ( ( a > 65535 ) ? 65535 : ( ( a < 0 ) ? 0 : a ) );
                }
            }
        }
    }
}
