using MathNet.Numerics.LinearAlgebra;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SpreadCommander.Tests.Helpers
{
    public class MathHelperTest
    {
        private readonly ITestOutputHelper output;

        public MathHelperTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact()]
        public void TestCompressionBrotli()
        {
            var watch = new Stopwatch();
            watch.Start();

            string exeName  = Assembly.GetExecutingAssembly().Location;
            string fileName = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\mnist_png.zip");

            long compLength = 0;

            using var zip = ZipFile.OpenRead(fileName);
            foreach (var entry in zip.Entries)
            {
                if (string.Compare(Path.GetExtension(entry.FullName), ".png", true) != 0)
                    continue;

                using var stream = entry.Open();
                using var memStream = new MemoryStream();
                stream.CopyTo(memStream);
                byte[] image = memStream.ToArray();
                Utils.ReadStreamToBuffer(stream, image);

                int maxCompSize = BrotliEncoder.GetMaxCompressedLength(image.Length);
                byte[] compImage = new byte[maxCompSize];
                if (!BrotliEncoder.TryCompress(new ReadOnlySpan<byte>(image), new Span<byte>(compImage), out int bytesWritten, 11, 16))
                    throw new Exception("Cannot compress image");

                compLength += bytesWritten;
            }

            watch.Stop();
            output.WriteLine($"Brotli: {watch.ElapsedMilliseconds:N0} msec.");
            output.WriteLine($"Summary length: {compLength:N0}");
        }

        [Fact()]
        public void TestCompressionGzip()
        {
            var watch = new Stopwatch();
            watch.Start();

            string exeName = Assembly.GetExecutingAssembly().Location;
            string fileName = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\mnist_png.zip");

            long compLength = 0;

            using var zip = ZipFile.OpenRead(fileName);
            foreach (var entry in zip.Entries)
            {
                if (string.Compare(Path.GetExtension(entry.FullName), ".png", true) != 0)
                    continue;

                using var stream = entry.Open();
                using var memStream = new MemoryStream();
                stream.CopyTo(memStream);
                memStream.Seek(0, SeekOrigin.Begin);

                using var compStream = new MemoryStream();
                using var gzStream = new GZipStream(compStream, CompressionLevel.Optimal);
                memStream.CopyTo(gzStream);
                gzStream.Flush();

                compStream.Seek(0, SeekOrigin.Begin);

                compLength += compStream.Length;
            }

            watch.Stop();
            output.WriteLine($"GZip: {watch.ElapsedMilliseconds:N0} msec.");
            output.WriteLine($"Summary length: {compLength:N0}");
        }

        [Fact()]
        public void TestCompressionDeflate()
        {
            var watch = new Stopwatch();
            watch.Start();

            string exeName = Assembly.GetExecutingAssembly().Location;
            string fileName = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\mnist_png.zip");

            long compLength = 0;

            using var zip = ZipFile.OpenRead(fileName);
            foreach (var entry in zip.Entries)
            {
                if (string.Compare(Path.GetExtension(entry.FullName), ".png", true) != 0)
                    continue;

                using var stream = entry.Open();
                using var memStream = new MemoryStream();
                stream.CopyTo(memStream);
                memStream.Seek(0, SeekOrigin.Begin);

                byte[] image = new byte[memStream.Length];
                Utils.ReadStreamToBuffer(memStream, image);

                memStream.Seek(0, SeekOrigin.Begin);

                using var compStream = new MemoryStream();
                using var zipStream = new DeflateStream(compStream, CompressionLevel.Optimal);
                memStream.CopyTo(zipStream);
                zipStream.Flush();

                compStream.Seek(0, SeekOrigin.Begin);

                compLength += compStream.Length;
            }

            watch.Stop();
            output.WriteLine($"Deflate: {watch.ElapsedMilliseconds:N0} msec.");
            output.WriteLine($"Summary length: {compLength:N0}");
        }

        /*
        [Fact()]
        public void TestCompressionZstd()
        {
            var watch = new Stopwatch();
            watch.Start();

            string exeName = Assembly.GetExecutingAssembly().Location;
            string fileName = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\mnist_png.zip");

            long compLength = 0;

            using var zip = ZipFile.OpenRead(fileName);
            foreach (var entry in zip.Entries)
            {
                if (string.Compare(Path.GetExtension(entry.FullName), ".png", true) != 0)
                    continue;

                using var stream    = entry.Open();
                using var memStream = new MemoryStream();
                stream.CopyTo(memStream);
                memStream.Seek(0, SeekOrigin.Begin);

                byte[] image = new byte[memStream.Length];
                Utils.ReadStreamToBuffer(memStream, image);

                memStream.Seek(0, SeekOrigin.Begin);

                using var compStream = new MemoryStream();
                using var zstdStream  = new ZstdNet.CompressionStream(compStream, new ZstdNet.CompressionOptions(-7));
                memStream.CopyTo(zstdStream);
                zstdStream.Flush();

                compStream.Seek(0, SeekOrigin.Begin);

                compLength += compStream.Length;
            }

            watch.Stop();
            output.WriteLine($"Zstd: {watch.ElapsedMilliseconds:N0} msec.");
            output.WriteLine($"Summary length: {compLength:N0}");
        }

        [Fact()]
        public void TestCompressionLz4()
        {
            var watch = new Stopwatch();
            watch.Start();

            string exeName  = Assembly.GetExecutingAssembly().Location;
            string fileName = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\mnist_png.zip");

            long compLength = 0;

            using var zip = ZipFile.OpenRead(fileName);
            foreach (var entry in zip.Entries)
            {
                if (string.Compare(Path.GetExtension(entry.FullName), ".png", true) != 0)
                    continue;

                using var stream = entry.Open();
                using var memStream = new MemoryStream();
                stream.CopyTo(memStream);
                memStream.Seek(0, SeekOrigin.Begin);

                byte[] image = new byte[memStream.Length];
                Utils.ReadStreamToBuffer(memStream, image);

                memStream.Seek(0, SeekOrigin.Begin);

                var target = new byte[K4os.Compression.LZ4.LZ4Codec.MaximumOutputSize(image.Length)];
                int len = K4os.Compression.LZ4.LZ4Codec.Encode(image, 0, image.Length, target, 0, target.Length, K4os.Compression.LZ4.LZ4Level.L12_MAX);

                compLength += target.Length;
            }

            watch.Stop();
            output.WriteLine($"LZ4: {watch.ElapsedMilliseconds:N0} msec.");
            output.WriteLine($"Summary length: {compLength:N0}");
        }

        [Fact()]
        public void TestCompressionLZMA()
        {
            var watch = new Stopwatch();
            watch.Start();

            string exeName = Assembly.GetExecutingAssembly().Location;
            string fileName = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\mnist_png.zip");

            long compLength = 0;

            using var zip = ZipFile.OpenRead(fileName);
            foreach (var entry in zip.Entries)
            {
                if (string.Compare(Path.GetExtension(entry.FullName), ".png", true) != 0)
                    continue;

                using var stream = entry.Open();
                using var memStream = new MemoryStream();
                stream.CopyTo(memStream);
                memStream.Seek(0, SeekOrigin.Begin);

                byte[] image = new byte[memStream.Length];
                Utils.ReadStreamToBuffer(memStream, image);

                memStream.Seek(0, SeekOrigin.Begin);

                using var compStream = new MemoryStream();
                SevenZip.Helper.Compress(memStream, compStream);

                compStream.Seek(0, SeekOrigin.Begin);

                compLength += compStream.Length;
            }

            watch.Stop();
            output.WriteLine($"LZMA: {watch.ElapsedMilliseconds:N0} msec.");
            output.WriteLine($"Summary length: {compLength:N0}");
        }
        */

        [Fact()]
        public void TestSqliteImagesDatabase()
        {
            var watch = new Stopwatch();
            watch.Start();

            string exeName = Assembly.GetExecutingAssembly().Location;
            string fileName = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\mnist_png.zip");
            string dbFileName = Path.Combine(Path.GetDirectoryName(fileName), "mnist_db.db");

            if (File.Exists(dbFileName))
                File.Delete(dbFileName);

            SQLiteConnection.CreateFile(dbFileName);
            using var conn = new SQLiteConnection();
            conn.ConnectionString = $"Data Source={dbFileName}";
            conn.Open();

            using var session = conn.BeginTransaction();
            try
            {
                using var cmdCreateTable = conn.CreateCommand();
                cmdCreateTable.CommandText = "create table Images ([Group] text, Category text, FileName text, Image blob)";
                cmdCreateTable.ExecuteNonQuery();

                using var cmdInsert = conn.CreateCommand();
                cmdInsert.CommandText = "insert into Images([Group], Category, FileName, Image) values (@group, @category, @fileName, @image)";
                var paramGroup        = cmdInsert.Parameters.Add("@group", System.Data.DbType.String);
                var paramCategory     = cmdInsert.Parameters.Add("@category", System.Data.DbType.String);
                var paramFileName     = cmdInsert.Parameters.Add("@fileName", System.Data.DbType.String);
                var paramImage        = cmdInsert.Parameters.Add("@image", System.Data.DbType.Binary);
                cmdInsert.Prepare();

                using var zip = ZipFile.OpenRead(fileName);
                foreach (var entry in zip.Entries)
                {
                    if (string.Compare(Path.GetExtension(entry.FullName), ".png", true) != 0)
                        continue;

                    using var stream = entry.Open();
                    using var memStream = new MemoryStream();
                    stream.CopyTo(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);

                    byte[] image = memStream.ToArray();

                    var folder          = Path.GetDirectoryName(entry.FullName);
                    paramGroup.Value    = DBNull.Value;
                    paramCategory.Value = folder;
                    paramFileName.Value = entry.Name;
                    paramImage.Value    = image;

                    cmdInsert.ExecuteNonQuery();
                }
            }
            finally
            {
                session.Commit();
            }

            watch.Stop();
            output.WriteLine($"Write SQLite db: {watch.ElapsedMilliseconds:N0} msec.");
        }

        [Fact()]
        public void TestSqliteImagesDatabase2()
        {
            var watch = new Stopwatch();
            watch.Start();

            string exeName    = Assembly.GetExecutingAssembly().Location;
            string fileName   = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\mnist_png.zip");
            string dbFileName = Path.Combine(Path.GetDirectoryName(fileName), "mnist_vector.db");

            if (File.Exists(dbFileName))
                File.Delete(dbFileName);

            SQLiteConnection.CreateFile(dbFileName);
            using var conn        = new SQLiteConnection();
            conn.ConnectionString = $"Data Source={dbFileName}";
            conn.Open();

            using var session = conn.BeginTransaction();
            try
            {
                using var cmdCreateTable = conn.CreateCommand();
                cmdCreateTable.CommandText = "create table Images ([Group] text, Category text, FileName text, ImageVector blob)";
                cmdCreateTable.ExecuteNonQuery();

                using var cmdInsert   = conn.CreateCommand();
                cmdInsert.CommandText = "insert into Images([Group], Category, FileName, ImageVector) values (@group, @category, @fileName, @imageVector)";
                var paramGroup        = cmdInsert.Parameters.Add("@group", System.Data.DbType.String);
                var paramCategory     = cmdInsert.Parameters.Add("@category", System.Data.DbType.String);
                var paramFileName     = cmdInsert.Parameters.Add("@fileName", System.Data.DbType.String);
                var paramImageVector  = cmdInsert.Parameters.Add("@imageVector", System.Data.DbType.Binary);
                cmdInsert.Prepare();

                using var zip = ZipFile.OpenRead(fileName);
                foreach (var entry in zip.Entries)
                {
                    if (string.Compare(Path.GetExtension(entry.FullName), ".png", true) != 0)
                        continue;

                    using var stream = entry.Open();
                    var vector  = BitmapHelper.BitmapToVector(stream);
                    var dVector = vector.AsArray() ?? vector.ToArray();

                    using var compStream = new MemoryStream();
                    using var zipStream  = new DeflateStream(compStream, CompressionLevel.Optimal);
                    unsafe
                    {
                        fixed (double* ptr = dVector)
                        {
                            long streamSize = dVector.Length * sizeof(double);
                            using var memVectorStream = new UnmanagedMemoryStream((byte*)ptr, streamSize, streamSize, FileAccess.Read);
                            memVectorStream.CopyTo(zipStream);
                        }
                    }
                    zipStream.Flush();

                    compStream.Seek(0, SeekOrigin.Begin);
                    var bCompVector = compStream.ToArray();

                    var folder = Path.GetDirectoryName(entry.FullName);
                    string group, category;

                    var matchFolder = Regex.Match(folder, @"[\\/]\s*(?<Group>.*?)\s*[\\/]\s*(?<Category>.*?)\s*$");
                    if (matchFolder.Success)
                    {
                        group    = matchFolder.Groups["Group"].Value;
                        category = matchFolder.Groups["Category"].Value;
                    }
                    else
                    {
                        group    = "unknown";
                        category = "unknown";
                    }

                    paramGroup.Value       = group;
                    paramCategory.Value    = category;
                    paramFileName.Value    = entry.Name;
                    paramImageVector.Value = bCompVector;

                    cmdInsert.ExecuteNonQuery();
                }
            }
            finally
            {
                session.Commit();
            }

            watch.Stop();
            output.WriteLine($"Write SQLite db: {watch.ElapsedMilliseconds:N0} msec.");
        }

        [Fact()]
        public void TestReadSqliteImageDatabase()
        {
            var watch = new Stopwatch();
            watch.Start();

            string exeName    = Assembly.GetExecutingAssembly().Location;
            string fileName   = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\mnist_png.zip");
            string dbFileName = Path.Combine(Path.GetDirectoryName(fileName), "mnist_db.db");

            using var conn = new SQLiteConnection();
            conn.ConnectionString = $"Data Source={dbFileName}";
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "select [Group], Category, FileName, Image from Images";

            long sizeImages = 0;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string group         = Convert.ToString(reader.GetValue(0));
                string category      = Convert.ToString(reader.GetValue(1));
                string imageFileName = Convert.ToString(reader.GetValue(2));
                
                using var streamImage = reader.GetStream(3);
                var image = new byte[streamImage.Length];
                Utils.ReadStreamToBuffer(streamImage, image);

                sizeImages += image.Length;
            }

            watch.Stop();
            output.WriteLine($"Reader SQLite db: {watch.ElapsedMilliseconds:N0} msec.");

            output.WriteLine($"Size of images: {sizeImages:N0}.");
        }

        [Fact()]
        public void TestReadSqliteImageDatabase2()
        {
            var watch = new Stopwatch();
            watch.Start();

            string exeName    = Assembly.GetExecutingAssembly().Location;
            string fileName   = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\mnist_png.zip");
            string dbFileName = Path.Combine(Path.GetDirectoryName(fileName), "mnist_db.db");

            using var conn = new SQLiteConnection();
            conn.ConnectionString = $"Data Source={dbFileName}";
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "select [Group], Category, FileName, Image from Images";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string group         = Convert.ToString(reader.GetValue(0));
                string category      = Convert.ToString(reader.GetValue(1));
                string imageFileName = Convert.ToString(reader.GetValue(2));

                using var streamImage = reader.GetStream(3);
                //var image = new byte[streamImage.Length];
                //Utils.ReadStreamToBuffer(streamImage, image);

                using var bmp = Bitmap.FromStream(streamImage);   //2.631 sec.
                //var vector = BitmapHelper.BitmapToVector(streamImage);  //6.160 sec
            }

            watch.Stop();
            output.WriteLine($"Reader SQLite db: {watch.ElapsedMilliseconds:N0} msec.");
        }

        [Fact()]
        public void TestReadSqliteImageDatabase3()
        {
            var watch = new Stopwatch();
            watch.Start();

            string exeName    = Assembly.GetExecutingAssembly().Location;
            string fileName   = Path.Combine(Path.GetDirectoryName(exeName), @"..\..\..\..\..\..\Data\mnist_png.zip");
            string dbFileName = Path.Combine(Path.GetDirectoryName(fileName), "mnist_vector.db");

            using var conn = new SQLiteConnection();
            conn.ConnectionString = $"Data Source={dbFileName}";
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "select [Group], Category, FileName, ImageVector from Images";

            var categories = new Dictionary<string, int>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string group         = Convert.ToString(reader.GetValue(0));
                string category      = Convert.ToString(reader.GetValue(1));
                string imageFileName = Convert.ToString(reader.GetValue(2));

                string groupCategory = $"{group}/{category}";
                if (!categories.ContainsKey(groupCategory))
                    categories.Add(groupCategory, 1);
                else
                    categories[groupCategory]++;

                using var streamImage = reader.GetStream(3);

                using var decompStream = new MemoryStream();
                using var zipStream    = new DeflateStream(streamImage, CompressionMode.Decompress);
                zipStream.CopyTo(decompStream);
                decompStream.Seek(0, SeekOrigin.Begin);

                double[] dVector = new double[decompStream.Length / sizeof(double)];
                unsafe
                {
                    fixed (double* ptr = dVector)
                    {
                        long streamSize = dVector.Length * sizeof(double);
                        using var memVectorStream = new UnmanagedMemoryStream((byte*)ptr, streamSize, streamSize, FileAccess.Write);
                        decompStream.CopyTo(memVectorStream);
                    }
                }
                var vector = Vector<double>.Build.Dense(dVector);
            }

            watch.Stop();
            output.WriteLine($"Reader SQLite db: {watch.ElapsedMilliseconds:N0} msec.");
            output.WriteLine(string.Empty);

            output.WriteLine("Categories:");
            output.WriteLine("-----------");
            foreach (KeyValuePair<string, int> pairCategory in categories)
                output.WriteLine($@"{pairCategory.Key,-15}: {pairCategory.Value,8:N0}");
        }
    }
}

//Brotli
//7 - 17'777'826 bytes, 3.161 sec
//11 - 17'777'829 bytes, 120.682 sec
//5 - 17'777'793 bytes, 2.941 sec
//1 - 17'777'829, 2.072 sec

//GZip
//Optimal - 18'815'065, 2.941 sec
//Fastest - 18'886'405, 2.849 sec

//Deflate
//Optimal - 18'115'065, 2.998 sec
//Fastest - 18'186'405, 2.833 sec

//Zstd
//Default (3) - 18'127'829, 14'727 msec.
//-7 - 18'127'829, 3'676 msec.
//10 - 18'127'829, 165'280 msec.

//LZ4
//Fast - 18'654'259, 1'469 msec.
//Max - 18'654'259, 2'158 msec.
//7 - 18'654'259, 1'994 msec.

//7z
//Default - 19'376'018, 240'664 msec.