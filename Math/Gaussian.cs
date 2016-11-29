







namespace MotionDetector.Math
{
    using System;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public class Gaussian
    {
        
        private double sigma = 1.0;
        
        private double sqrSigma = 1.0;

        
        
        
        
        
        
        
        
        
        public double Sigma
        {
            get { return sigma; }
            set
            {
                sigma = Math.Max( 0.00000001, value );
                sqrSigma = sigma * sigma;
            }
        }

        
        
        
        
        public Gaussian( ) { }

        
        
        
        
        
        
        public Gaussian( double sigma )
        {
            Sigma = sigma;
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public double Function( double x )
        {
            return Math.Exp( x * x / ( -2 * sqrSigma ) ) / ( Math.Sqrt( 2 * Math.PI ) * sigma );
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public double Function2D( double x, double y )
        {
            return Math.Exp( ( x * x + y * y ) / ( -2 * sqrSigma ) ) / ( 2 * Math.PI * sqrSigma );
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public double[] Kernel( int size )
        {
            
            if ( ( ( size % 2 ) == 0 ) || ( size < 3 ) || ( size > 101 ) )
            {
                throw new ArgumentException( "Wrong kernal size." );
            }

            
            int r = size / 2;
            
            double[] kernel = new double[size];

            
            for ( int x = -r, i = 0; i < size; x++, i++ )
            {
                kernel[i] = Function( x );
            }

            return kernel;
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public double[,] Kernel2D( int size )
        {
            
            if ( ( ( size % 2 ) == 0 ) || ( size < 3 ) || ( size > 101 ) )
            {
                throw new ArgumentException( "Wrong kernal size." );
            }

            
            int r = size / 2;
            
            double[,] kernel = new double[size, size];

            
            for ( int y = -r, i = 0; i < size; y++, i++ )
            {
                for ( int x = -r, j = 0; j < size; x++, j++ )
                {
                    kernel[i, j] = Function2D( x, y );
                }
            }

            return kernel;
        }
    }
}
