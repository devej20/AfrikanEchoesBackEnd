using System;
using System.Collections.Generic;

#nullable disable

namespace AfrikanEchoes.Entities
{
    public partial class BookImage
    {
        public long Id { get; set; }
        public long? BookId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }

        public virtual Book Book { get; set; }
    }
}
