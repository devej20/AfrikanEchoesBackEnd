using System.ComponentModel.DataAnnotations;

namespace AfrikanEchoes.ViewModels.Languages
{
    public class LanguageEditViewModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Language Name")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
