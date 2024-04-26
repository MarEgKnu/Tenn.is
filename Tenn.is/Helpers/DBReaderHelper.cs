using System.Data;

namespace Tennis.Helpers
{
    public static class DBReaderHelper
    {
        public static string? GetStringOrNull(this IDataReader reader, int attributeColumn)
        {
            return reader.IsDBNull(attributeColumn) ? null : reader.GetString(attributeColumn);
        }
        public static int? GetIntOrNull(this IDataReader reader, int attributeColumn)
        {
            return reader.IsDBNull(attributeColumn) ? null : reader.GetInt32(attributeColumn);
        }

        public static DateTime? GetDateTimeOrNull(this IDataReader reader, int attributeColumn)
        {
            return reader.IsDBNull(attributeColumn) ? null : reader.GetDateTime(attributeColumn);
        }

        public static string? SetStringOrNull(this IDataReader reader, string? stringToCheck, int attributeColumn)
        {
            return string.IsNullOrEmpty(stringToCheck) ? null : stringToCheck;
        }
    }
}
