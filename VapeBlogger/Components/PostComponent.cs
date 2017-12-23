using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VapeBlogger.Data;

namespace VapeBlogger.Models
{
    public class PostComponent: ViewComponent
    {
        public readonly ApplicationDbContext _context;
        public PostComponent(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int catId)
        {
          var a = await _context.Posts.Include(i => i.Category)
                .Where(c => c.CategoryId == catId).ToListAsync();
            ViewBag.a = a;
            return View(a);
        }
    }
}
