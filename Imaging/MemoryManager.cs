







namespace MotionDetector.Imaging
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    
    
    
    
    
    
    
    
    public static class MemoryManager
    {
        
        private static int maximumCacheSize = 3;
        
        private static int currentCacheSize = 0;
        
        private static int busyBlocks = 0;
        
        private static int cachedMemory = 0;

        
        private static int maxSizeToCache = 20 * 1024 * 1024;
        
        private static int minSizeToCache = 10 * 1024;

        
        private class CacheBlock
        {
            public IntPtr   MemoryBlock;
            public int      Size;
            public bool     Free;

            public CacheBlock( IntPtr memoryBlock, int size )
            {
                this.MemoryBlock = memoryBlock;
                this.Size = size;
                this.Free = false;
            }
        }

        private static List<CacheBlock> memoryBlocks = new List<CacheBlock>( );

        
        
        
        
        
        
        
        
        
        
        public static int MaximumCacheSize
        {
            get
            {
                lock ( memoryBlocks )
                {
                    return maximumCacheSize;
                }
            }
            set
            {
                lock ( memoryBlocks )
                {
                    maximumCacheSize = Math.Max( 0, Math.Min( 10, value ) );
                }
            }
        }

        
        
        
        
        public static int CurrentCacheSize
        {
            get
            {
                lock ( memoryBlocks )
                {
                    return currentCacheSize;
                }
            }
        }

        
        
        
        
        public static int BusyMemoryBlocks
        {
            get
            {
                lock ( memoryBlocks )
                {
                    return busyBlocks;
                }
            }
        }

        
        
        
        
        public static int FreeMemoryBlocks
        {
            get
            {
                lock ( memoryBlocks )
                {
                    return currentCacheSize - busyBlocks;
                }
            }
        }

        
        
        
        
        public static int CachedMemory
        {
            get
            {
                lock ( memoryBlocks )
                {
                    return cachedMemory;
                }
            }
        }

        
        
        
        
        
        
        public static int MaxSizeToCache
        {
            get { return maxSizeToCache; }
            set { maxSizeToCache = value; }
        }

        
        
        
        
        
        
        public static int MinSizeToCache
        {
            get { return minSizeToCache; }
            set { minSizeToCache = value; }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        public static IntPtr Alloc( int size )
        {
            lock ( memoryBlocks )
            {
                
                if ( ( busyBlocks >= maximumCacheSize ) || ( size > maxSizeToCache ) || ( size < minSizeToCache ) )
                    return Marshal.AllocHGlobal( size );

                
                if ( currentCacheSize == busyBlocks )
                {
                    IntPtr memoryBlock = Marshal.AllocHGlobal( size );
                    memoryBlocks.Add( new CacheBlock( memoryBlock, size ) );

                    busyBlocks++;
                    currentCacheSize++;
                    cachedMemory += size;

                    return memoryBlock;
                }

                
                for ( int i = 0; i < currentCacheSize; i++ )
                {
                    CacheBlock block = memoryBlocks[i];

                    if ( ( block.Free == true ) && ( block.Size >= size ) )
                    {
                        block.Free = false;
                        busyBlocks++;
                        return block.MemoryBlock;
                    }
                }

                
                for ( int i = 0; i < currentCacheSize; i++ )
                {
                    CacheBlock block = memoryBlocks[i];

                    if ( block.Free == true )
                    {
                        
                        Marshal.FreeHGlobal( block.MemoryBlock );
                        memoryBlocks.RemoveAt( i );
                        currentCacheSize--;
                        cachedMemory -= block.Size;

                        
                        IntPtr memoryBlock = Marshal.AllocHGlobal( size );
                        memoryBlocks.Add( new CacheBlock( memoryBlock, size ) );

                        busyBlocks++;
                        currentCacheSize++;
                        cachedMemory += size;

                        return memoryBlock;
                    }
                }

                return IntPtr.Zero;
            }
        }

        
        
        
        
        
        
        
        
        
        public static void Free( IntPtr pointer )
        {
            lock ( memoryBlocks )
            {
                
                for ( int i = 0; i < currentCacheSize; i++ )
                {
                    if ( memoryBlocks[i].MemoryBlock == pointer )
                    {
                        
                        memoryBlocks[i].Free = true;
                        busyBlocks--;
                        return;
                    }
                }

                
                Marshal.FreeHGlobal( pointer );
            }
        }

        
        
        
        
        
        
        
        
        public static int FreeUnusedMemory( )
        {
            lock ( memoryBlocks )
            {
                int freedBlocks = 0;

                
                for ( int i = currentCacheSize - 1; i >= 0; i-- )
                {
                    if ( memoryBlocks[i].Free )
                    {
                        Marshal.FreeHGlobal( memoryBlocks[i].MemoryBlock );
                        cachedMemory -= memoryBlocks[i].Size;
                        memoryBlocks.RemoveAt( i );
                        freedBlocks++;
                    }
                }
                currentCacheSize -= freedBlocks;

                return freedBlocks;
            }
        }
    }
}
