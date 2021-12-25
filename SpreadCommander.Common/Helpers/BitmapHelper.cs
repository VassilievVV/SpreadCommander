using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Numerics = System.Numerics;

namespace SpreadCommander.Common.Helpers
{
    public static class BitmapHelper
    {
        public static bool CanUseAvx        { get; set; } = true;
        public static bool CanUseSse2       { get; set; } = true;
        public static bool CanUseSIMD       { get; set; } = true;
        public static bool UseGrayConverter { get; set; } = true;

        private static bool UseAvx  { get; } = CanUseAvx && Avx.IsSupported;
        private static bool UseSse2 { get; } = CanUseSse2 & Sse2.IsSupported;
        private static bool UseSIMD { get; } = CanUseSIMD && Numerics.Vector.IsHardwareAccelerated;


        public enum BitmapChannel { Gray, Red, Green, Blue, Alpha };

        public static Vector<double> BitmapToVector(string fileName, BitmapChannel channel = BitmapChannel.Gray)
        {
            using var bitmap = Bitmap.FromFile(fileName);
            return BitmapToVector(bitmap as Bitmap, channel);
        }

        public static Vector<double> BitmapToVector(Stream stream, BitmapChannel channel = BitmapChannel.Gray)
        {
            using var bitmap = Bitmap.FromStream(stream);
            return BitmapToVector(bitmap as Bitmap, channel);
        }

        public unsafe static Vector<double> BitmapToVector(Bitmap bitmap, BitmapChannel channel = BitmapChannel.Gray)
        {
            int width      = bitmap.Width;
            int height     = bitmap.Height;
            int pixelCount = width * height;

            bool needDispose = false;
            bool isGray      = false;

            var rect = new Rectangle(0, 0, width, height);

            int depth = Bitmap.GetPixelFormatSize(bitmap.PixelFormat);

            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    break;
                default:
                    bitmap      = channel == BitmapChannel.Gray ? MakeGrayscale(bitmap) : MakeColor(bitmap);
                    needDispose = true;
                    break;
            }

            if (!needDispose && channel == BitmapChannel.Gray)
            {
                bitmap      = MakeGrayscale(bitmap);
                needDispose = true;
                isGray      = true;
            }

            var result = new double[pixelCount];

