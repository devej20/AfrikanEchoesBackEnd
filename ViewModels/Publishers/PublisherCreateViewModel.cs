using System.ComponentModel.DataAnnotations;

namespace AfrikanEchoes.ViewModels.Publishers
{
    public class PublisherCreateViewModel
    {
        [Required]
        [Display(Name = "Publisher Name")]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
