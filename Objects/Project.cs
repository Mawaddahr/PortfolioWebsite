using System.ComponentModel.DataAnnotations;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;

namespace PortfolioWebsite.Objects
{
    [Table("Projects")]
    public class  Project
    {
        public Project (){ }
        public Project(string Name, string Description, string Link, string ImageUrl)
        {
            Id = Guid.NewGuid().ToString();
            this.Name = Name;
            this.Description = Description;
            this.Link = Link;
            this.ImageUrl = ImageUrl;
        }
        [System.ComponentModel.DataAnnotations.Key]
        [Required(ErrorMessage = "Id must be specified.")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Name must be specified.")]
        [MaxLength(100, ErrorMessage = "Total characters must be less than 100")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description must be specified.")]
        [MaxLength(100, ErrorMessage = "Total characters must be less than 100")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Link must be specified.")]
        public string Link { get; set; }
        [Required(ErrorMessage = "Image url must be specified")]
        public string ImageUrl { get; set; }
    }
}