            var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            try
            {
                unsafe
                {
                    byte* scan0 = (byte*)bitmapData.Scan0.ToPointer();

                    var ptr = bitmapData.Scan0;
                    int startIndex;

                    switch (depth)
                    {
                        case 8: // For 8 bpp get color value (Red, Green and Blue values are the same)
                            if (channel == BitmapChannel.Alpha)
                                break;

                            for (int y = 0; y < bitmapData.Height; y++)
                            {
                                var rowB   = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);
                                startIndex = y * bitmapData.Width;
                                if (bitmapData.Stride < 0)
                                    startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                for (int x = 0; x < bitmapData.Width; x++)
                                    result[startIndex + x] = rowB[x];
                            }

                            PointwiseDivideInPlace(result, 256.0);
                            break;
                        case 16: // For 16 bpp - gray with 65536 shades
                            if (channel == BitmapChannel.Alpha)
                                break;

                            for (int y = 0; y < bitmapData.Height; y++)
                            {
                                var rowS   = (short*)bitmapData.Scan0 + (y * bitmapData.Stride);
                                startIndex = y * bitmapData.Width;
                                if (bitmapData.Stride < 0)
                                    startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                for (int x = 0; x < bitmapData.Width; x++)
                                    result[startIndex + x] = rowS[x];
                            }

                            PointwiseDivideInPlace(result, 65536.0);
                            break;
                        case 24: // For 24 bpp get Red, Green and Blue
                        case 32: // For 32 bpp get Red, Green, Blue and Alpha
                            if (channel == BitmapChannel.Alpha && depth == 24)
                                break;

                            int step = depth  / 8;

                            if (channel == BitmapChannel.Gray)
                            {
                                if (isGray && UseGrayConverter)
                                {
                                    for (int y = 0; y < bitmapData.Height; y++)
                                    {
                                        var row3B = (byte*)bitmapData.Scan0.ToPointer() + (y * bitmapData.Stride);
                                        startIndex = y * bitmapData.Width;
                                        if (bitmapData.Stride < 0)
                                            startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                        for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                            result[startIndex + i] = row3B[x]; //In gray image (made with method MakeGray()) R = G = B.
                                    }

                                    PointwiseDivideInPlace(result, 256.0);
                                }
                                else if (UseAvx)
                                {
                                    var vectorGrayCoeffAvx = Vector256.Create(0.11d, 0.59d, 0.3d, 0d);

                                    for (int y = 0; y < bitmapData.Height; y++)
                                    {
                                        var row3B  = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);
                                        startIndex = y * bitmapData.Width;
                                        if (bitmapData.Stride < 0)
                                            startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                        for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                        {
                                            var vectorB    = Vector128.Create((int)row3B[x], (int)row3B[x + 1], (int)row3B[x + 2], (int)0);
                                            var vectorD    = Avx.ConvertToVector256Double(vectorB);
                                            var vectorGray = Avx.Multiply(vectorD, vectorGrayCoeffAvx);
                                            double dGray   = vectorGray.GetElement(0) + vectorGray.GetElement(1) + vectorGray.GetElement(2);
                                            result[startIndex + i] = dGray;
                                        }
                                    }

                                    PointwiseDivideInPlace(result, 256.0);
                                }
                                else if (UseSIMD)
                                {
                                    var vectorGrayCoeff = new Numerics.Vector4(0.11f, 0.59f, 0.3f, 0f);

                                    for (int y = 0; y < bitmapData.Height; y++)
                                    {
                                        var row3B  = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);
                                        startIndex = y * bitmapData.Width;
                                        if (bitmapData.Stride < 0)
                                            startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                        for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                        {
                                            var vectorF = new Numerics.Vector4(row3B[x], row3B[x + 1], row3B[x + 2], 0);
                                            var fGray   = Numerics.Vector4.Dot(vectorF, vectorGrayCoeff);
                                            result[startIndex + i] = fGray;
                                        }
                                    }

                                    PointwiseDivideInPlace(result, 256.0);
                                }
                                else
                                {
                                    for (int y = 0; y < bitmapData.Height; y++)
                                    {
                                        var row3B  = (byte*)bitmapData.Scan0.ToPointer() + (y * bitmapData.Stride);
                                        startIndex = y * bitmapData.Width;
                                        if (bitmapData.Stride < 0)
                                            startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                        for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                        {
                                            double gray = 0.11d * row3B[x] + 0.59d * row3B[x + 1] + 0.11d * row3B[x + 2];
                                            result[startIndex + i] = gray;
                                        }
                                    }

                                    PointwiseDivideInPlace(result, 256.0);
                                }
                            }
                            else
                            {
                                for (int y = 0; y < bitmapData.Height; y++)
                                {
                                    var row3B  = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);
                                    startIndex = y * bitmapData.Width;
                                    if (bitmapData.Stride < 0)
                                        startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                    switch (channel)
                                    {
                                        case BitmapChannel.Red:
                                            for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                                result[startIndex + i] = row3B[x + 2];
                                            break;
                                        case BitmapChannel.Green:
                                            for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                                result[startIndex + i] = row3B[x + 1];
                                            break;
                                        case BitmapChannel.Blue:
                                            for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                                result[startIndex + i] = row3B[x];
                                            break;
                                        case BitmapChannel.Alpha:
                                            if (depth == 32)
                                            {
                                                for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                                    result[startIndex + i] = row3B[x + 3];
                                            }
                                            else
                                            {
                                                //Do nothing, 24bit images have no alpha channel
                                            }
                                            break;
                                    }
                                }

