using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VapeBlogger.Models
{
    public class LoginViewModel
    {
        [EmailAddress]
        [StringLength(200)]
        [Required]
        public string UserName { get; set; }

        [StringLength(256)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
