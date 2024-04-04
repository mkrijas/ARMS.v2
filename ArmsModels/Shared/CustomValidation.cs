using System.ComponentModel.DataAnnotations;
using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;

namespace ArmsModels.BaseModels
{   

    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfTrueAttribute : RequiredAttribute
    {
        private string PropertyName { get; set; }

        public RequiredIfTrueAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();            

            bool.TryParse(type.GetProperty(PropertyName).GetValue(instance)?.ToString(), out bool propertyValue);

            if (propertyValue && string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfAttribute : RequiredAttribute
    {
        private string PropertyName { get; set; }
        private object ValueCondition { get; set; }
        public RequiredIfAttribute(string propertyName, object valueCondition)
        {
            PropertyName = propertyName;
            ValueCondition = valueCondition;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();            
            var proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (proprtyvalue != null && proprtyvalue.ToString() == ValueCondition.ToString() && value == null)
            {
                return new ValidationResult(ErrorMessage);
            } 
            return ValidationResult.Success;
        }
    }


    [AttributeUsage(AttributeTargets.Property)]
    public class MustContainAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count > 0;
            }
            return false;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AtLeastOneRequiredAttribute : ValidationAttribute
    {
        private readonly string _field1;
        private readonly string _field2;

        public AtLeastOneRequiredAttribute(string field1, string field2) => (_field1, _field2) = (field1, field2);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!TryGetProperty(_field1, validationContext, out var property1))
            {
                return new ValidationResult(string.Format("Unknown property: {0}", _field1), new[] { _field1 });
            }

            if (!TryGetProperty(_field2, validationContext, out var property2))
            {
                return new ValidationResult(string.Format("Unknown property: {0}", _field2), new[] { _field1 });
            }

            if (property1.GetValue(validationContext.ObjectInstance) != null ||
                property2.GetValue(validationContext.ObjectInstance) != null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.Format("Either or both of \"{0}\" and \"{1}\" are required", _field1, _field2), new[] { _field1, _field2 });
        }
        private bool TryGetProperty(string fieldName, ValidationContext validateionContext, out PropertyInfo propertyInfo)
        {
            return (propertyInfo = validateionContext.ObjectType.GetProperty(fieldName)) != null;
        }
    }

    public class ValidateAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public ValidateAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                var age = CalculateAge(dateOfBirth);
                if (age < _minimumAge)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            // Check if the birthday has occurred this year
            if (dateOfBirth.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }


    //////////////

    [AttributeUsage(AttributeTargets.Property)]
    public class NotlessAttribute : RequiredAttribute
    {
        private string _truckIdName, _eventTimeName;
        public NotlessAttribute(string truckIDName,string eventTimeName)
        {
            _truckIdName = truckIDName;
            _eventTimeName = eventTimeName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();
            if (type.GetProperty(_truckIdName) != null)
            {
                var truckID = type.GetProperty(_truckIdName).GetValue(instance, null);
                var eventTime = type.GetProperty(_eventTimeName).GetValue(instance, null);
                if (truckID != null && eventTime != null)
                {

                }
                if (value == null)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }


    //[AttributeUsage(AttributeTargets.Property)]
    //public class GstCodeAttribute : RequiredAttribute
    //{
    //    private string _GstNo;
    //    private string _PlaceID;
    //    public GstCodeAttribute(string GstNo, string PlaceID)
    //    {
    //        _GstNo = GstNo;
    //        _PlaceID = PlaceID;
    //    }

    //    protected override ValidationResult IsValid(object value, ValidationContext context)
    //    {
    //        object instance = context.ObjectInstance;
    //        Type type = instance.GetType();
    //        if (type.GetProperty(_GstNo) != null)
    //        {
    //            var GstNo = type.GetProperty(_GstNo).GetValue(instance, null);
    //            var PlaceID = type.GetProperty(_PlaceID).GetValue(instance, null);
    //            if (GstNo != null && PlaceID != null)
    //            {

    //            }
    //            if (value == null)
    //            {
    //                return new ValidationResult(ErrorMessage);
    //            }
    //        }
    //        return ValidationResult.Success;
    //    }
    //}
    //////////////
    ///
    public class NotFutureDateTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime tripDate = (DateTime)value;
                if (tripDate > DateTime.Now)
                {
                    return new ValidationResult("Trip date and time cannot be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
