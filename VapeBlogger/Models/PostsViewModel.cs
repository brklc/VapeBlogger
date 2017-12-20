using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VapeBlogger.Models
{
    public class PostsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreateDate { get; set; }
        public string Photo { get; set; }
    }
}
