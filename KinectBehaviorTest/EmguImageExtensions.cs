using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using media = System.Windows.Media;
using Microsoft.Kinect;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.IO;


namespace ImageManipulationExtensionMethods
{
    public static class ImageExtensions
    {
        public static Bitmap ToBitmap(this byte[] data, int width, int height, System.Drawing.Imaging.PixelFormat format)
        {
            var bitmap = new Bitmap(width, height, format);
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.WriteOnly,
                bitmap.PixelFormat);
            Marshal.Copy(data, 0, bitmapData.Scan0, data.Length);
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }

        public static Bitmap ToBitmap(this short[] data, int width, int height, System.Drawing.Imaging.PixelFormat format)
        {
            var bitmap = new Bitmap(width, height, format);
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.WriteOnly,
                bitmap.PixelFormat);
            Marshal.Copy(data, 0, bitmapData.Scan0, data.Length);
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }

        public static media.Imaging.BitmapSource ToBitmapSource(this byte[] data, media.PixelFormat format, int width, int height)
        {
            return media.Imaging.BitmapSource.Create(width, height, 96, 96, format, null, data, width * format.BitsPerPixel / 8);
        }
        public static media.Imaging.BitmapSource ToBitmapSource(this short[] data, media.PixelFormat format, int width, int height)
        {
            return media.Imaging.BitmapSource.Create(width, height, 96, 96, format, null, data, width * format.BitsPerPixel / 8);
        }

        public static Bitmap ToBitmap(this ColorImageFrame image, System.Drawing.Imaging.PixelFormat format)
        {
            if (image == null || image.PixelDataLength == 0)
                return null;
            var data = new byte[image.PixelDataLength];
            image.CopyPixelDataTo(data);
            return data.ToBitmap(image.Width, image.Height, format);
        }

        public static Bitmap ToBitmap(this DepthImageFrame image, System.Drawing.Imaging.PixelFormat format)
        {
            if (image == null || image.PixelDataLength == 0)
                return null;
            var data = new short[image.PixelDataLength];
            image.CopyPixelDataTo(data);
            return data.ToBitmap(image.Width, image.Height, format);
        }

        public static Bitmap ToBitmap(this ColorImageFrame image)
        {
            return image.ToBitmap(System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        }

        public static Bitmap ToBitmap(this DepthImageFrame image)
        {
            return image.ToBitmap(System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
        }

        //bitmap source
        public static media.Imaging.BitmapSource ToBitmapSource(this ColorImageFrame image)
        {
            if (image == null || image.PixelDataLength == 0)
                return null;
            var data = new byte[image.PixelDataLength];
            image.CopyPixelDataTo(data);
            return data.ToBitmapSource(media.PixelFormats.Bgr32, image.Width, image.Height);
        }

        public static media.Imaging.BitmapSource ToBitmapSource(this DepthImageFrame image)
        {
            if (image == null || image.PixelDataLength == 0)
                return null;
            var data = new short[image.PixelDataLength];
            image.CopyPixelDataTo(data);
            return data.ToBitmapSource(media.PixelFormats.Bgr555, image.Width, image.Height);
        }

        public static media.Imaging.BitmapSource ToTransparentBitmapSource(this byte[] data, int width, int height)
        {
            return data.ToBitmapSource(media.PixelFormats.Bgra32, width, height);
        }

    }

    public static class EmguImageExtensions
    {
        public static Image<TColor, TDepth> ToOpenCVImage<TColor, TDepth>(this ColorImageFrame image)
            where TColor : struct, IColor
            where TDepth : new()
        {
            var bitmap = image.ToBitmap();
            return new Image<TColor, TDepth>(bitmap);
        }


        public static Image<TColor, TDepth> ToOpenCVImage<TColor, TDepth>(this Bitmap bitmap)
            where TColor : struct, IColor
            where TDepth : new()
        {
            return new Image<TColor, TDepth>(bitmap);
        }

        public static System.Windows.Media.Imaging.BitmapSource ToBitmapSource(this IImage image)
        {
            var source = image.ToBitmapSource();
            return source;
        }
    }
}
