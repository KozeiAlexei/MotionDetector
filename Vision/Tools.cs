using System.Drawing.Imaging;

using MotionDetector.Imaging;
using MotionDetector.Imaging.Filters;

namespace MotionDetector.Vision
{
    internal static class Tools
    {
        public static void ConvertToGrayscale( UnmanagedImage source, UnmanagedImage destination )
        {
            if ( source.PixelFormat != PixelFormat.Format8bppIndexed )
                Grayscale.CommonAlgorithms.BT709.Apply( source, destination );
            else
                source.Copy( destination );
        }
    }
}
