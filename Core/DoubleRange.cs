







namespace MotionDetector
{
    using System;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    [Serializable]
    public struct DoubleRange
    {
        private double min, max;

        
        
        
        
        
        
        
        public double Min
        {
            get { return min; }
            set { min = value; }
        }

        
        
        
        
        
        
        
        public double Max
        {
            get { return max; }
            set { max = value; }
        }

        
        
        
        public double Length
        {
            get { return max - min; }
        }


        
        
        
        
        
        
        
        public DoubleRange( double min, double max )
        {
            this.min = min;
            this.max = max;
        }

        
        
        
        
        
        
        
        
        
        public bool IsInside( double x )
        {
            return ( ( x >= min ) && ( x <= max ) );
        }

        
        
        
        
        
        
        
        
        
        public bool IsInside( DoubleRange range )
        {
            return ( ( IsInside( range.min ) ) && ( IsInside( range.max ) ) );
        }

        
        
        
        
        
        
        
        
        
        public bool IsOverlapping( DoubleRange range )
        {
            return ( ( IsInside( range.min ) ) || ( IsInside( range.max ) ) ||
                     ( range.IsInside( min ) ) || ( range.IsInside( max ) ) );
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        public IntRange ToIntRange( bool provideInnerRange )
        {
            int iMin, iMax;

            if ( provideInnerRange )
            {
                iMin = (int) Math.Ceiling( min );
                iMax = (int) Math.Floor( max );
            }
            else
            {
                iMin = (int) Math.Floor( min );
                iMax = (int) Math.Ceiling( max );
            }

            return new IntRange( iMin, iMax );
        }

        
        
        
        
        
        
        
        
        
        
        public static bool operator ==( DoubleRange range1, DoubleRange range2 )
        {
            return ( ( range1.min == range2.min ) && ( range1.max == range2.max ) );
        }

        
        
        
        
        
        
        
        
        
        
        public static bool operator !=( DoubleRange range1, DoubleRange range2 )
        {
            return ( ( range1.min != range2.min ) || ( range1.max != range2.max ) );

        }

        
        
        
        
        
        
        
        
        public override bool Equals( object obj )
        {
            return ( obj is Range ) ? ( this == (DoubleRange) obj ) : false;
        }

        
        
        
        
        
        
        public override int GetHashCode( )
        {
            return min.GetHashCode( ) + max.GetHashCode( );
        }

        
        
        
        
        
        
        public override string ToString( )
        {
            return string.Format( System.Globalization.CultureInfo.InvariantCulture, "{0}, {1}", min, max );
        }
    }
}
