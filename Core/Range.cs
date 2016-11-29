







namespace MotionDetector
{
    using System;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    [Serializable]
    public struct Range
    {
        private float min, max;

        
        
        
        
        
        
        
        public float Min
        {
            get { return min; }
            set { min = value; }
        }

        
        
        
        
        
        
        
        public float Max
        {
            get { return max; }
            set { max = value; }
        }

        
        
        
        public float Length
        {
            get { return max - min; }
        }


        
        
        
        
        
        
        
        public Range( float min, float max )
        {
            this.min = min;
            this.max = max;
        }

        
        
        
        
        
        
        
        
        
        public bool IsInside( float x )
        {
            return ( ( x >= min ) && ( x <= max ) );
        }

        
        
        
        
        
        
        
        
        
        public bool IsInside( Range range )
        {
            return ( ( IsInside( range.min ) ) && ( IsInside( range.max ) ) );
        }

        
        
        
        
        
        
        
        
        
        public bool IsOverlapping( Range range )
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

        
        
        
        
        
        
        
        
        
        
        public static bool operator ==( Range range1, Range range2 )
        {
            return ( ( range1.min == range2.min ) && ( range1.max == range2.max ) );
        }

        
        
        
        
        
        
        
        
        
        
        public static bool operator !=( Range range1, Range range2 )
        {
            return ( ( range1.min != range2.min ) || ( range1.max != range2.max ) );

        }

        
        
        
        
        
        
        
        
        public override bool Equals( object obj )
        {
            return ( obj is Range ) ? ( this == (Range) obj ) : false;
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
