namespace Tennis.Helpers
{
    public static class LaneBookingHelpers
    {
        public static Dictionary<int, string> HourOptions = new Dictionary<int, string>()
            {
                {
                    8 , "8:00"
                },
                {
                    9 , "9:00"
                },
                {
                    10 , "10:00"
                },
                {
                    11 , "11:00"
                },
                {
                    12 , "12:00"
                },
                {
                    13 , "13:00"
                },
                {
                    14 , "14:00"
                },
                {
                    15 , "15:00"
                },
                {
                    16 , "16:00"
                },
                {
                    17 , "17:00"
                },
                {
                    18 , "18:00"   
                },
                {
                    19 , "19:00"
                },
                {
                    20,"20:00"
                },
                {
                    21,"21:00"
                },
                {
                    22,"22:00"
                }
            };
        public static Dictionary<DayOfWeek, string> DayOptions = new Dictionary<DayOfWeek, string>()
            {
                { DayOfWeek.Monday, "Mandag"},
                { DayOfWeek.Tuesday, "Tirsdag"},
                { DayOfWeek.Wednesday, "Onsdag"},
                { DayOfWeek.Thursday, "Torsdag"},
                { DayOfWeek.Friday, "Fredag"},
                { DayOfWeek.Saturday, "Lørdag"},
                { DayOfWeek.Sunday, "Søndag"}
            };
    }
    
}
