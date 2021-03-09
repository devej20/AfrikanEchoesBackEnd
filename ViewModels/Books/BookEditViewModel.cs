using AfrikanEchoes.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AfrikanEchoes.ViewModels.Books
{
    public class BookEditViewModel
    {
        public long Id { get; set; }

        public string ExistingCoverImage { get; set; }

        [Required]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        [Display(Name = "Author")]
        public long? AuthorId { get; set; }

        [Required]
        [Display(Name = "Narrator")]
        public long? NarratorId { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        [Required]
        [Display(Name = "Category")]
        public long? CategoryId { get; set; }

        [Required]
        [Display(Name = "Publisher")]
        public long? PublisherId { get; set; }

        [Required]
        [Display(Name = "Language")]
        public long? LanguageId { get; set; }

        [Required]
        [Display(Name = "Audio")]
        public long? AudioId { get; set; }


        [Display(Name = "Cover Image")]
        public IFormFile CoverImage { get; set; }

        public List<IFormFile> Images { get; set; }

        public AudioFile Audio { get; set; }
    }
}
