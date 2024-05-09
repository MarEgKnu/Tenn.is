using System.Text.Json;
using System.Web;

namespace Tennis.Helpers
{
    // kilde: https://stackoverflow.com/questions/58627155/input-type-hidden-asp-for-does-not-work-for-list
    public static class SerializeHelpers
    {
        public static T DeserializeForHidden<T>(this string value, T _)
        {
            if (string.IsNullOrEmpty(value))
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }
        public static string SerializeForHidden(this object value)
        {
            if (value == null)
                return null;

            return JsonSerializer.Serialize(value);
        }

    }

}
