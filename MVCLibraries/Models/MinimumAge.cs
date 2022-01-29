using System;
using System.ComponentModel.DataAnnotations;


namespace StudentLibraries
{
    public class MinimumAge :ValidationAttribute
    {
        int _minimumAge;

        public MinimumAge(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        public override bool IsValid(object value)
        {
            DateTime date;
            if(DateTime.TryParse(value.ToString(), out date))
            {
                return date.AddYears(_minimumAge) < DateTime.Now;
            }
            return false;
        }
    } 
}