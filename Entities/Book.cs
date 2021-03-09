using System;
using System.Collections.Generic;

#nullable disable

namespace AfrikanEchoes.Entities
{
    public partial class Book
    {
        public Book()
        {
            BookAuthors = new HashSet<BookAuthor>();
            BookImages = new HashSet<BookImage>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public long? AuthorId { get; set; }
        public long? NarratorId { get; set; }
        public long? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public long? PublisherId { get; set; }
        public long? LanguageId { get; set; }
        public long? AudioId { get; set; }
        public string CoverImagePath { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? IsActive { get; set; }

        public virtual AudioFile Audio { get; set; }
        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
        public virtual Language Language { get; set; }
        public virtual Narrator Narrator { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        public virtual ICollection<BookImage> BookImages { get; set; }
    }
}
