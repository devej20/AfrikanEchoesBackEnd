using AfrikanEchoes.Entities;
using AfrikanEchoes.ViewModels.AudioFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AfrikanEchoes.Controllers
{
    [Authorize]
    public class AudioFilesController : Controller
    {
        private readonly AfrikanEchoesDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public AudioFilesController(AfrikanEchoesDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: AudioFiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.AudioFiles.ToListAsync());
        }

        // GET: AudioFiles/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audioFile = await _context.AudioFiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (audioFile == null)
            {
                return NotFound();
            }

            return View(audioFile);
        }

        // GET: AudioFiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AudioFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Size,File")] AudioFileCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                string result = null;
                long Size = 0;



                if (model.File != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads/audios");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = model.File.FileName;
                    result = Path.GetFileName(fileName);
                    long fileSize = model.File.Length;
                    Size = fileSize / 1000000;

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + result;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.File.CopyTo(fileStream);
                    }



                }

                //string file = Path.Combine(hostingEnvironment.WebRootPath, "uploads/audios", uniqueFileName);

                //var inputFile = new MediaFile { Filename = file };

                //using (var engine = new Engine())
                //{
                //    engine.GetMetadata(inputFile);
                //}

                //Console.WriteLine(inputFile.Metadata.Duration);

                //WaveFileReader wf = new WaveFileReader(filePath);
                //TimeSpan duration = inputFile.Metadata.Duration;

                AudioFile audioFile = new AudioFile
                {
                    Name = result,
                    FileSize = Size,
                    FilePath = uniqueFileName,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,

                };

                _context.Add(audioFile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AudioFiles/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audioFile = await _context.AudioFiles.FindAsync(id);
            if (audioFile == null)
            {
                return NotFound();
            }
            return View(audioFile);
        }

        // POST: AudioFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,FileSize,FilePath")] AudioFile audioFile)
        {
            if (id != audioFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(audioFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AudioFileExists(audioFile.Id))
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
            return View(audioFile);
        }

        // GET: AudioFiles/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audioFile = await _context.AudioFiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (audioFile == null)
            {
                return NotFound();
            }

            return View(audioFile);
        }

        // POST: AudioFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var audioFile = await _context.AudioFiles.FindAsync(id);

            string audioPath = Path.Combine(hostingEnvironment.WebRootPath, "uploads/audios", audioFile.FilePath);

            if ((System.IO.File.Exists(audioPath)))
            {
                System.IO.File.Delete(audioPath);
            }

            _context.AudioFiles.Remove(audioFile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AudioFileExists(long id)
        {
            return _context.AudioFiles.Any(e => e.Id == id);
        }
    }
}
