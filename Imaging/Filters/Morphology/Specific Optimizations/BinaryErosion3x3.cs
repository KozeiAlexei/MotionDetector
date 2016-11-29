







namespace MotionDetector.Imaging.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public class BinaryErosion3x3 : BaseUsingCopyPartialFilter
    {
        
        private Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>( );

        
        
        
        public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
        {
            get { return formatTranslations; }
        }

        
        
        
        
        public BinaryErosion3x3( )
        {
            
            formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format8bppIndexed;
        }

        
        
        
        
        
        
        
        
        
        
        protected override unsafe void ProcessFilter( UnmanagedImage sourceData, UnmanagedImage destinationData, Rectangle rect )
        {
            if ( ( rect.Width < 3 ) || ( rect.Height < 3 ) )
            {
                throw new InvalidImagePropertiesException( "Processing rectangle mast be at least 3x3 in size." );
            }

            
            int startX  = rect.Left + 1;
            int startY  = rect.Top + 1;
            int stopX   = rect.Right - 1;
            int stopY   = rect.Bottom - 1;

            int dstStride = destinationData.Stride;
            int srcStride = sourceData.Stride;

            int dstOffset = dstStride - rect.Width + 1;
            int srcOffset = srcStride - rect.Width + 1;

            
            byte* src = (byte*) sourceData.ImageData.ToPointer( );
            byte* dst = (byte*) destinationData.ImageData.ToPointer( );

            
            src += ( startX - 1 ) + ( startY - 1 ) * srcStride;
            dst += ( startX - 1 ) + ( startY - 1 ) * dstStride;

            
            for ( int x = startX - 1; x < stopX; x++, src++, dst++ )
            {
                *dst = 0;
            }
            *dst = 0;

            src += srcOffset;
            dst += dstOffset;

            
            for ( int y = startY; y < stopY; y++ )
            {
                
                *dst = 0;

                src++;
                dst++;

                
                for ( int x = startX; x < stopX; x++, src++, dst++ )
                {
                    *dst = (byte) ( *src & src[-1] & src[1] &
                        src[-srcStride] & src[-srcStride - 1] & src[-srcStride + 1] &
                        src[srcStride] & src[srcStride - 1] & src[srcStride + 1] );
                }

                
                *dst = 0;

                src += srcOffset;
                dst += dstOffset;
            }

            

            
            for ( int x = startX - 1; x < stopX; x++, src++, dst++ )
            {
                *dst = 0;
            }
            *dst = 0;
        }
    }
}