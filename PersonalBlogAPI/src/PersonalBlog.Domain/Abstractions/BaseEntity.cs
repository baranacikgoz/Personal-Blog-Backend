namespace PersonalBlog.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public string HashId { get; set; } = null!;
    }
}
