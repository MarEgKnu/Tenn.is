using Tennis.Interfaces;

namespace Tennis.Helpers
{
    public class SqlWhereCondition : ISqlWhereCondition
    {
        /// <summary>
        /// Creates a full SqlWhereCondition
        /// </summary>
        /// <param name="bracketStart"></param>
        /// <param name="attributeName"></param>
        /// <param name="token"></param>
        /// <param name="value"></param>
        /// <param name="wildCard"></param>
        /// <param name="bracketEnd"></param>
        public SqlWhereCondition(bool bracketStart, string attributeName,
                                 string token, object value, bool wildCard,
                                 bool bracketEnd) 
        {
            BracketStart = bracketStart;
            BracketEnd = bracketEnd;
            Token = token;
            Value = value;
            WildCard = wildCard;
            AttributeName = attributeName;
            BracketOnly = false;

        }
        /// <summary>
        /// Creates a condition that puts a bracket between other conditions
        /// If true, bracket will be an ending bracket ")", if false it will be a starting bracket "("
        /// </summary>
        /// <param name="bracketEnd"></param>
        public SqlWhereCondition(bool bracketEnd)
        {
            BracketOnly = true;
            if (bracketEnd)
            {
                BracketEnd = true;
            }
            else
            {
                BracketStart = true;
            }
        }

        public bool BracketStart { get; set; }
        public string AttributeName { get; set; }
        public string Token { get; set; }
        public object Value { get; set; }
        public bool WildCard { get; set; }
        public bool BracketEnd { get; set; }
        public bool BracketOnly { get; set; }
    }
}
