







namespace MotionDetector
{
    using System;
    using System.ComponentModel;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    [Serializable]
    public struct IntPoint
    {
        
        
        
        
        public int X;

        
        
        
        
        public int Y;

        
        
        
        
        
        
        
        public IntPoint( int x, int y )
        {
            this.X = x;
            this.Y = y;
        }

        
        
        
        
        
        
        
        
        
        public float DistanceTo( IntPoint anotherPoint )
        {
            int dx = X - anotherPoint.X;
            int dy = Y - anotherPoint.Y;

            return (float) System.Math.Sqrt( dx * dx + dy * dy );
        }

        
        
        
        
        
        
        
        
        
        public float SquaredDistanceTo( Point anotherPoint )
        {
            float dx = X - anotherPoint.X;
            float dy = Y - anotherPoint.Y;

            return dx * dx + dy * dy;
        }

        
        
        
        
        
        
        
        
        
        
        public static IntPoint operator +( IntPoint point1, IntPoint point2 )
        {
            return new IntPoint( point1.X + point2.X, point1.Y + point2.Y );
        }

        
        
        
        
        
        
        
        
        
        
        public static IntPoint Add( IntPoint point1, IntPoint point2 )
        {
            return new IntPoint( point1.X + point2.X, point1.Y + point2.Y );
        }

        
        
        
        
        
        
        
        
        
        
        public static IntPoint operator -( IntPoint point1, IntPoint point2 )
        {
            return new IntPoint( point1.X - point2.X, point1.Y - point2.Y );
        }

        
        
        
        
        
        
        
        
        
        
        public static IntPoint Subtract( IntPoint point1, IntPoint point2 )
        {
            return new IntPoint( point1.X - point2.X, point1.Y - point2.Y );
        }

        
        
        
        
        
        
        
        
        
        
        public static IntPoint operator +( IntPoint point, int valueToAdd )
        {
            return new IntPoint( point.X + valueToAdd, point.Y + valueToAdd );
        }

        
        
        
        
        
        
        
        
        
        
        public static IntPoint Add( IntPoint point, int valueToAdd )
        {
            return new IntPoint( point.X + valueToAdd, point.Y + valueToAdd );
        }

        
        
        
        
        
        
        
        
        
        
        public static IntPoint operator -( IntPoint point, int valueToSubtract )
        {
            return new IntPoint( point.X - valueToSubtract, point.Y - valueToSubtract );
        }

        
        
        
        
        
        
        
        
        
        
        public static IntPoint Subtract( IntPoint point, int valueToSubtract )
        {
            return new IntPoint( point.X - valueToSubtract, point.Y - valueToSubtract );
        }

        
        
        
        
        
        
        
        
        
        
        public static IntPoint operator *( IntPoint point, int factor )
        {
            return new IntPoint( point.X * factor, point.Y * factor );
        }

        
        
        
        
        
        
        
        
        
        
        public static IntPoint Multiply( IntPoint point, int factor )
        {
            return new IntPoint( point.X * factor, point.Y * factor );
        }

        
        
        
        
        
        
        
        
        
        
        public static IntPoint operator /( IntPoint point, int factor )
        {
            return new IntPoint( point.X / factor, point.Y / factor );
        }

        
        
        
        
        
        
        
        
        
        
        public static IntPoint Divide( IntPoint point, int factor )
        {
            return new IntPoint( point.X / factor, point.Y / factor );
        }

        
        
        
        
        
        
        
        
        
        
        public static bool operator ==( IntPoint point1, IntPoint point2 )
        {
            return ( ( point1.X == point2.X ) && ( point1.Y == point2.Y ) );
        }

        
        
        
        
        
        
        
        
        
        
        public static bool operator !=( IntPoint point1, IntPoint point2 )
        {
            return ( ( point1.X != point2.X ) || ( point1.Y != point2.Y ) );
        }

        
        
        
        
        
        
        
        
        public override bool Equals( object obj )
        {
            return ( obj is IntPoint ) ? ( this == (IntPoint) obj ) : false;
        }

        
        
        
        
        
        
        public override int GetHashCode( )
        {
            return X.GetHashCode( ) + Y.GetHashCode( );
        }

        
        
        
        
        
        
        
        
        
        public static implicit operator Point( IntPoint point )
        {
            return new Point( point.X, point.Y );
        }

        
        
        
        
        
        
        
        
        
        public static implicit operator DoublePoint( IntPoint point )
        {
            return new DoublePoint( point.X, point.Y );
        } 

        
        
        
        
        
        
        public override string ToString( )
        {
            return string.Format( System.Globalization.CultureInfo.InvariantCulture, "{0}, {1}", X, Y );
        }

        
        
        
        
        
        
        
        public float EuclideanNorm( )
        {
            return (float) System.Math.Sqrt( X * X + Y * Y );
        }
    }    
}
