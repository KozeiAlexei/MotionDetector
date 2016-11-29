







namespace MotionDetector.Imaging
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public static class Drawing
    {
        
        
        
        
        
        
        
        
        
        
        public static unsafe void FillRectangle( BitmapData imageData, Rectangle rectangle, Color color )
        {
            FillRectangle( new UnmanagedImage( imageData ), rectangle, color );
        }

        
        
        
        
        
        
        
        
        
        
        public static unsafe void FillRectangle( UnmanagedImage image, Rectangle rectangle, Color color )
        {
            CheckPixelFormat( image.PixelFormat );

            int pixelSize = System.Drawing.Image.GetPixelFormatSize( image.PixelFormat ) / 8;

            
            int imageWidth  = image.Width;
            int imageHeight = image.Height;
            int stride      = image.Stride;

            
            int rectX1 = rectangle.X;
            int rectY1 = rectangle.Y;
            int rectX2 = rectangle.X + rectangle.Width - 1;
            int rectY2 = rectangle.Y + rectangle.Height - 1;

            
            if ( ( rectX1 >= imageWidth ) || ( rectY1 >= imageHeight ) || ( rectX2 < 0 ) || ( rectY2 < 0 ) )
            {
                
                return;
            }

            int startX  = Math.Max( 0, rectX1 );
            int stopX   = Math.Min( imageWidth - 1, rectX2 );
            int startY  = Math.Max( 0, rectY1 );
            int stopY   = Math.Min( imageHeight - 1, rectY2 );

            
            byte* ptr = (byte*) image.ImageData.ToPointer( ) + startY * stride + startX * pixelSize;

            if ( image.PixelFormat == PixelFormat.Format8bppIndexed )
            {
                
                byte gray = (byte) ( 0.2125 * color.R + 0.7154 * color.G + 0.0721 * color.B );

                int fillWidth = stopX - startX + 1;

                for ( int y = startY; y <= stopY; y++ )
                {
                    SystemTools.SetUnmanagedMemory( ptr, gray, fillWidth );
                    ptr += stride;
                }
            }
            else if ( image.PixelFormat == PixelFormat.Format32bppArgb )
            {
                
                double fillAlpha    = color.A / 255.0;
                double fillNegAlpha = 1.0 - fillAlpha;

                double fillRedPart   = fillAlpha * color.R;
                double fillGreenPart = fillAlpha * color.G;
                double fillBluePart  = fillAlpha * color.B;

                int offset = stride - ( stopX - startX + 1 ) * 4;

                for ( int y = startY; y <= stopY; y++ )
                {
                    for ( int x = startX; x <= stopX; x++, ptr += 4 )
                    {
                        double srcAlphaPart = ( ptr[RGB.A] / 255.0 ) * fillNegAlpha;

                        ptr[RGB.R] = (byte) ( ( fillRedPart   + srcAlphaPart * ptr[RGB.R] ) );
                        ptr[RGB.G] = (byte) ( ( fillGreenPart + srcAlphaPart * ptr[RGB.G] ) );
                        ptr[RGB.B] = (byte) ( ( fillBluePart  + srcAlphaPart * ptr[RGB.B] ) );

                        ptr[RGB.A] = (byte) ( 255 * ( fillAlpha + srcAlphaPart ) );
                    }
                    ptr += offset;
                }
            }
            else
            {
                
                byte red    = color.R;
                byte green  = color.G;
                byte blue   = color.B;

                int offset = stride - ( stopX - startX + 1 ) * pixelSize;

                if ( color.A == 255 )
                {
                    
                    for ( int y = startY; y <= stopY; y++ )
                    {
                        for ( int x = startX; x <= stopX; x++, ptr += pixelSize )
                        {
                            ptr[RGB.R] = red;
                            ptr[RGB.G] = green;
                            ptr[RGB.B] = blue;
                        }
                        ptr += offset;
                    }
                }
                else
                {
                    
                    int fillAlpha = color.A;
                    int fillNegAlpha = 255 - fillAlpha;

                    int fillRedPart   = fillAlpha * color.R;
                    int fillGreenPart = fillAlpha * color.G;
                    int fillBluePart  = fillAlpha * color.B;

                    for ( int y = startY; y <= stopY; y++ )
                    {
                        for ( int x = startX; x <= stopX; x++, ptr += pixelSize )
                        {
                            ptr[RGB.R] = (byte) ( ( fillRedPart   + fillNegAlpha * ptr[RGB.R] ) / 255 );
                            ptr[RGB.G] = (byte) ( ( fillGreenPart + fillNegAlpha * ptr[RGB.G] ) / 255 );
                            ptr[RGB.B] = (byte) ( ( fillBluePart  + fillNegAlpha * ptr[RGB.B] ) / 255 );
                        }
                        ptr += offset;
                    }
                }
            }
        }

        
        
        
        
        
        
        
        
        
        
        public static unsafe void Rectangle( BitmapData imageData, Rectangle rectangle, Color color )
        {
            Rectangle( new UnmanagedImage( imageData ), rectangle, color );
        }

        
        
        
        
        
        
        
        
        
        
        public static unsafe void Rectangle( UnmanagedImage image, Rectangle rectangle, Color color )
        {
            CheckPixelFormat( image.PixelFormat );

            int pixelSize = System.Drawing.Image.GetPixelFormatSize( image.PixelFormat ) / 8;

            
            int imageWidth  = image.Width;
            int imageHeight = image.Height;
            int stride      = image.Stride;

            
            int rectX1 = rectangle.X;
            int rectY1 = rectangle.Y;
            int rectX2 = rectangle.X + rectangle.Width - 1;
            int rectY2 = rectangle.Y + rectangle.Height - 1;

            
            if ( ( rectX1 >= imageWidth ) || ( rectY1 >= imageHeight ) || ( rectX2 < 0 ) || ( rectY2 < 0 ) )
            {
                
                return;
            }

            
            Line( image, new IntPoint( rectX1, rectY1 ), new IntPoint( rectX2, rectY1 ), color );
            Line( image, new IntPoint( rectX2, rectY2 ), new IntPoint( rectX1, rectY2 ), color );

            Line( image, new IntPoint( rectX2, rectY1 + 1 ), new IntPoint( rectX2, rectY2 - 1 ), color );
            Line( image, new IntPoint( rectX1, rectY2 - 1 ), new IntPoint( rectX1, rectY1 + 1 ), color );
        }

        
        
        
        
        
        
        
        
        
        
        
        public static unsafe void Line( BitmapData imageData, IntPoint point1, IntPoint point2, Color color )
        {
            Line( new UnmanagedImage( imageData ), point1, point2, color );
        }

        
        
        
        
        
        
        
        
        
        
        
        public static unsafe void Line( UnmanagedImage image, IntPoint point1, IntPoint point2, Color color )
        {
            

            CheckPixelFormat( image.PixelFormat );

            int pixelSize = System.Drawing.Image.GetPixelFormatSize( image.PixelFormat ) / 8;

            
            int imageWidth  = image.Width;
            int imageHeight = image.Height;
            int stride      = image.Stride;

            
            if (
                ( ( point1.X < 0 ) && ( point2.X < 0 ) ) ||
                ( ( point1.Y < 0 ) && ( point2.Y < 0 ) ) ||
                ( ( point1.X >= imageWidth ) && ( point2.X >= imageWidth ) ) ||
                ( ( point1.Y >= imageHeight ) && ( point2.Y >= imageHeight ) ) )
            {
                
                return;
            }

            CheckEndPoint( imageWidth, imageHeight, point1, ref point2 );
            CheckEndPoint( imageWidth, imageHeight, point2, ref point1 );

            
            if (
                ( ( point1.X < 0 ) && ( point2.X < 0 ) ) ||
                ( ( point1.Y < 0 ) && ( point2.Y < 0 ) ) ||
                ( ( point1.X >= imageWidth ) && ( point2.X >= imageWidth ) ) ||
                ( ( point1.Y >= imageHeight ) && ( point2.Y >= imageHeight ) ) )
            {
                
                return;
            }

            int startX = point1.X;
            int startY = point1.Y;
            int stopX  = point2.X;
            int stopY  = point2.Y;

            
            byte gray = 0;
            if ( image.PixelFormat == PixelFormat.Format8bppIndexed )
            {
                gray = (byte) ( 0.2125 * color.R + 0.7154 * color.G + 0.0721 * color.B );
            }

            
            double fillAlpha    = color.A / 255.0;
            double fillNegAlpha = 1.0 - fillAlpha;

            double fillRedPart   = fillAlpha * color.R;
            double fillGreenPart = fillAlpha * color.G;
            double fillBluePart  = fillAlpha * color.B;

            int fillNegAlphaInt = 255 - color.A;

            int fillRedPartInt   = color.A * color.R;
            int fillGreenPartInt = color.A * color.G;
            int fillBluePartInt  = color.A * color.B;

            
            int dx = stopX - startX;
            int dy = stopY - startY;

            if ( Math.Abs( dx ) >= Math.Abs( dy ) )
            {
                
                float slope = ( dx != 0 ) ? (float) dy / dx : 0;
                int step = ( dx > 0 ) ? 1 : -1;

                
                dx += step;

                if ( image.PixelFormat == PixelFormat.Format8bppIndexed )
                {
                    
                    for ( int x = 0; x != dx; x += step )
                    {
                        int px = startX + x;
                        int py = (int) ( (float) startY + ( slope * (float) x ) );

                        byte* ptr = (byte*) image.ImageData.ToPointer( ) + py * stride + px;
                        *ptr = gray;
                    }
                }
                else if ( image.PixelFormat == PixelFormat.Format32bppArgb )
                {
                    
                    for ( int x = 0; x != dx; x += step )
                    {
                        int px = startX + x;
                        int py = (int) ( (float) startY + ( slope * (float) x ) );

                        byte* ptr = (byte*) image.ImageData.ToPointer( ) + py * stride + px * 4;

                        double srcAlphaPart = ( ptr[RGB.A] / 255.0 ) * fillNegAlpha;

                        ptr[RGB.R] = (byte) ( ( fillRedPart   + srcAlphaPart * ptr[RGB.R] ) );
                        ptr[RGB.G] = (byte) ( ( fillGreenPart + srcAlphaPart * ptr[RGB.G] ) );
                        ptr[RGB.B] = (byte) ( ( fillBluePart  + srcAlphaPart * ptr[RGB.B] ) );

                        ptr[RGB.A] = (byte) ( 255 * ( fillAlpha + srcAlphaPart ) );
                    }
                }
                else
                {
                    
                    if ( color.A == 255 )
                    {
                        
                        for ( int x = 0; x != dx; x += step )
                        {
                            int px = startX + x;
                            int py = (int) ( (float) startY + ( slope * (float) x ) );

                            byte* ptr = (byte*) image.ImageData.ToPointer( ) + py * stride + px * pixelSize;

                            ptr[RGB.R] = color.R;
                            ptr[RGB.G] = color.G;
                            ptr[RGB.B] = color.B;
                        }
                    }
                    else
                    {
                        
                        for ( int x = 0; x != dx; x += step )
                        {
                            int px = startX + x;
                            int py = (int) ( (float) startY + ( slope * (float) x ) );

                            byte* ptr = (byte*) image.ImageData.ToPointer( ) + py * stride + px * pixelSize;

                            ptr[RGB.R] = (byte) ( ( fillRedPartInt   + fillNegAlphaInt * ptr[RGB.R] ) / 255 );
                            ptr[RGB.G] = (byte) ( ( fillGreenPartInt + fillNegAlphaInt * ptr[RGB.G] ) / 255 );
                            ptr[RGB.B] = (byte) ( ( fillBluePartInt  + fillNegAlphaInt * ptr[RGB.B] ) / 255 );
                        }
                    }
                }
            }
            else
            {
                
                float slope = ( dy != 0 ) ? (float) dx / dy : 0;
                int step = ( dy > 0 ) ? 1 : -1;

                
                dy += step;

                if ( image.PixelFormat == PixelFormat.Format8bppIndexed )
                {
                    
                    for ( int y = 0; y != dy; y += step )
                    {
                        int px = (int) ( (float) startX + ( slope * (float) y ) );
                        int py = startY + y;

                        byte* ptr = (byte*) image.ImageData.ToPointer( ) + py * stride + px;
                        *ptr = gray;
                    }
                }
                else if ( image.PixelFormat == PixelFormat.Format32bppArgb )
                {
                    
                    for ( int y = 0; y != dy; y += step )
                    {
                        int px = (int) ( (float) startX + ( slope * (float) y ) );
                        int py = startY + y;

                        byte* ptr = (byte*) image.ImageData.ToPointer( ) + py * stride + px * 4;

                        double srcAlphaPart = ( ptr[RGB.A] / 255.0 ) * fillNegAlpha;

                        ptr[RGB.R] = (byte) ( ( fillRedPart   + srcAlphaPart * ptr[RGB.R] ) );
                        ptr[RGB.G] = (byte) ( ( fillGreenPart + srcAlphaPart * ptr[RGB.G] ) );
                        ptr[RGB.B] = (byte) ( ( fillBluePart  + srcAlphaPart * ptr[RGB.B] ) );

                        ptr[RGB.A] = (byte) ( 255 * ( fillAlpha + srcAlphaPart ) );
                    }
                }
                else
                {
                    
                    if ( color.A == 255 )
                    {
                        
                        for ( int y = 0; y != dy; y += step )
                        {
                            int px = (int) ( (float) startX + ( slope * (float) y ) );
                            int py = startY + y;

                            byte* ptr = (byte*) image.ImageData.ToPointer( ) + py * stride + px * pixelSize;

                            ptr[RGB.R] = color.R;
                            ptr[RGB.G] = color.G;
                            ptr[RGB.B] = color.B;
                        }
                    }
                    else
                    {
                        
                        for ( int y = 0; y != dy; y += step )
                        {
                            int px = (int) ( (float) startX + ( slope * (float) y ) );
                            int py = startY + y;

                            byte* ptr = (byte*) image.ImageData.ToPointer( ) + py * stride + px * pixelSize;

                            ptr[RGB.R] = (byte) ( ( fillRedPartInt   + fillNegAlphaInt * ptr[RGB.R] ) / 255 );
                            ptr[RGB.G] = (byte) ( ( fillGreenPartInt + fillNegAlphaInt * ptr[RGB.G] ) / 255 );
                            ptr[RGB.B] = (byte) ( ( fillBluePartInt  + fillNegAlphaInt * ptr[RGB.B] ) / 255 );
                        }
                    }
                }
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        public static void Polygon( BitmapData imageData, List<IntPoint> points, Color color )
        {
            Polygon( new UnmanagedImage( imageData ), points, color );
        }

        
        
        
        
        
        
        
        
        
        
        
        
        public static void Polygon( UnmanagedImage image, List<IntPoint> points, Color color )
        {
            for ( int i = 1, n = points.Count; i < n; i++ )
            {
                Line( image, points[i - 1], points[i], color );
            }
            Line( image, points[points.Count - 1], points[0], color );
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        public static void Polyline( BitmapData imageData, List<IntPoint> points, Color color )
        {
            Polyline( new UnmanagedImage( imageData ), points, color );
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        public static void Polyline( UnmanagedImage image, List<IntPoint> points, Color color )
        {
            for ( int i = 1, n = points.Count; i < n; i++ )
            {
                Line( image, points[i - 1], points[i], color );
            }
        }

        
        private static void CheckPixelFormat( PixelFormat format )
        {
            
            if (
                ( format != PixelFormat.Format24bppRgb ) &&
                ( format != PixelFormat.Format8bppIndexed ) &&
                ( format != PixelFormat.Format32bppArgb ) &&
                ( format != PixelFormat.Format32bppRgb )
                )
            {
                throw new UnsupportedImageFormatException( "Unsupported pixel format of the source image." );
            }
        }

        
        private static void CheckEndPoint( int width, int height, IntPoint start, ref IntPoint end )
        {
            if ( end.X >= width )
            {
                int newEndX = width - 1;

                double c = (double) ( newEndX - start.X ) / ( end.X - start.X );

                end.Y = (int) ( start.Y + c * ( end.Y - start.Y ) );
                end.X = newEndX;
            }

            if ( end.Y >= height )
            {
                int newEndY = height - 1;

                double c = (double) ( newEndY - start.Y ) / ( end.Y - start.Y );

                end.X = (int) ( start.X + c * ( end.X - start.X ) );
                end.Y = newEndY;
            }

            if ( end.X < 0 )
            {
                double c = (double) ( 0 - start.X ) / ( end.X - start.X );

                end.Y = (int) ( start.Y + c * ( end.Y - start.Y ) );
                end.X = 0;
            }

            if ( end.Y < 0 )
            {
                double c = (double) ( 0 - start.Y ) / ( end.Y - start.Y );

                end.X = (int) ( start.X + c * ( end.X - start.X ) );
                end.Y = 0;
            }
        }
    }
}
