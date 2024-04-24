namespace Tennis.Interfaces
{
    public interface ISqlWhereCondition
    {
        public bool BracketStart { get; set; }

        public string AttributeName { get; set; }

        public string Token { get; set; }

        public object Value { get; set; }

        public bool WildCard { get; set; }

        public bool BracketEnd { get; set; }

        public bool BracketOnly { get; set; }


    }
}
