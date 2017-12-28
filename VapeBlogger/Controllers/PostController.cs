using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VapeBlogger.Data;
using Microsoft.EntityFrameworkCore;
using VapeBlogger.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VapeBlogger.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext context;
        public PostController(ApplicationDbContext context)
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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int? id)
        {
            ViewBag.Comment = context.Comments.Select(co => new CommentViewModel { Id = co.Id, Article = co.Article, CreateDate = co.CreateDate, FullName = co.FullName }).ToList();
            ViewBag.Posts = context.Posts.Select(c => new PostsViewModel { Id = c.Id, Photo = c.Photo, Title = c.Title, CreateDate = c.CreateDate, Hits = c.Hits }).OrderByDescending(c => c.Hits).Take(5).ToList();
            var post = context.Posts.Include(i => i.Category)
                .Where(p => p.Id == id)
                .FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }
            post.Hits +=1;
            context.SaveChanges();
           

            return View(post);
        }
        
     
        
        public JsonResult SendComment( string fullName, string article,int postid)
        {
            
            Comment c = new Comment();
            c.PostId = postid;
            c.FullName = fullName;
            c.Article = article;
            c.CreateDate = DateTime.Now;
            c.CreatedBy = fullName;
            c.IsPublished = false;
           
                context.Add(c);
                context.SaveChanges();
          
           

            return Json(RedirectToAction("Home"));
        }
       
    }
}