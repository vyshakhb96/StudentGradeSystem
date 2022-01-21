using StudentLibraries.Enum;
using System.ComponentModel.DataAnnotations;

namespace StudentGradingSystem.Models
{
    public class Student
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Register number is required.")]
        public string Regnum { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression("[a-zA-Z]{3,15}", ErrorMessage = "Enter proper name")]
        public string Name { get; set; }

        [MinimumAge(17)]
        [Required(ErrorMessage = "Date of birth is required.")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        public string Dob { get; set; }

        [Required(ErrorMessage = "Standard is required.")]
        public string Standard { get; set; }

        [Required(ErrorMessage = "Mark is required.")]
        public int Mathematics { get; set; }

        [Required(ErrorMessage = "Mark is required.")]
        public int Physics { get; set; }

        [Required(ErrorMessage = "Mark is required.")]
        public int Chemistry { get; set; }

        public string Grade { get; set; }

        public int Active { get; set; }

        public string Reqtype { get; set; }

        public Operations ActionType { get; set; }
    }
}