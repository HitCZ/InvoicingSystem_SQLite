using InvoicingSystem_SQLite.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InvoicingSystem_SQLite.Logic
{
    public class NotificationObject : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> backingDictionary = new Dictionary<string, object>();

        protected T GetPropertyValue<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));
            if (backingDictionary.TryGetValue(propertyName, out var value))
                return (T)value;
            return default;
        }

        protected bool SetPropertyValue<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));

            var previousValue = GetPropertyValue<T>(propertyName);

            if (typeof(T).IsValueType && EqualityComparer<T>.Default.Equals(newValue, previousValue)
                || ReferenceEquals(newValue, previousValue))
                return false;

            backingDictionary[propertyName] = newValue;
            OnPropertyChanged(propertyName);

            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged
    }
}