                                PointwiseDivideInPlace(result, 256.0);
                            }
                            break;
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
                if (needDispose)
                    bitmap.Dispose();
            }

            return Vector<double>.Build.Dense(result);
        }

        public static Vector<float> BitmapToVectorF(Stream stream, BitmapChannel channel = BitmapChannel.Gray)
        {
            using var bitmap = Bitmap.FromStream(stream);
            return BitmapToVectorF(bitmap as Bitmap, channel);
        }

        public unsafe static Vector<float> BitmapToVectorF(Bitmap bitmap, BitmapChannel channel = BitmapChannel.Gray)
        {
            int width      = bitmap.Width;
            int height     = bitmap.Height;
            int pixelCount = width * height;

            bool needDispose = false;
            bool isGray      = false;

            var rect = new Rectangle(0, 0, width, height);

            int depth = Bitmap.GetPixelFormatSize(bitmap.PixelFormat);

            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    break;
                default:
                    bitmap      = channel == BitmapChannel.Gray ? MakeGrayscale(bitmap) : MakeColor(bitmap);
                    needDispose = true;
                    break;
            }

            if (!needDispose && channel == BitmapChannel.Gray)
            {
                bitmap      = MakeGrayscale(bitmap);
                needDispose = true;
                isGray      = true;
            }

            var result = new float[pixelCount];

            var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            try
            {
                unsafe
                {
                    byte* scan0 = (byte*)bitmapData.Scan0.ToPointer();

                    var ptr = bitmapData.Scan0;
                    int startIndex;

                    switch (depth)
                    {
                        case 8: // For 8 bpp get color value (Red, Green and Blue values are the same)
                            if (channel == BitmapChannel.Alpha)
                                break;

                            for (int y = 0; y < bitmapData.Height; y++)
                            {
                                var rowB   = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);
                                startIndex = y * bitmapData.Width;
                                if (bitmapData.Stride < 0)
                                    startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                for (int x = 0; x < bitmapData.Width; x++)
                                    result[startIndex + x] = rowB[x];
                            }

                            PointwiseDivideInPlaceF(result, 256.0f);
                            break;
                        case 16: // For 16 bpp - gray with 65536 shades
                            if (channel == BitmapChannel.Alpha)
                                break;

                            for (int y = 0; y < bitmapData.Height; y++)
                            {
                                var rowS   = (short*)bitmapData.Scan0 + (y * bitmapData.Stride);
                                startIndex = y * bitmapData.Width;
                                if (bitmapData.Stride < 0)
                                    startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                for (int x = 0; x < bitmapData.Width; x++)
                                    result[startIndex + x] = rowS[x];
                            }

                            PointwiseDivideInPlaceF(result, 65536.0f);
                            break;
                        case 24: // For 24 bpp get Red, Green and Blue
                        case 32: // For 32 bpp get Red, Green, Blue and Alpha
                            if (channel == BitmapChannel.Alpha && depth == 24)
                                break;

                            int step = depth  / 8;

                            if (channel == BitmapChannel.Gray)
                            {
                                if (isGray && UseGrayConverter)
                                {
                                    for (int y = 0; y < bitmapData.Height; y++)
                                    {
                                        var row3B = (byte*)bitmapData.Scan0.ToPointer() + (y * bitmapData.Stride);
                                        startIndex = y * bitmapData.Width;
                                        if (bitmapData.Stride < 0)
                                            startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                        for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                            result[startIndex + i] = row3B[x]; //In gray image (made with method MakeGray()) R = G = B.
                                    }

                                    PointwiseDivideInPlaceF(result, 256.0f);
                                }
                                else if (UseAvx)
                                {
                                    var vectorGrayCoeffAvx = Vector128.Create(0.11f, 0.59f, 0.3f, 0f);

                                    for (int y = 0; y < bitmapData.Height; y++)
                                    {
                                        var row3B  = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);
                                        startIndex = y * bitmapData.Width;
                                        if (bitmapData.Stride < 0)
                                            startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                        for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                        {
                                            var vectorB    = Vector128.Create((int)row3B[x], (int)row3B[x + 1], (int)row3B[x + 2], (int)0);
                                            var vectorF    = Avx.ConvertToVector128Single(vectorB);
                                            var vectorGray = Avx.Multiply(vectorF, vectorGrayCoeffAvx);
                                            float fGray    = vectorGray.GetElement(0) + vectorGray.GetElement(1) + vectorGray.GetElement(2);
                                            result[startIndex + i] = fGray;
                                        }
                                    }

                                    PointwiseDivideInPlaceF(result, 256.0f);
                                }
                                else if (UseSIMD)
                                {
                                    var vectorGrayCoeff = new Numerics.Vector4(0.11f, 0.59f, 0.3f, 0f);

                                    for (int y = 0; y < bitmapData.Height; y++)
                                    {
                                        var row3B  = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);
                                        startIndex = y * bitmapData.Width;
                                        if (bitmapData.Stride < 0)
                                            startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                        for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                        {
                                            var vectorF = new Numerics.Vector4(row3B[x], row3B[x + 1], row3B[x + 2], 0);
                                            var fGray   = Numerics.Vector4.Dot(vectorF, vectorGrayCoeff);
                                            result[startIndex + i] = fGray;
                                        }
                                    }

                                    PointwiseDivideInPlaceF(result, 256.0f);
                                }
                                else
                                {
                                    for (int y = 0; y < bitmapData.Height; y++)
                                    {
                                        var row3B  = (byte*)bitmapData.Scan0.ToPointer() + (y * bitmapData.Stride);
                                        startIndex = y * bitmapData.Width;
                                        if (bitmapData.Stride < 0)
                                            startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                        for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                        {
                                            float gray = 0.11f * row3B[x] + 0.59f * row3B[x + 1] + 0.11f * row3B[x + 2];
                                            result[startIndex + i] = gray;
                                        }
                                    }

                                    PointwiseDivideInPlaceF(result, 256.0f);
                                }
                            }
                            else
                            {
                                for (int y = 0; y < bitmapData.Height; y++)
                                {
                                    var row3B  = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);
                                    startIndex = y * bitmapData.Width;
                                    if (bitmapData.Stride < 0)
                                        startIndex = (pixelCount - bitmapData.Width) - startIndex;

                                    switch (channel)
                                    {
                                        case BitmapChannel.Red:
                                            for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                                result[startIndex + i] = row3B[x + 2];
                                            break;
                                        case BitmapChannel.Green:
                                            for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                                result[startIndex + i] = row3B[x + 1];
                                            break;
                                        case BitmapChannel.Blue:
                                            for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                                result[startIndex + i] = row3B[x];
                                            break;
                                        case BitmapChannel.Alpha:
                                            if (depth == 32)
                                            {
                                                for (int i = 0, x = 0; i < bitmapData.Width; i++, x += step)
                                                    result[startIndex + i] = row3B[x + 3];
                                            }
                                            else
                                            {
                                                //Do nothing, 24bit images have no alpha channel
                                            }
                                            break;
                                    }
                                }

                                PointwiseDivideInPlaceF(result, 256.0f);
                            }
                            break;
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
                if (needDispose)
                    bitmap.Dispose();
            }

            return Vector<float>.Build.Dense(result);
        }

        private static void PointwiseDivideInPlace(double[] vector, double coeff)
        {
            if (UseAvx)
            {
                var vectorSize = Vector256<double>.Count;
                var vectCoeff  = Vector256.Create(coeff);

                unsafe
                {
                    fixed (double* ptrV = vector)
                    {
                        int i;
                        for (i = 0; i < vector.Length - vectorSize; i += vectorSize)
                        {
                            var vect = Avx.LoadVector256(ptrV + i);
                            var res  = Avx.Divide(vect, vectCoeff);

                            Avx.Store(ptrV + i, res);
                        }
                        for (; i < vector.Length; i++)
                            vector[i] /= coeff;
                    }
                }
            }
            else if (UseSse2)
            {
                var vectorSize = Vector128<double>.Count;
                var vectCoeff  = Vector128.Create(coeff);

                unsafe
                {
                    fixed (double* ptrV = vector)
                    {
                        int i;
                        for (i = 0; i < vector.Length - vectorSize; i += vectorSize)
                        {
                            var vect = Sse2.LoadVector128(ptrV + i);
                            var res  = Sse2.Divide(vect, vectCoeff);

                            Sse2.Store(ptrV + i, res);
                        }
                        for (; i < vector.Length; i++)
                            vector[i] /= coeff;
                    }
                }
            }
            else
            {
                for (int i = 0; i < vector.Length; i++)
                    vector[i] /= coeff;
            }
        }

        private static void PointwiseDivideInPlaceF(float[] vector, float coeff)
        {
            if (UseAvx)
            {
                var vectorSize = Vector256<float>.Count;
                var vectCoeff  = Vector256.Create(coeff);

                unsafe
                {
                    fixed (float* ptrV = vector)
                    {
                        int i;
                        for (i = 0; i < vector.Length - vectorSize; i += vectorSize)
                        {
                            var vect = Avx.LoadVector256(ptrV + i);
                            var res  = Avx.Divide(vect, vectCoeff);

                            Avx.Store(ptrV + i, res);
                        }
                        for (; i < vector.Length; i++)
                            vector[i] /= coeff;
                    }
                }
            }
            else if (UseSse2)
            {
                var vectorSize = Vector128<float>.Count;
                var vectCoeff  = Vector128.Create(coeff);

                unsafe
                {
                    fixed (float* ptrV = vector)
                    {
                        int i;
                        for (i = 0; i < vector.Length - vectorSize; i += vectorSize)
                        {
                            var vect = Sse2.LoadVector128(ptrV + i);
                            var res  = Sse2.Divide(vect, vectCoeff);

                            Sse2.Store(ptrV + i, res);
                        }
                        for (; i < vector.Length; i++)
                            vector[i] /= coeff;
                    }
                }
            }
            else
            {
                for (int i = 0; i < vector.Length; i++)
                    vector[i] /= coeff;
            }
        }

        public static Bitmap MakeGrayscale(Bitmap original)
        {
            var newBitmap    = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);
            using Graphics g = Graphics.FromImage(newBitmap);

            var colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] {.3f,  .3f,  .3f,  0,    0},
                    new float[] {.59f, .59f, .59f, 0,    0},
                    new float[] {.11f, .11f, .11f, 0,    0},
                    new float[] {0,    0,    0,    1,    0},
                    new float[] {0,    0,    0,    0,    1}
                });

            using var attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);

            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            return newBitmap;
        }

        public static Bitmap MakeColor(Bitmap original)
        {
            var newBitmap    = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);
            using Graphics g = Graphics.FromImage(newBitmap);

            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel);

            return newBitmap;
        }
    }
}
