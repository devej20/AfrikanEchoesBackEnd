using AfrikanEchoes.Models.BookImages;
using System;
using System.Collections.Generic;

namespace AfrikanEchoes.Models.Books
{
    public class BookModel
    {
        public long Id { get; set; }
        public string CoverImage { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Author { get; set; }
        public string Narrator { get; set; }
        public string Category { get; set; }
        public string Publisher { get; set; }
        
        public string Language { get; set; }
        public string AudioName { get; set; }
        public string AudioFileName { get; set; }
        public decimal? AudioSize { get; set; }
        public TimeSpan? AudioDuration { get; set; }
        public string AudioUrl { get; set; }
        public ICollection<BookImageModel> Images { get; set; }

    }
}
