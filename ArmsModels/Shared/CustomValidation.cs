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


}
