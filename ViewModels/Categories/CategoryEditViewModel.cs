using System.ComponentModel.DataAnnotations;

namespace AfrikanEchoes.ViewModels.Categories
{
    public class CategoryEditViewModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
