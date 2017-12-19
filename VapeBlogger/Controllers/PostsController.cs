﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VapeBlogger.Data;
using Microsoft.EntityFrameworkCore;

namespace VapeBlogger.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext context;
        public PostsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int? id)
        {
            var post = context.Posts
                .Include(p => p.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}