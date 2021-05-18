using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Virtual_School_Register.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        public string OtherProperty { get; set; }
        public string OtherPropertyDisplayName { get; set; }
        public object OtherPropertyValue { get; set; }
        public bool isInverted { get; set; }
        public bool IsEmpty { get; set; }


        public override bool RequiresValidationContext => true;

        public RequiredIfAttribute(string otherProperty, object otherPropertyValue, string errorMessage) : base(errorMessage)
        {
            this.OtherProperty = otherProperty;
            this.OtherPropertyValue = otherPropertyValue;
            this.isInverted = false;
            this.IsEmpty = IsEmpty;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException("validationContext");
            }

            PropertyInfo otherProperty = validationContext.ObjectType.GetProperty(this.OtherProperty);

            if (OtherProperty == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            object otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            if (!this.isInverted && Equals(otherValue, this.OtherPropertyValue) ||
                this.isInverted && !Equals(otherValue, this.OtherPropertyValue))
            {
                if (value == null)
                {
                    return new ValidationResult(ErrorMessage);
                }

                string val = value as string;

                if (val != null && val.Trim().Length == 0)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
