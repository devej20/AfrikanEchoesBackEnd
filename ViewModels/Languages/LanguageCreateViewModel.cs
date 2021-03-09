using System.ComponentModel.DataAnnotations;

namespace AfrikanEchoes.ViewModels.Languages
{
    public class LanguageCreateViewModel
    {
        [Required]
        [Display(Name = "Language Name")]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
