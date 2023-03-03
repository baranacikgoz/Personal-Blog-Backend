using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Domain.Abstractions
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        [MaxLength(12)] // Currently it is set to 12 from appsettings.json
        public string HashId { get; set; } = null!;
    }
}
