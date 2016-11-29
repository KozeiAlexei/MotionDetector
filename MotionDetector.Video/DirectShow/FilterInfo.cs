namespace MotionDetector.Video.DirectShow
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using MotionDetector.Video.DirectShow.Internals;

    
    
    
    
    public class FilterInfo : IComparable
    {
        
        
        
        public string Name { get; private set; }

        
        
        
        
        public string MonikerString { get; private set; }

        
        
        
        
        
        
        public FilterInfo( string monikerString )
        {
            MonikerString = monikerString;
            Name = GetName( monikerString );
        }

        
        
        
        
        
        
        internal FilterInfo( IMoniker moniker )
        {
            MonikerString = GetMonikerString( moniker );
            Name = GetName( moniker );
        }

        
        
        
        
        
        
        
        
        public int CompareTo( object value )
        {
            FilterInfo f = (FilterInfo) value;

            if ( f == null )
                return 1;

            return ( this.Name.CompareTo( f.Name ) );
        }

        
        
        
        
        
        
        
        
        
        
        public static object CreateFilter( string filterMoniker )
        {
            
            object filterObject = null;
            
            IBindCtx bindCtx = null;
            IMoniker moniker = null;

            int n = 0;

            
            if ( Win32.CreateBindCtx( 0, out bindCtx ) == 0 )
            {
                
                if ( Win32.MkParseDisplayName( bindCtx, filterMoniker, ref n, out moniker ) == 0 )
                {
                    
                    Guid filterId = typeof( IBaseFilter ).GUID;
                    moniker.BindToObject( null, null, ref filterId, out filterObject );

                    Marshal.ReleaseComObject( moniker );
                }
                Marshal.ReleaseComObject( bindCtx );
            }
            return filterObject;
        }

        
        
        
        private string GetMonikerString( IMoniker moniker )
        {
            string str;
            moniker.GetDisplayName( null, null, out str );
            return str;
        }

        
        
        
        private string GetName( IMoniker moniker )
        {
            Object bagObj = null;
            IPropertyBag bag = null;

            try
            {
                Guid bagId = typeof( IPropertyBag ).GUID;
                
                moniker.BindToStorage( null, null, ref bagId, out bagObj );
                bag = (IPropertyBag) bagObj;

                
                object val = "";
                int hr = bag.Read( "FriendlyName", ref val, IntPtr.Zero );
                if ( hr != 0 )
                    Marshal.ThrowExceptionForHR( hr );

                
                string ret = (string) val;
                if ( ( ret == null ) || ( ret.Length < 1 ) )
                    throw new ApplicationException( );

                return ret;
            }
            catch ( Exception )
            {
                return "";
            }
            finally
            {
                
                bag = null;
                if ( bagObj != null )
                {
                    Marshal.ReleaseComObject( bagObj );
                    bagObj = null;
                }
            }
        }

        
        
        
        private string GetName( string monikerString )
        {
            IBindCtx bindCtx = null;
            IMoniker moniker = null;
            String name = "";
            int n = 0;

            
            if ( Win32.CreateBindCtx( 0, out bindCtx ) == 0 )
            {
                
                if ( Win32.MkParseDisplayName( bindCtx, monikerString, ref n, out moniker ) == 0 )
                {
                    
                    name = GetName( moniker );

                    Marshal.ReleaseComObject( moniker );
                    moniker = null;
                }
                Marshal.ReleaseComObject( bindCtx );
                bindCtx = null;
            }
            return name;
        }
    }
}
