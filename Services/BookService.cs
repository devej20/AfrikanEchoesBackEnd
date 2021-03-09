using AfrikanEchoes.Entities;
using AfrikanEchoes.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfrikanEchoes.Services
{
    public interface IBookService
    {
        IEnumerable<BookModel> GetAll();
        IEnumerable<BookModel> GetByCategory(long categoryId);
        IEnumerable<BookModel> GetBookByFeautured();
        IEnumerable<BookModel> GetBookByNew();
        IEnumerable<BookModel> GetBookByTopChart();
        Book GetById(long id);

    }

    public class BookService : IBookService
    {
        private AfrikanEchoesDbContext _context;

        public BookService(AfrikanEchoesDbContext context)
        {
            _context = context;
        }
        public IEnumerable<BookModel> GetAll()
        {

            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Narrator)
                .Include(b => b.Publisher)
                .Include(b => b.Category)
                .Include(b => b.Language)
                .Include(b => b.Audio)
                .Include(b => b.BookImages);

            return BooksModel(books);
        }

        public IEnumerable<BookModel> GetByCategory(long categoryId)
        {

            var books = _context.Books.Where(x => x.CategoryId == categoryId)
                .Include(b => b.Author)
                .Include(b => b.Narrator)
                .Include(b => b.Publisher)
                .Include(b => b.Category)
                .Include(b => b.Language)
                .Include(b => b.Audio)
                .Include(b => b.BookImages);

            return BooksModel(books);

        }

        public IEnumerable<BookModel> GetBookByFeautured()
        {

            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Narrator)
                .Include(b => b.Publisher)
                .Include(b => b.Category)
                .Include(b => b.Language)
                .Include(b => b.Audio)
                .Include(b => b.BookImages)
                .OrderByDescending(b => b.Author)
                .Take(15);

            return BooksModel(books);

        }

        public IEnumerable<BookModel> GetBookByNew()
        {

            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Narrator)
                .Include(b => b.Publisher)
                .Include(b => b.Category)
                .Include(b => b.Language)
                .Include(b => b.Audio)
                .Include(b => b.BookImages)
                .OrderByDescending(b => b.CreatedAt)
                .Take(15);

            return BooksModel(books);

        }

        public IEnumerable<BookModel> GetBookByTopChart()
        {

            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Narrator)
                .Include(b => b.Publisher)
                .Include(b => b.Category)
                .Include(b => b.Language)
                .Include(b => b.Audio)
                .Include(b => b.BookImages)
                .OrderByDescending(b => b.Publisher)
                .Take(15);

            return BooksModel(books);

        }

        private IEnumerable<BookModel> BooksModel(IQueryable<Book> books)
        {
            ICollection<BookModel> list = new List<BookModel>();

            //string baseUrl = "https://localhost:44366";
            string baseUrl = "http://173.248.135.167/africanechoes";
            string imageUrl = baseUrl + "/uploads/images/";
            string audioUrl = baseUrl + "/uploads/audios/";

            foreach (Book book in books)
            {

                //if (true)
                //{

                //}

                BookModel model = new BookModel();

                model.Id = book.Id;
                if (book.CoverImagePath != null)
                {
                    model.CoverImage = imageUrl + book.CoverImagePath;
                }
                else
                {
                    model.CoverImage = imageUrl + "african_echoes_logo.png";
                }
                model.Title = book.Title;
                model.Description = book.Description ?? "";
                model.ReleaseDate = book.ReleaseDate ?? null;
                model.Author = book.Author.FirstName ?? "" + book.Author.LastName ?? "";
                model.Narrator = book.Narrator.LastName ?? "" + book.Narrator.LastName ?? "";
                model.Publisher = book.Publisher.Name ?? "";
                model.Language = book.Language.Name ?? "";
                model.Category = book.Category.Name ?? "";
                model.AudioName = book.Audio.Name ?? "";
                model.AudioFileName = book.Audio.FilePath ?? "";
                model.AudioSize = book.Audio.FileSize ?? 0;
                model.AudioDuration = book.Audio.Duration ?? null;
                model.AudioUrl = audioUrl + book.Audio.FilePath ?? "";



                list.Add(model);
            }

            return list;
        }



        public Book GetById(long id)
        {
            return _context.Books.Find(id);
        }


    }
}
