namespace MotionDetector.Video.DirectShow.Internals
{
    using System;
    using System.Runtime.InteropServices;

    
    
    
    
	[ComImport,
	Guid("56A868B4-0AD4-11CE-B03A-0020AF0BA770"),
	InterfaceType(ComInterfaceType.InterfaceIsDual)]
    internal interface IVideoWindow
	{
        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_Caption( string caption );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_Caption( [Out] out string caption );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_WindowStyle( int windowStyle );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_WindowStyle( out int windowStyle );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_WindowStyleEx( int windowStyleEx );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_WindowStyleEx( out int windowStyleEx );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_AutoShow( [In, MarshalAs( UnmanagedType.Bool )] bool autoShow );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_AutoShow( [Out, MarshalAs( UnmanagedType.Bool )] out bool autoShow );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_WindowState( int windowState );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_WindowState( out int windowState );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_BackgroundPalette( [In, MarshalAs( UnmanagedType.Bool )] bool backgroundPalette );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_BackgroundPalette( [Out, MarshalAs( UnmanagedType.Bool )] out bool backgroundPalette );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_Visible( [In, MarshalAs( UnmanagedType.Bool )] bool visible );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_Visible( [Out, MarshalAs( UnmanagedType.Bool )] out bool visible );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_Left( int left );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_Left( out int left );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_Width( int width );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_Width( out int width );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_Top( int top );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_Top( out int top );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_Height( int height );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_Height( out int height );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_Owner( IntPtr owner );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_Owner( out IntPtr owner );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_MessageDrain( IntPtr drain );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_MessageDrain( out IntPtr drain );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_BorderColor( out int color );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_BorderColor( int color );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int get_FullScreenMode(
            [Out, MarshalAs( UnmanagedType.Bool )] out bool fullScreenMode );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int put_FullScreenMode( [In, MarshalAs( UnmanagedType.Bool )] bool fullScreenMode );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int SetWindowForeground( int focus );

        
        
        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int NotifyOwnerMessage( IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam );

        
        
        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int SetWindowPosition( int left, int top, int width, int height );

        
        
        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int GetWindowPosition( out int left, out int top, out int width, out int height );

        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int GetMinIdealImageSize( out int width, out int height );

        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int GetMaxIdealImageSize( out int width, out int height );

        
        
        
        
        
        
        
        
        
        
        
        [PreserveSig]
        int GetRestorePosition( out int left, out int top, out int width, out int height );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int HideCursor( [In, MarshalAs( UnmanagedType.Bool )] bool hideCursor );

        
        
        
        
        
        
        
        
        [PreserveSig]
        int IsCursorHidden( [Out, MarshalAs( UnmanagedType.Bool )] out bool hideCursor );
    }
}
