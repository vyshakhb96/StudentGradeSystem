using StudentLibraries.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentCrudOperation.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        public string Dob { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }
        public string Role { get; set; }
        public bool IsAdmin { get; set; }
        public string Reqtype { get; set; }
        public Operations ActionType { get; set; }

    }
}