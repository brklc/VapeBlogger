using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VapeBlogger.Data;

namespace VapeBlogger.Models
{
    public class PostViewComponent: ViewComponent
    {
        public readonly ApplicationDbContext _context;
        public PostViewComponent(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int CatId)
        {
           var ca =await _context.Posts.Include(i => i.Category)
                .Where(c => c.CategoryId == CatId).ToListAsync();

            return View(ca);
        }
    }
}
