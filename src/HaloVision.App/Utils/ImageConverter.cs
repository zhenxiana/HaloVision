using System;
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
            HOperatorSet.GetImagePointer1(hImage, out HTuple ptr, out HTuple _, out HTuple width, out HTuple height);
            var ptrGray = new IntPtr(ptr.L);
            var w = width.I;
            var h = height.I;
            return BitmapSource.Create(w, h, 96, 96, PixelFormats.Gray8, null, ptrGray, w * h, w);
        }

        if (channels.I == 3)
        {
            HOperatorSet.GetImagePointer3(hImage, out HTuple ptrR, out HTuple ptrG, out HTuple ptrB, out HTuple _, out HTuple width, out HTuple height);
            var pR = new IntPtr(ptrR.L);
            var pG = new IntPtr(ptrG.L);
            var pB = new IntPtr(ptrB.L);
            var w = width.I;
            var h = height.I;

            var data = new byte[w * h * 3];
            for (int i = 0; i < w * h; i++)
            {
                data[i * 3] = Marshal.ReadByte(pB, i);
                data[i * 3 + 1] = Marshal.ReadByte(pG, i);
                data[i * 3 + 2] = Marshal.ReadByte(pR, i);
            }

            return BitmapSource.Create(w, h, 96, 96, PixelFormats.Bgr24, null, data, w * 3);
        }

        return null;
    }
}
