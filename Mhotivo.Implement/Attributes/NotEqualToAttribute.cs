using System;
using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Implement.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NotEqualToAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Debe ingresar una contraseña diferente a la actual.";
        public string OtherProperty { get; private set; }

        public NotEqualToAttribute(string otherProperty) : base(DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(otherProperty))
            {
                throw new ArgumentNullException("otherProperty");
            }
            OtherProperty = otherProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherProperty);
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            var otherProperty = validationContext.ObjectInstance.GetType()
                .GetProperty(OtherProperty);
            var otherPropertyValue = otherProperty
                .GetValue(validationContext.ObjectInstance, null);
            if (value.Equals(otherPropertyValue))
            {
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}