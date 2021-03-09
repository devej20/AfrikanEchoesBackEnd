using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AfrikanEchoes.ViewModels.AudioFiles
{
    public class AudioFileEditViewModel
    {
        public string Name { get; set; }
        public int? Size { get; set; }

        [Required]
        [Display(Name = "Audio File")]
        public IFormFile File { get; set; }

        public TimeSpan? Duration { get; set; }

    }
}
