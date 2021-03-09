using System.ComponentModel.DataAnnotations;

namespace AfrikanEchoes.ViewModels.Publishers
{
    public class PublisherEditViewModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Publisher Name")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
