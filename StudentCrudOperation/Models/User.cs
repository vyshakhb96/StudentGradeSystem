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
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        public string UserName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "E-mail is required.")]
        //[RegularExpression("[a-zA-Z ]{3,15}", ErrorMessage = "Enter proper e-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("[a-zA-Z ]{3,15}", ErrorMessage = "Enter proper password")]
        public string Password { get; set; }
        public int Active { get; set; }
        public string Reqtype { get; set; }
        public Operations ActionType { get; set; }

    }
}