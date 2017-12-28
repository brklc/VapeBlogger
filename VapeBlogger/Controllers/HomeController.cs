using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VapeBlogger.Models;
using VapeBlogger.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using ReflectionIT.Mvc.Paging;

namespace VapeBlogger.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context;
        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public override void OnActionExecuting(ActionExecutingContext filerContext)
        {
            ViewBag.Categories = context.Categories.Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name, Count = c.Posts.Count }).ToList();
            
            if (filerContext.RouteData.Values["id"] != null)
            {
                ViewBag.ActiveCategory = context.Categories.FirstOrDefault(c => c.Id.ToString() == filerContext.RouteData.Values["id"].ToString());
            }
            base.OnActionExecuting(filerContext);
        }
        public async Task<IActionResult> Index(int? id, int page =1)
        {
            
            var post = context.Posts.Include(i => i.Category)
                .Where(n => (id != null ? n.CategoryId == id : true) && n.IsPublished == true).AsNoTracking()
                .OrderByDescending(o => o.PublishDate);
            var model = await PagingList<Post>.CreateAsync(post, 9, page);
            return View(model);
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
