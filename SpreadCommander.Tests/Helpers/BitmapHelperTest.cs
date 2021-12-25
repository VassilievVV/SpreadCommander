using SpreadCommander.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SpreadCommander.Tests.Helpers
{
    public class BitmapHelperTest
    {
        private readonly ITestOutputHelper output;

        public BitmapHelperTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact()]
        public void TestBitmap001()
        {
            string exeName = Assembly.GetExecutingAssembly().Location;
            string fileName = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\10.png");

            var vector = BitmapHelper.BitmapToVector(fileName, BitmapHelper.BitmapChannel.Gray);
            int nonZeroCount = vector.Where(v => v != 0.0).Count();
            output.WriteLine($"Vector length: {vector.Count}; Non-zero count: {nonZeroCount}") ;
            Assert.Equal(169, nonZeroCount);
        }

        [Fact()]
        public void CompareVectorAcceleration()
        {
            BitmapHelper.UseGrayConverter = false;
            BitmapHelper.CanUseSIMD       = false;
            BitmapHelper.CanUseSse2       = false;
            BitmapHelper.CanUseAvx        = false;

            long msec;

            const int testCount = 10_000;

            msec = Test(testCount);
            output.WriteLine($"No acceleration: {msec} msec.");

            BitmapHelper.UseGrayConverter = true;
            msec = Test(testCount);
            output.WriteLine($"Gray bitmap: {msec} msec.");

            BitmapHelper.UseGrayConverter = false;
            BitmapHelper.CanUseSIMD       = true;
            BitmapHelper.CanUseSse2       = true;
            msec = Test(testCount);
            output.WriteLine($"SSE2+SIMD:   {msec} msec.");

            BitmapHelper.CanUseAvx = true;
            msec = Test(testCount);
            output.WriteLine($"AVX:         {msec} msec.");


            static long Test(int iterations)
            {
                string exeName = Assembly.GetExecutingAssembly().Location;
                string fileName = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\10.png");

                var watch = new Stopwatch();
                watch.Start();

                using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                for (int i = 0; i < iterations; i++)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    var vector = BitmapHelper.BitmapToVector(stream, BitmapHelper.BitmapChannel.Gray);
                }

                watch.Stop();
                return watch.ElapsedMilliseconds;
            }
        }

        [Fact()]
        public void CompareVectorAccelerationParallel()
        {
            BitmapHelper.UseGrayConverter = false;
            BitmapHelper.CanUseSIMD = false;
            BitmapHelper.CanUseSse2 = false;
            BitmapHelper.CanUseAvx = false;

            long msec;

            const int testCount = 10_000;

            msec = Test(testCount);
            output.WriteLine($"No acceleration: {msec} msec.");

            BitmapHelper.UseGrayConverter = true;
            msec = Test(testCount);
            output.WriteLine($"Gray bitmap: {msec} msec.");

            BitmapHelper.UseGrayConverter = false;
            BitmapHelper.CanUseSIMD = true;
            BitmapHelper.CanUseSse2 = true;
            msec = Test(testCount);
            output.WriteLine($"SSE2+SIMD:   {msec} msec.");

            BitmapHelper.CanUseAvx = true;
            msec = Test(testCount);
            output.WriteLine($"AVX:         {msec} msec.");


            static long Test(int iterations)
            {
                string exeName = Assembly.GetExecutingAssembly().Location;
                string fileName = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\10.png");

                var watch = new Stopwatch();
                watch.Start();

                Parallel.For(0, testCount, (x) =>
                {
                    var vector = BitmapHelper.BitmapToVector(fileName, BitmapHelper.BitmapChannel.Gray);
                });

                watch.Stop();
                return watch.ElapsedMilliseconds;
            }
        }

        [Fact()]
        public void CompareVectorAccelerationZip()
        {
            BitmapHelper.UseGrayConverter = false;
            BitmapHelper.CanUseSIMD = false;
            BitmapHelper.CanUseSse2 = false;
            BitmapHelper.CanUseAvx = false;

            long msec;

            msec = Test();
            output.WriteLine($"No acceleration: {msec} msec.");

            BitmapHelper.UseGrayConverter = true;
            msec = Test();
            output.WriteLine($"Gray bitmap: {msec} msec.");

            BitmapHelper.UseGrayConverter = false;
            BitmapHelper.CanUseSIMD = true;
            BitmapHelper.CanUseSse2 = true;
            msec = Test();
            output.WriteLine($"SSE2+SIMD:   {msec} msec.");

            BitmapHelper.CanUseAvx = true;
            msec = Test();
            output.WriteLine($"AVX:         {msec} msec.");


            static long Test()
            {
                string exeName = Assembly.GetExecutingAssembly().Location;
                string fileName = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\mnist_png.zip");

                var watch = new Stopwatch();
                watch.Start();

                using var zip = ZipFile.OpenRead(fileName);
                foreach (var entry in zip.Entries)
                {
                    if (string.Compare(Path.GetExtension(entry.FullName), ".png", true) != 0)
                        continue;

                    using var stream = entry.Open();
                    var vector = BitmapHelper.BitmapToVector(stream, BitmapHelper.BitmapChannel.Gray);
                }

                watch.Stop();
                return watch.ElapsedMilliseconds;
            }
        }
    }
}
