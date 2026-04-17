using Dapper.Contrib;
using Dapper.Contrib.Extensions;

namespace PortfolioWebsite.Objects
{
    [Table("Projects")]
    public class  Project
    {
        public Project(string Name, string Description, string Link, string ImageUrl)
        {
            Id = Guid.NewGuid().ToString();
            this.Name = Name;
            this.Description = Description;
            this.Link = Link;
            this.ImageUrl = ImageUrl;
        }
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
    }
}
