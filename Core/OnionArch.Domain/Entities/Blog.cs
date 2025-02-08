using OnionArch.Domain.Entities.Common;

namespace OnionArch.Domain.Entities
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public string Slug { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishDate { get; set; }
        public int ReadingTime { get; set; }

        // SEO alanları
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }

        // Navigation properties
        public ICollection<BlogImageFile> CoverImage { get; set; }
        public ICollection<BlogCategory> Categories { get; set; }
    }
}