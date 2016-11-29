







namespace MotionDetector.Imaging.Filters
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public sealed class GaussianBlur : Convolution
    {
        private double sigma = 1.4;
        private int    size = 5;

        
        
        
        
        
        
        
        
        
        
        public double Sigma
        {
            get { return sigma; }
            set
            {
                
                sigma = Math.Max( 0.5, Math.Min( 5.0, value ) );
                
                CreateFilter( );
            }
        }

        
        
        
        
        
        
        
        
        
        public int Size
        {
            get { return size; }
            set
            {
                size = Math.Max( 3, Math.Min( 21, value | 1 ) );
                CreateFilter( );
            }
        }

        
        
        
        
        public GaussianBlur( )
        {
            CreateFilter( );
            base.ProcessAlpha = true;
        }

        
        
        
        
        
        
        public GaussianBlur( double sigma )
        {
            Sigma = sigma;
            base.ProcessAlpha = true;
        }

        
        
        
        
        
        
        
        public GaussianBlur( double sigma, int size )
        {
            Sigma = sigma;
            Size = size;
            base.ProcessAlpha = true;
        }

        
        #region Private Members

        
        private void CreateFilter( )
        {
            
            MotionDetector.Math.Gaussian gaus = new MotionDetector.Math.Gaussian( sigma );
            
            double[,] kernel = gaus.Kernel2D( size );
            double min = kernel[0, 0];
            
            int[,] intKernel = new int[size, size];
            int divisor = 0;

            for ( int i = 0; i < size; i++ )
            {
                for ( int j = 0; j < size; j++ )
                {
                    double v = kernel[i, j] / min;

                    if ( v > ushort.MaxValue )
                    {
                        v = ushort.MaxValue;
                    }
                    intKernel[i, j] = (int) v;

                    
                    divisor += intKernel[i, j];
                }
            }

            
            this.Kernel = intKernel;
            this.Divisor = divisor;
        }
        #endregion
    }
}
