







namespace MotionDetector
{
    using System;

    
    
    
    
    
    
    
    
    
    public sealed class ThreadSafeRandom : Random
    {
        private object sync = new object( );

        
        
        
        
        
        
        public ThreadSafeRandom( )
            : base( )
        {
        }

        
        
        
        
        
        
        
        
        
        
        public ThreadSafeRandom( int seed )
            : base( seed )
        {
        }

        
        
        
        
        
        
        
        
        
        public override int Next( )
        {
            lock ( sync ) return base.Next( );
        }

        
        
        
        
        
        
        
        
        
        
        
        
        public override int Next( int maxValue )
        {
            lock ( sync ) return base.Next( maxValue );
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public override int Next( int minValue, int maxValue )
        {
            lock ( sync ) return base.Next( minValue, maxValue );
        }

        
        
        
        
        
        
        
        
        public override void NextBytes( byte[] buffer )
        {
            lock ( sync ) base.NextBytes( buffer );
        }

        
        
        
        
        
        
        
        
        public override double NextDouble( )
        {
            lock ( sync ) return base.NextDouble( );
        }
    }
}
