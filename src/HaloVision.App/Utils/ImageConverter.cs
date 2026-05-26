using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HalconDotNet;

namespace HaloVision.App.Utils;

public static class ImageConverter
{
    public static BitmapSource? HObjectToBitmapSource(HObject hImage)
    {
        if (hImage is null || !hImage.IsInitialized()) return null;

        HOperatorSet.CountChannels(hImage, out HTuple channels);
        if (channels.I == 1)
        {
            HOperatorSet.GetImagePointer1(hImage, out IntPtr ptr, out _, out int width, out int height);
            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Gray8, null, ptr, width * height, width);
        }

        if (channels.I == 3)
        {
            HOperatorSet.GetImagePointer3(hImage, out IntPtr ptrR, out IntPtr ptrG, out IntPtr ptrB, out _, out int width, out int height);
            var data = new byte[width * height * 3];
            for (int i = 0; i < width * height; i++)
            {
                data[i * 3] = Marshal.ReadByte(ptrB, i);
                data[i * 3 + 1] = Marshal.ReadByte(ptrG, i);
                data[i * 3 + 2] = Marshal.ReadByte(ptrR, i);
            }

            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgr24, null, data, width * 3);
        }

        return null;
    }
}
