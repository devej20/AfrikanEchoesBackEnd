using System;
using System.Collections.Generic;

#nullable disable

namespace AfrikanEchoes.Entities
{
    public partial class FeaturedBook
    {
        public int Id { get; set; }
        public long BookId { get; set; }
        public string Title { get; set; }
    }
}
