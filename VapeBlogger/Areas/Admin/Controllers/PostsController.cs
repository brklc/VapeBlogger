using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VapeBlogger.Data;
using VapeBlogger.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace VapeBlogger.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public PostsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/Posts
        public async Task<IActionResult> Index()
        {
            Security.LoginCheck(HttpContext);
            var applicationDbContext = _context.Posts.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Security.LoginCheck(HttpContext);
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Admin/Posts/Create
        public IActionResult Create()
        {
            Security.LoginCheck(HttpContext);
            var post = new Post();
            post.CreateDate = DateTime.Now;
            post.CreatedBy = User.Identity.Name;
            post.UpdateDate = DateTime.Now;
            post.UpdatedBy = User.Identity.Name;
            post.PublishDate = DateTime.Now;

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View(post);
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Photo,PublishDate,IsPublished,CreateDate,CreatedBy,UpdateDate,UpdatedBy,CategoryId")] Post post, IFormFile upload)
        {
            Security.LoginCheck(HttpContext);
            // dosya uzantısı için geçerlilik denetimi
            if (upload != null && !IsExtensionValid(upload))
            {
                ModelState.AddModelError("Photo", "Dosya uzantısı .jpg, .jpeg, .gif veya .png olmalıdır.");
            }

            if (ModelState.IsValid)
            {
                post.CreateDate = DateTime.Now;
                post.CreatedBy = User.Identity.Name;
                post.UpdateDate = DateTime.Now;
                post.UpdatedBy = User.Identity.Name;
                // dosya yüklemesi
                string fileName = await UploadFileAsync(upload);
                if (fileName != null)
                {
                    post.Photo = fileName;
                }
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", post.CategoryId);
            return View(post);
        }

        // GET: Admin/Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Security.LoginCheck(HttpContext);
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", post.CategoryId);
            return View(post);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Photo,PublishDate,IsPublished,CreateDate,CreatedBy,UpdateDate,UpdatedBy,CategoryId")] Post post,IFormFile upload)
        {
            Security.LoginCheck(HttpContext);
            if (id != post.Id)
            {
                return NotFound();
            }
            // dosya uzantısı için geçerlilik denetimi
            if (upload != null && !IsExtensionValid(upload))
            {
                ModelState.AddModelError("Photo", "Dosya uzantısı .jpg, .jpeg, .gif veya .png olmalıdır.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    post.UpdateDate = DateTime.Now;
                    post.UpdatedBy = User.Identity.Name;
                    // dosya yüklemesi
                    string fileName = await UploadFileAsync(upload);
                    if (fileName != null)
                    {
                        post.Photo = fileName;
                    }
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", post.CategoryId);
            return View(post);
        }

        // GET: Admin/Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Security.LoginCheck(HttpContext);
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Security.LoginCheck(HttpContext);
            var post = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
        // upload edilecek dosyanın uzantısı geçerli mi?
        private bool IsExtensionValid(IFormFile upload)
        {
            if (upload != null)
            {
                var allowedExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png" };
                var extension = Path.GetExtension(upload.FileName).ToLowerInvariant();
                return allowedExtensions.Contains(extension);
            }
            return false;
        }
        private async Task<string> UploadFileAsync(IFormFile upload)
        {
            if (upload != null && upload.Length > 0 && IsExtensionValid(upload))
            {
                var fileName = upload.FileName;
                var extension = Path.GetExtension(fileName);
                // sitenin içinde uploads dizinine yüklenecek
                var uploadLocation = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

                // eğer uploads dizini yoksa oluştur
                if (!Directory.Exists(uploadLocation))
                {
                    Directory.CreateDirectory(uploadLocation);
                }

                uploadLocation += "/" + fileName;

                using (var stream = new FileStream(uploadLocation, FileMode.Create))
                {
                    await upload.CopyToAsync(stream);
                }
                return fileName;
            }
            return null;
        }
    }
}
