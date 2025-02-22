using System.Data;

namespace MovieVault.Data.Utilities
{
    public static class DataReaderExtensions
    {
        public static T SafeGet<T>(this IDataReader reader, string columnName)
        {
            object value = reader[columnName];
            return value != DBNull.Value ? (T)Convert.ChangeType(value, typeof(T)) : default!;
        }
    }
}
