using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioWebsite.Objects
{
    [Table("Projects")]
    public class  Project
    {
        public Project(string Name, string Description, string Link, string ImageUrl)
        {
            Id = new Guid().ToString();
            this.Name = Name;
            this.Description = Description;
            this.Link = Link;
            this.ImageUrl = ImageUrl;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
    }
}
