using System;
using System.Collections.Generic;

#nullable disable

namespace AfrikanEchoes.Entities
{
    public partial class Author
    {
        public Author()
        {
            AuthorContacts = new HashSet<AuthorContact>();
            BookAuthors = new HashSet<BookAuthor>();
            Books = new HashSet<Book>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<AuthorContact> AuthorContacts { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
