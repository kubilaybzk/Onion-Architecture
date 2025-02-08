using OnionArch.Domain.Entities.Common;

namespace OnionArch.Domain.Entities
{
    public class BlogImageFile : File
    {
        public bool IsHeader { get; set; }
        public string AltText { get; set; }
        public Blog Blog { get; set; }
    }
}