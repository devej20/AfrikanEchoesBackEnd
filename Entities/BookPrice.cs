using System;
using System.Collections.Generic;

#nullable disable

namespace AfrikanEchoes.Entities
{
    public partial class BookPrice
    {
        public long Id { get; set; }
        public long? BookId { get; set; }
        public decimal? BookPrice1 { get; set; }
        public DateTime? PriceDate { get; set; }
    }
}
