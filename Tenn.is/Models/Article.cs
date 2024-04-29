namespace Tennis.Models
{
    public class Article
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? AuthorID { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime? LastEdited { get; set; }
        public string? ImgPath { get; set; }

        public Article(int articleId, string title, string content, int? authorId, DateTime timeStamp, DateTime? lastEdited, string? imgPath)
        {
            ArticleID = articleId;
            Title = title;
            Content = content;
            TimeStamp = timeStamp;
            AuthorID = authorId;
            LastEdited = lastEdited;
            ImgPath = imgPath;
        }

        //public Article(string title, string content, string? imgPath)
        //{
        //    Title = title;
        //    Content = content;
        //    if (ImgPath != null) ImgPath = imgPath;
        //}
        public Article(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}
