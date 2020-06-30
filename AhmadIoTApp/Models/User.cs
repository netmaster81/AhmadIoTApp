using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AhmadIoTApp.Models
{
    public class User :IdentityUser
    {
        [Display(Name = "FirstName")]
        [StringLength(30)]
        public string FirstName { get; set; }


        [Display(Name = "LastName")]
        [StringLength(30)]
        public string LastName { get; set; }
    }
}
