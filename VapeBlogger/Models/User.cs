using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VapeBlogger.Models
{
    public class User
    {
        public int Id { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }


        [StringLength(256)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [NotMapped]
        [StringLength(256)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required]
        public string ConfirmPassword { get; set; }

        public DateTime CreateDate { get; set; }

        [StringLength(200)]
        public string CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        [StringLength(200)]
        public string UpdatedBy { get; set; }
    }
}
