namespace Web.Models
{
    public class  Comment
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public virtual string UserId { get; set; }
        public User User { get; set; } 
    }
}
