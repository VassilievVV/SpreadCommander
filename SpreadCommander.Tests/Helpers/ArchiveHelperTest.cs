using MathNet.Numerics.LinearAlgebra;
using SpreadCommander.Common.Helpers;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SpreadCommander.Tests.Helpers
{
    public class ArchiveHelperTest
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ITestOutputHelper output;
#pragma warning restore IDE0052 // Remove unread private members

        public ArchiveHelperTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact()]
        public void TestCompressDecompress()
        {
            var data  = new byte[1000];
            for (int i = 0; i < data.Length; i++)
                data[i] = (byte)((i + 1) % 256);

            var comp  = ArchiveHelper.CompressBytes(data);
            var data2 = ArchiveHelper.DecompressBytes(comp);
            Assert.Equal(data, data2);
        }

        [Fact()]
        public void TestCompressDecompressText()
        {
            var value  = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var comp   = ArchiveHelper.CompressText(value);
            var value2 = ArchiveHelper.DecompressText(comp);
            Assert.Equal(value, value2);
        }

        [Fact()]
        public void TestCompressDecompressVector()
        {
            var vector  = Vector<double>.Build.Random(1000);
            var comp    = ArchiveHelper.CompressVector(vector);
            var vector2 = ArchiveHelper.DecompressVector(comp);
            Assert.Equal(vector, vector2);
        }

        [Fact()]
        public void TestCompressDecompressVectorF()
        {
            var vector  = Vector<float>.Build.Random(1000);
            var comp    = ArchiveHelper.CompressVectorF(vector);
            var vector2 = ArchiveHelper.DecompressVectorF(comp);
            Assert.Equal(vector, vector2);
        }
    }
}
