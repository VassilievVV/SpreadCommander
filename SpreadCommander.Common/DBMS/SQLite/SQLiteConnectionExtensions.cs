using SpreadCommander.Common.DBMS.SQLite.Functions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.DBMS.SQLite.Functions.Collations;
using SpreadCommander.Common.DBMS.SQLite.Functions.Math;
using SpreadCommander.Common.DBMS.SQLite.Functions.Path;
using SpreadCommander.Common.DBMS.SQLite.Functions.Random;
using SpreadCommander.Common.DBMS.SQLite.Functions.Regex;
using SpreadCommander.Common.DBMS.SQLite.Functions.String;
using SpreadCommander.Common.DBMS.SQLite.Functions.Aggregates;
using SpreadCommander.Common.DBMS.SQLite.Functions.Hash;
using SpreadCommander.Common.DBMS.SQLite.Functions.Guid;

namespace SpreadCommander.Common.DBMS.SQLite
{
    public static class SQLiteConnectionExtensions
    {
        public static void BindFunction(this SQLiteConnection connection, SQLiteFunction function)
        {
            var attributes = function.GetType().GetCustomAttributes(typeof(SQLiteFunctionAttribute), true).Cast<SQLiteFunctionAttribute>().ToArray();
            if (attributes.Length <= 0)
                throw new InvalidOperationException("SQLiteFunction doesn't have SQLiteFunctionAttribute");
            connection.BindFunction(attributes[0], function);
        }

        public static void BindSCFunctions(this SQLiteConnection connection)
        {
            //Static functions
            //Regex
            connection.BindFunction(new RegexIsMatchFunction());
            connection.BindFunction(new RegexMatchFunction());
            connection.BindFunction(new RegexNamedMatchFunction());
            connection.BindFunction(new RegexReplaceFunction());
            //String
            connection.BindFunction(new StringFormatFunction());
            //Hash
            connection.BindFunction(new MD5Function());
            connection.BindFunction(new SHA1Function());
            connection.BindFunction(new SHA256Function());
            connection.BindFunction(new SHA384Function());
            connection.BindFunction(new SHA512Function());
            //Path
            connection.BindFunction(new PathChangeExtensionFunction());
            connection.BindFunction(new PathCombineFunction());
            connection.BindFunction(new PathGetDirectoryNameFunction());
            connection.BindFunction(new PathGetExtensionFunction());
            connection.BindFunction(new PathGetFileNameFunction());
            connection.BindFunction(new PathGetFileNameWithoutExtensionFunction());
            //Random
            connection.BindFunction(new RandUniformFunction());
            connection.BindFunction(new RandNormalFunction());
            connection.BindFunction(new RandTriangularFunction());
            //Math
            connection.BindFunction(new AcosFunction());
            connection.BindFunction(new AsinFunction());
            connection.BindFunction(new Atan2Function());
            connection.BindFunction(new AtanFunction());
            connection.BindFunction(new CeilingFunction());
            connection.BindFunction(new CosFunction());
            connection.BindFunction(new CoshFunction());
            connection.BindFunction(new ExpFunction());
            connection.BindFunction(new FloorFunction());
            connection.BindFunction(new IEEERemainderFunction());
            connection.BindFunction(new Log10Function());
            connection.BindFunction(new LogFunction());
            connection.BindFunction(new PowFunction());
            connection.BindFunction(new SignFunction());
            connection.BindFunction(new SinFunction());
            connection.BindFunction(new SinhFunction());
            connection.BindFunction(new SqrtFunction());
            connection.BindFunction(new TanFunction());
            connection.BindFunction(new TanhFunction());
            connection.BindFunction(new TruncateFunction());
            //Guid
            connection.BindFunction(new NewIdFunction());

            //Collations
            connection.BindFunction(new LogicalCollationFunction());
            connection.BindFunction(new LogicalCollationCIFunction());

            //Aggregates
            connection.BindFunction(new GeometricMeanFunction());
            connection.BindFunction(new HarmonicMeanFunction());
            connection.BindFunction(new InterQuantileRangeFunction());
            connection.BindFunction(new KurtosisFunction());
            connection.BindFunction(new MedianFunction());
            connection.BindFunction(new PopulationKurtosisFunction());
            connection.BindFunction(new PopulationSkewnessFunction());
            connection.BindFunction(new PopulationStdevFunction());
            connection.BindFunction(new PopulationVarianceFunction());
            connection.BindFunction(new QuantileFunction());
            connection.BindFunction(new RootMeanSquareFunction());
            connection.BindFunction(new SkewnessFunction());
            connection.BindFunction(new StdevFunction());
            connection.BindFunction(new VarianceFunction());
            connection.BindFunction(new CovarianceFunction());
            connection.BindFunction(new PearsonCorrelationFunction());
            connection.BindFunction(new PopulationCovarianceFunction());
            connection.BindFunction(new SpearmanCorrelationFunction());
        }
    }
}
