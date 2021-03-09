using AfrikanEchoes.Entities;
using AfrikanEchoes.ViewModels.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AfrikanEchoes.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly AfrikanEchoesDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public BooksController(AfrikanEchoesDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var africanEchoesDBContext = _context.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Narrator).Include(b => b.Publisher);
            return View(await africanEchoesDBContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Narrator)
                .Include(b => b.Publisher)

                .Include(b => b.Language)
                .Include(b => b.Audio)
                .Include(b => b.BookImages)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            var authors = _context.Authors.Select(s =>
            new
            {
                Id = s.Id,
                FullName = string.Format("{0} {1}", s.FirstName, s.LastName)
            }
            );
            var narrators = _context.Narrators.Select(s =>
            new
            {
                Id = s.Id,
                FullName = string.Format("{0} {1}", s.FirstName, s.LastName)
            }
            );

            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["NarratorId"] = new SelectList(narrators, "Id", "FullName");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");

            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name");
            ViewData["AudioId"] = new SelectList(_context.AudioFiles, "Id", "Name");

            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ReleaseDate,AuthorId,NarratorId,CategoryId,PublisherId,LanguageId,AudioId,CoverImage,Images,Price")] BookCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueCoverImageFileName = ProcessUploadImage(model.CoverImage);

                string uniqueImageFileName = null;
                ICollection<BookImage> bookImages = null;
                if (model.Images != null && model.Images.Count > 0)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads/images");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    bookImages = new List<BookImage>();

                    foreach (IFormFile image in model.Images)
                    {
                        BookImage images = new BookImage();

                        string fileName = image.FileName;
                        string result = Path.GetFileName(fileName);

                        uniqueImageFileName = Guid.NewGuid().ToString() + "_" + result;
                        string filePath = Path.Combine(uploadsFolder, uniqueImageFileName);
                        image.CopyTo(new FileStream(filePath, FileMode.Create));

                        images.Name = result;
                        images.ImagePath = uniqueImageFileName;
                        bookImages.Add(images);
                    }

                }


                Book book = new Book
                {
                    Title = model.Title,
                    Description = model.Description,
                    ReleaseDate = model.ReleaseDate,
                    AuthorId = model.AuthorId,
                    NarratorId = model.NarratorId,
                    CategoryId = model.CategoryId,
                    PublisherId = model.PublisherId,
                    Price = model.Price,
                    LanguageId = model.LanguageId,
                    AudioId = model.AudioId,
                    CoverImagePath = uniqueCoverImageFileName,
                    BookImages = bookImages,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                };

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            var authors = _context.Authors.Select(s =>
           new
           {
               Id = s.Id,
               FullName = string.Format("{0} {1}", s.FirstName, s.LastName)
           }
           );
            var narrators = _context.Narrators.Select(s =>
            new
            {
                Id = s.Id,
                FullName = string.Format("{0} {1}", s.FirstName, s.LastName)
            }
            );

            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName", model.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
            ViewData["NarratorId"] = new SelectList(narrators, "Id", "FullName", model.NarratorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", model.Price);

            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name", model.LanguageId);
            ViewData["AudioId"] = new SelectList(_context.AudioFiles, "Id", "Name", model.AudioId);

            return View(model);
        }

        private string ProcessUploadImage(IFormFile CoverImage)
        {
            string uniqueCoverImageFileName = null;
            if (CoverImage != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads/images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }


                string fileName = CoverImage.FileName;
                string result = Path.GetFileName(fileName);

                uniqueCoverImageFileName = Guid.NewGuid().ToString() + "_" + result;
                string filePath = Path.Combine(uploadsFolder, uniqueCoverImageFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    CoverImage.CopyTo(fileStream);
                }



            }

            return uniqueCoverImageFileName;
        }


        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            BookEditViewModel viewModel = new BookEditViewModel()
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                ReleaseDate = book.ReleaseDate,
                AuthorId = book.AuthorId,
                NarratorId = book.NarratorId,
                CategoryId = book.CategoryId,
                PublisherId = book.PublisherId,
                Price = book.Price,
                LanguageId = book.LanguageId,
                AudioId = book.AudioId,
                ExistingCoverImage = book.CoverImagePath,
            };


            var authors = _context.Authors.Select(s =>
           new
           {
               Id = s.Id,
               FullName = string.Format("{0} {1}", s.FirstName, s.LastName)
           }
           );
            var narrators = _context.Narrators.Select(s =>
            new
            {
                Id = s.Id,
                FullName = string.Format("{0} {1}", s.FirstName, s.LastName)
            }
            );

            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            ViewData["NarratorId"] = new SelectList(narrators, "Id", "FullName", book.NarratorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);

            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name", book.LanguageId);
            ViewData["AudioId"] = new SelectList(_context.AudioFiles, "Id", "Name", book.AudioId);

            return View(viewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,Description,ReleaseDate,AuthorId,NarratorId,CategoryId,PublisherId,LanguageId,AudioId,CoverImage,Images,ExistingCoverImage,Price")] BookEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Book book = await _context.Books.FindAsync(id);

                    book.Title = viewModel.Title;
                    book.Description = viewModel.Description;
                    book.ReleaseDate = viewModel.ReleaseDate;
                    book.AuthorId = viewModel.AuthorId;
                    book.NarratorId = viewModel.NarratorId;
                    book.CategoryId = viewModel.CategoryId;
                    book.PublisherId = viewModel.PublisherId;
                    book.Price = viewModel.Price;
                    book.LanguageId = viewModel.LanguageId;
                    book.AudioId = viewModel.AudioId;
                    book.ModifiedAt = DateTime.Now;

                    if (viewModel.CoverImage != null)
                    {
                        if (viewModel.ExistingCoverImage != null)
                        {
                            string imagePath = Path.Combine(hostingEnvironment.WebRootPath, "uploads/images", viewModel.ExistingCoverImage);

                            if ((System.IO.File.Exists(imagePath)))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }

                        book.CoverImagePath = ProcessUploadImage(viewModel.CoverImage);
                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var authors = _context.Authors.Select(s =>
          new
          {
              Id = s.Id,
              FullName = string.Format("{0} {1}", s.FirstName, s.LastName)
          }
          );
            var narrators = _context.Narrators.Select(s =>
            new
            {
                Id = s.Id,
                FullName = string.Format("{0} {1}", s.FirstName, s.LastName)
            }
            );

            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName", viewModel.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", viewModel.CategoryId);
            ViewData["NarratorId"] = new SelectList(narrators, "Id", "FullName", viewModel.NarratorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", viewModel.PublisherId);

            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name", viewModel.LanguageId);
            ViewData["AudioId"] = new SelectList(_context.AudioFiles, "Id", "Name", viewModel.AudioId);
            return View(viewModel);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Narrator)
                .Include(b => b.Publisher)

                .Include(b => b.Language)
                .Include(b => b.Audio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book.CoverImagePath != null)
            {
                string imagePath = Path.Combine(hostingEnvironment.WebRootPath, "uploads/images", book.CoverImagePath);

                if ((System.IO.File.Exists(imagePath)))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(long id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
