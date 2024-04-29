namespace Tennis.Helpers
{
    public static class BoolHelpers
    {
        public static readonly Dictionary<bool, string> CancelledBoolKeyValuePair = new Dictionary<bool, string>()
        {
            { false, "Ikke aflyst" },
            { true, "Aflyst" }
        };

    public static string ToJaNejValue(bool? value)
        {
            switch(value)
            {
                case true: 
                    return "Ja";
                case false:
                    return "Nej";
                default:
                    return "Ukendt";
            }
        }
        public static string ToJaNejValue(bool value)
        {
            switch (value)
            {
                case true:
                    return "Ja";
                case false:
                    return "Nej";
                default:
                    return "Ukendt";
            }
        }
    }
}
