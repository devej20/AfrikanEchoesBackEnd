using AfrikanEchoes.Entities;
using AfrikanEchoes.Models.BookImages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfrikanEchoes.Services
{
    public interface IBookImagesService
    {
        IEnumerable<BookImageModel> GetAll();
        IEnumerable<BookImageModel> GetByBook(long bookId);
        Task<BookImage> GetById(long id);

    }

    public class BookImagesService : IBookImagesService
    {

        private AfrikanEchoesDbContext _context;

        public BookImagesService(AfrikanEchoesDbContext context)
        {
            _context = context;
        }
        public IEnumerable<BookImageModel> GetAll()
        {
            return _context.BookImages
                .Include(b => b.Book)
                .Select(s =>
                new BookImageModel
                {
                    Id = s.Id,
                    Image = "http://173.248.135.167/africanechoes/uploads/images/" + s.ImagePath,
                    Name = s.Name,
                    BookTitle = s.Book.Title,
                }
                );
        }

        public IEnumerable<BookImageModel> GetByBook(long bookId)
        {
            return _context.BookImages
               .Include(b => b.Book)
               .Where(x => x.BookId == bookId)
               .Select(s =>
               new BookImageModel
               {
                   Id = s.Id,
                   Image = "http://173.248.135.167/africanechoes/uploads/images/" + s.ImagePath,
                   Name = s.Name,
                   BookTitle = s.Book.Title,
               }
               );
        }

        public async Task<BookImage> GetById(long id)
        {
            return await _context.BookImages.FindAsync(id);
        }
    }
}
