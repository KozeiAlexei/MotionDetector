namespace MotionDetector.Video.DirectShow
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using MotionDetector.Video.DirectShow.Internals;

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public class FilterInfoCollection : CollectionBase
    {
        
        
        
        
        
        
        
        
        
        public FilterInfoCollection( Guid category )
		{
			CollectFilters( category );
		}

        
        
        
        
        
        
        
        
        public FilterInfo this[int index]
        {
            get
            {
                return ( (FilterInfo) InnerList[index] );
            }
        }
        
        
		private void CollectFilters( Guid category )
		{
			object			comObj = null;
			ICreateDevEnum	enumDev = null;
			IEnumMoniker	enumMon = null;
			IMoniker[]		devMon = new IMoniker[1];
			int				hr;

            try
            {
                
                Type srvType = Type.GetTypeFromCLSID( Clsid.SystemDeviceEnum );
                if ( srvType == null )
                    throw new ApplicationException( "Failed creating device enumerator" );

                
                comObj = Activator.CreateInstance( srvType );
                enumDev = (ICreateDevEnum) comObj;

                
                hr = enumDev.CreateClassEnumerator( ref category, out enumMon, 0 );
                if ( hr != 0 )
                    throw new ApplicationException( "No devices of the category" );

                
                IntPtr n = IntPtr.Zero;
                while ( true )
                {
                    
                    hr = enumMon.Next( 1, devMon, n );
                    if ( ( hr != 0 ) || ( devMon[0] == null ) )
                        break;

                    
                    FilterInfo filter = new FilterInfo( devMon[0] );
                    InnerList.Add( filter );

                    
                    Marshal.ReleaseComObject( devMon[0] );
                    devMon[0] = null;
                }

                
                InnerList.Sort( );
            }
            catch
            {
            }
			finally
			{
				
				enumDev = null;
				if ( comObj != null )
				{
					Marshal.ReleaseComObject( comObj );
					comObj = null;
				}
				if ( enumMon != null )
				{
					Marshal.ReleaseComObject( enumMon );
					enumMon = null;
				}
				if ( devMon[0] != null )
				{
					Marshal.ReleaseComObject( devMon[0] );
					devMon[0] = null;
				}
			}
		}
    }
}
