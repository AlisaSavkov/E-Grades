using System;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace Projekat.Filters
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class ValidateDateOfBirth : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateBirth = Convert.ToDateTime(value);
            //string dateBirthStr = dateBirth.ToString();
            //DateTime dt;
            //string[] formats = { "dd.MM.yyyy" };
            //if (DateTime.TryParseExact(dateBirthStr, formats, CultureInfo.InvariantCulture,
            //                          DateTimeStyles.None, out dt))
            //{
                int years = 0;
                DateTime today = DateTime.Now;
                 years = today.Year - dateBirth.Year;
                if (dateBirth.Month > today.Month || (dateBirth.Month == today.Month && dateBirth.Day > today.Day))
                {
                    years--;
                }
                if (years < 6 || years > 20)
                {
                    return false;
                }
                else
                {
                    return true;
                }
           
            
        }
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
            ErrorMessageString, name);
        }
    }
                    
        
    
    //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    //sealed public class AlwaysValidAttribute : ValidationAttribute
    //{
    //    public override bool IsValid(object value)
    //    {
    //        DateTime dateBirth = Convert.ToDateTime(value);

    //            int years = 0;

    //            DateTime today = DateTime.Now;

    //            years = today.Year - dateBirth.Year;
    //            if (dateBirth.Month > today.Month || (dateBirth.Month == today.Month && dateBirth.Day > today.Day))
    //            {
    //                years--;

    //            }

    //        }
    //        if (years <6)
    //        {
    //            return ValidationResult.Success;
    //        }
    //        else
    //        {
    //            return new ValidationResult
    //                ("Join date can not be greater than current date.");
    //        }
    //    }
    //    public override string FormatErrorMessage(string name)
    //    {
    //        return String.Format(CultureInfo.CurrentCulture,
    //        ErrorMessageString, name);
    //    }
    //}
}