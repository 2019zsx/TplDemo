using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TplDemo.Common.Validation
{
    /// <summary>自定特性验证</summary>
    public class ClassicMovieAttribute : ValidationAttribute
    {
        private int _year;

        public ClassicMovieAttribute(int Year)
        {
            _year = Year;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //  Movies movie = (Movies)validationContext.ObjectInstance;
            //if (movie.ReleaseDate.Year > _year)
            //{
            //    return new ValidationResult("发布年份不能大于" + _year);
            //}
            return ValidationResult.Success;
        }
    }
}