﻿using System;
using System.Collections.Generic;

#nullable disable

namespace AfrikanEchoes.Entities
{
    public partial class Category
    {
        public Category()
        {
            Books = new HashSet<Book>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string CoverImagePath { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
