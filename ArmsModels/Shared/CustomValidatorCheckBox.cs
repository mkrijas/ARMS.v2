using System.ComponentModel.DataAnnotations;

namespace ArmsModels.Shared
{
    public class CustomValidatorCheckBox : ValidationAttribute
    {
        public CustomValidatorCheckBox(bool errorMessage) 
        {
            message= errorMessage;
        }
        public bool message { get; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                return new ValidationResult("Date Filed Is requird");
            }
            else
            {
             
                if (message== true)
                {
                    return new ValidationResult("Date field is required");
                }
                return base.IsValid(value, validationContext);
            }
    
        }
    }
}
