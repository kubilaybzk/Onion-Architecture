using OnionArch.Domain.Entities.Common;

namespace OnionArch.Domain.Entities
{
    public class BlogCategory : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }

        // Navigation property
        public ICollection<Blog> Blogs { get; set; }

        public BlogCategory()
        {
            Blogs = new HashSet<Blog>();
        }
    }
}