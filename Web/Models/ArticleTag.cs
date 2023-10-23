namespace Web.Models
{
    public class ArticleTag : BaseEntity
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }

    }
}
