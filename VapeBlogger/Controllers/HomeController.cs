using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VapeBlogger.Models;
using VapeBlogger.Data;
using Microsoft.EntityFrameworkCore;

namespace VapeBlogger.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context;
        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index(int? id)
        {

            var categories = context.Categories
               .OrderBy(o => o.Name)
               .Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name, Count = c.Posts.Count })
               .ToList();
            ViewBag.Categories = categories;

            var post = context.Posts.Include(i => i.Category)
                .Where(n => (id != null ? n.CategoryId == id : true) && n.IsPublished == true)
                .OrderByDescending(o => o.PublishDate).ToList();

            if (id != null)
            {
                ViewBag.ActiveCategory = context.Categories.FirstOrDefault(c => c.Id == id);
            }
            return View(post);
        }

        public IActionResult About()
        {
           
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
           
            ViewData["Message"] = "Your contact page.";

            return View();
        }

       

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
