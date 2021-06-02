using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SpireHL.Core.Models
{
    public class BindableBaseWithValidation : BindableBase, INotifyDataErrorInfo
    {
        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
            || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            var result = base.SetProperty(ref storage, value, propertyName);

            if (result && !string.IsNullOrEmpty(propertyName))
            {
                ValidateProperty(propertyName);
            }
            return result;
        }

        private void ValidateProperty(string propertyName)
        {
            var propertyInfo = this.GetType().GetRuntimeProperty(propertyName);

            // no validation on this property
            if (propertyInfo == null)
            {
                return;
            }

            TryValidateProperty(propertyInfo);
        }

        private bool TryValidateProperty(PropertyInfo propertyInfo)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyInfo.Name };
            var propertyValue = propertyInfo.GetValue(this);

            // Validate the property
            bool isValid = Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
            {
                _validationErrors[propertyInfo.Name] = results.Select(c => c.ErrorMessage).ToList();
                RaiseErrorsChanged(propertyInfo.Name);
            }
            else if (_validationErrors.ContainsKey(propertyInfo.Name))
            {
                _validationErrors.Remove(propertyInfo.Name);
            }

            return isValid;
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public bool TryValidateProperties()
        {
            // Get all the properties decorated with the ValidationAttribute attribute.
            var propertiesToValidate = this.GetType()
                                           .GetRuntimeProperties()
                                           .Where(c => c.GetCustomAttributes(typeof(ValidationAttribute)).Any());

            foreach (PropertyInfo propertyInfo in propertiesToValidate)
            {
                TryValidateProperty(propertyInfo);
            }

            return this.HasErrors;
        }
    }
}
