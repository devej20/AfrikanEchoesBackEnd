namespace AfrikanEchoes.Models.BookImages
{
    public class BookImageModel
    {
        public long Id { get; set; }
        public long? BookId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string BookTitle { get; set; }


    }
}
