using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Helpers
{
    public static class ArchiveHelper
    {
        public static byte[] CompressBytes(byte[] data)
        {
            using var dataStream = new MemoryStream(data, false);
            using var compStream = new MemoryStream();
            using var zipStream  = new DeflateStream(compStream, CompressionLevel.Optimal, true);

            dataStream.CopyTo(zipStream);
            zipStream.Flush();
            compStream.Seek(0, SeekOrigin.Begin);

            return compStream.ToArray();
        }

        public static byte[] DecompressBytes(byte[] data)
        {
            using var dataStream   = new MemoryStream(data, false);
            using var decompStream = new MemoryStream();
            using var zipStream    = new DeflateStream(dataStream, CompressionMode.Decompress);
            
            zipStream.CopyTo(decompStream);
            decompStream.Seek(0, SeekOrigin.Begin);

            return decompStream.ToArray();
        }

        public static byte[] CompressText(string value, Encoding encoding)
        {
            var data = (encoding ?? Encoding.UTF8).GetBytes(value);

            using var dataStream = new MemoryStream(data, false);
            using var compStream = new MemoryStream();
            using var zipStream  = new DeflateStream(compStream, CompressionLevel.Optimal);

            dataStream.CopyTo(zipStream);
            zipStream.Flush();
            compStream.Seek(0, SeekOrigin.Begin);

            return compStream.ToArray();
        }

        private static Encoding StringToEncoding(string encoding)
        {
            if (string.IsNullOrWhiteSpace(encoding))
                return Encoding.UTF8;
            if (string.Compare(encoding, "UTF8", true) == 0 ||
                string.Compare(encoding, "UTF-8", true) == 0)
                return Encoding.UTF8;
            if (string.Compare(encoding, "ASCII", true) == 0)
                return Encoding.ASCII;
            return Encoding.GetEncoding(encoding);
        }

        public static byte[] CompressText(string value, string encoding = "UTF8") =>
            CompressText(value, StringToEncoding(encoding));

        public static string DecompressText(byte[] data, Encoding encoding)
        {
            using var dataStream   = new MemoryStream(data, false);
            using var decompStream = new MemoryStream();
            using var zipStream    = new DeflateStream(dataStream, CompressionMode.Decompress);
            
            zipStream.CopyTo(decompStream);
            decompStream.Seek(0, SeekOrigin.Begin);

            var textArray = decompStream.ToArray();
            var result    = (encoding ?? Encoding.UTF8).GetString(textArray);
            return result;
        }

        public static string DecompressText(byte[] data, string encoding = "UTF8") =>
            DecompressText(data, StringToEncoding(encoding));

        public static byte[] CompressVector(Vector<double> data)
        {
            var vector = data.AsArray() ?? data.ToArray();

            using var compStream = new MemoryStream();
            using var zipStream  = new DeflateStream(compStream, CompressionLevel.Optimal);
            unsafe
            {
                fixed (double* ptr = vector)
                {
                    long streamSize = vector.Length * sizeof(double);
                    using var memVectorStream = new UnmanagedMemoryStream((byte*)ptr, streamSize, streamSize, FileAccess.Read);
                    memVectorStream.CopyTo(zipStream);
                }
            }
            zipStream.Flush();
            compStream.Seek(0, SeekOrigin.Begin);

            return compStream.ToArray();
        }

        public static Vector<double> DecompressVector(byte[] data)
        {
            using var dataStream   = new MemoryStream(data, false);
            using var decompStream = new MemoryStream();
            using var zipStream    = new DeflateStream(dataStream, CompressionMode.Decompress);
            
            zipStream.CopyTo(decompStream);
            decompStream.Seek(0, SeekOrigin.Begin);

            if (decompStream.Length % sizeof(double) != 0)
                throw new Exception("Invalid size of binary data.");

            double[] dVector = new double[decompStream.Length / sizeof(double)];
            unsafe
            {
                fixed (double* ptr = dVector)
                {
                    long streamSize           = dVector.Length * sizeof(double);
                    using var memVectorStream = new UnmanagedMemoryStream((byte*)ptr, streamSize, streamSize, FileAccess.Write);
                    decompStream.CopyTo(memVectorStream);
                }
            }

            var vector = Vector<double>.Build.Dense(dVector);
            return vector;
        }

        public static byte[] CompressVectorF(Vector<float> data)
        {
            var vector = data.AsArray() ?? data.ToArray();

            using var compStream = new MemoryStream();
            using var zipStream  = new DeflateStream(compStream, CompressionLevel.Optimal);
            unsafe
            {
                fixed (float* ptr = vector)
                {
                    long streamSize = vector.Length * sizeof(float);
                    using var memVectorStream = new UnmanagedMemoryStream((byte*)ptr, streamSize, streamSize, FileAccess.Read);
                    memVectorStream.CopyTo(zipStream);
                }
            }
            zipStream.Flush();
            compStream.Seek(0, SeekOrigin.Begin);

            return compStream.ToArray();
        }

        public static Vector<float> DecompressVectorF(byte[] data)
        {
            using var dataStream   = new MemoryStream(data, false);
            using var decompStream = new MemoryStream();
            using var zipStream    = new DeflateStream(dataStream, CompressionMode.Decompress);
            
            zipStream.CopyTo(decompStream);
            decompStream.Seek(0, SeekOrigin.Begin);

            if (decompStream.Length % sizeof(float) != 0)
                throw new Exception("Invalid size of binary data.");

            float[] fVector = new float[decompStream.Length / sizeof(float)];
            unsafe
            {
                fixed (float* ptr = fVector)
                {
                    long streamSize           = fVector.Length * sizeof(float);
                    using var memVectorStream = new UnmanagedMemoryStream((byte*)ptr, streamSize, streamSize, FileAccess.Write);
                    decompStream.CopyTo(memVectorStream);
                }
            }

            var vector = Vector<float>.Build.Dense(fVector);
            return vector;
        }
    }
}
