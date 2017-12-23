using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VapeBlogger.Data;

namespace VapeBlogger.Components
{
    public class FooterComponent : ViewComponent
    {
        public readonly ApplicationDbContext _context;
        public FooterComponent(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var a = await _context.Posts.Where(c => c.PublishDate.Day == DateTime.Now.Day).ToListAsync();
        
            return View(a);
        }
    }
}
