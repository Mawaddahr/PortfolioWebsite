using System.ComponentModel.DataAnnotations;

namespace PortfolioWebsite.Objects.InputObjects
{
    public class InputProject
    {
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
