using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Views.Validator
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class Validator : ValidationAttribute
    {
        public string[] list { get; set; }
        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {            
            if(!list.Contains(value.ToString()))
            {
                return new ValidationResult("Value is not in the list", new[] { validationContext.MemberName });
            }            
                return null;
        }
    }

    
}
