







namespace MotionDetector
{
    using System;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    [Serializable]
    public struct IntRange
    {
        private int min, max;

        
        
        
        
        
        
        
        public int Min
        {
            get { return min; }
            set { min = value; }
        }

        
        
        
        
        
        
        
        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        
        
        
        public int Length
        {
            get { return max - min; }
        }

        
        
        
        
        
        
        
        public IntRange( int min, int max )
        {
            this.min = min;
            this.max = max;
        }

        
        
        
        
        
        
        
        
        
        public bool IsInside( int x )
        {
            return ( ( x >= min ) && ( x <= max ) );
        }

        
        
        
        
        
        
        
        
        
        public bool IsInside( IntRange range )
        {
            return ( ( IsInside( range.min ) ) && ( IsInside( range.max ) ) );
        }

        
        
        
        
        
        
        
        
        
        public bool IsOverlapping( IntRange range )
        {
            return ( ( IsInside( range.min ) ) || ( IsInside( range.max ) ) ||
                     ( range.IsInside( min ) ) || ( range.IsInside( max ) ) );
        }

        
        
        
        
        
        
        
        
        
        public static implicit operator Range( IntRange range )
        {
            return new Range( range.Min, range.Max );
        }

        
        
        
        
        
        
        
        
        
        
        public static bool operator ==( IntRange range1, IntRange range2 )
        {
            return ( ( range1.min == range2.min ) && ( range1.max == range2.max ) );
        }

        
        
        
        
        
        
        
        
        
        
        public static bool operator !=( IntRange range1, IntRange range2 )
        {
            return ( ( range1.min != range2.min ) || ( range1.max != range2.max ) );

        }

        
        
        
        
        
        
        
        
        public override bool Equals( object obj )
        {
            return ( obj is IntRange ) ? ( this == (IntRange) obj ) : false;
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
