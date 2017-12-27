using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VapeBlogger.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime CreateDate { get; set; }
        public string MyComment { get; set; }
        public bool IsPublished { get; set; }

        public ICollection<PostComment> PostComment { get; set; }
    }
}
