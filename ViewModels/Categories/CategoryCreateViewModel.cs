using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AfrikanEchoes.ViewModels.Categories
{
    public class CategoryCreateViewModel
    {
        [Display(Name = "Cover Image")]
        public IFormFile CoverImage { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
