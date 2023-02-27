﻿using PersonalBlog.Domain.Abstractions;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Domain.Entities;

public class Tag : AuditableBaseEntity
{
    public const int NameMaxLength = 50;

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    public virtual ICollection<ArticleTag> ArticleTags { get; set; } = new Collection<ArticleTag>();
}