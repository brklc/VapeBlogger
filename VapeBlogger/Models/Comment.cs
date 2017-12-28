using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VapeBlogger.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Article { get; set; }

        public DateTime CreateDate { get; set; }

        [StringLength(200)]
        public string CreatedBy { get; set; }

        public bool IsPublished { get; set; }

        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }


    }
}
