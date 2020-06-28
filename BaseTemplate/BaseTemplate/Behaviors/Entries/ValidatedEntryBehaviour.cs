using System;
using Xamarin.Forms;

namespace BaseTemplate.Behaviors.Entries
{
    public class ValidatedEntryBehaviour : BehaviorBase<Entry>
    {
        #region StaticFields
        public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ValidatedEntryBehaviour), true, BindingMode.Default, null, (bindable, oldValue, newValue) => OnIsValidChanged(bindable, newValue));
        #endregion
        #region Properties
        public bool IsValid
        {
            get
            {
                return (bool)GetValue(IsValidProperty);
            }
            set
            {
                SetValue(IsValidProperty, value);
            }
        }
        #endregion
        #region StaticMethods
        private static void OnIsValidChanged(BindableObject bindable, object newValue)
        {
            if (bindable is ValidatedEntryBehaviour IsValidBehavior && newValue is bool IsValid)
            {
                IsValidBehavior.AssociatedObject.TextColor = IsValid ? Color.Default : Color.Red;
            }
        }

        #endregion
    }
}
