namespace Web.Models
{
    public class Article : BaseEntity
    {
        
        public string Content { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int Views { get; set; }
        public string SeoUrl { get; set; }
        public List<ArticleTag> ArticleTags { get; set; }

    }
    
}
