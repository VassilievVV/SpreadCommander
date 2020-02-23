using System;
using System.Data;
using System.Linq;

namespace SpreadCommander.Common.SqlScript
{
    public class ScriptRunParameter
    {
        public string Name      { get; set; }
        public Type Type        { get; set; }
        public DbType DbType    { get; set; }
        public object Value     { get; set; }
        public ParameterDirection Direction { get; set; } = ParameterDirection.Input;

        public static DbType GetColumnDbType(Type dataType)
        {
            (var result, var _) = GetColumnDbType(dataType, 0);
            return result;
        }

        public static (DbType dbType, int len) GetColumnDbType(Type dataType, int maxLength)
        {
            if (dataType == typeof(Guid))
                return (DbType.String, 50);

            return System.Type.GetTypeCode(dataType) switch
            {
                TypeCode.Boolean  => (DbType.Boolean, 0),
                TypeCode.Byte     => (DbType.Byte, 0),
                TypeCode.Char     => (DbType.String, maxLength > 1000 ? int.MaxValue / 2 : maxLength),
                TypeCode.DateTime => (DbType.DateTime, 0),
                TypeCode.Decimal  => (DbType.Double, 0),
                TypeCode.Double   => (DbType.Double, 0),
                TypeCode.Int16    => (DbType.Int16, 0),
                TypeCode.Int32    => (DbType.Int32, 0),
                TypeCode.Int64    => (DbType.Int64, 0),
                TypeCode.Object   => (DbType.Binary, int.MaxValue),
                TypeCode.SByte    => (DbType.Int16, 0),
                TypeCode.Single   => (DbType.Double, 0),
                TypeCode.String   => (DbType.String, maxLength > 1000 ? int.MaxValue / 2 : maxLength),
                TypeCode.UInt16   => (DbType.UInt16, 0),
                TypeCode.UInt32   => (DbType.UInt32, 0),
                TypeCode.UInt64   => (DbType.UInt64, 0),
                _                 => (DbType.Binary, int.MaxValue)
            };
        }
    }
}
