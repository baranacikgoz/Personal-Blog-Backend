using PersonalBlog.Domain.Abstractions;

namespace PersonalBlog.Domain.Common
{
    public abstract class AuditableBaseEntity : BaseEntity
    {
        public virtual DateTime CreatedAt { get; set; }
        public virtual int CreatedBy { get; set; }
        public virtual DateTime LastModifiedAt { get; set; }
        public virtual int LastModifiedBy { get; set; }

        public bool IsDeleted { get; set; }
        public virtual DateTime DeletedAt { get; set; }
        public virtual int DeletedBy { get; set; }

        protected AuditableBaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
            LastModifiedAt = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
