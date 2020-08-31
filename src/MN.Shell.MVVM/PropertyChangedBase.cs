using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Base class for all kind of ViewModels
    /// </summary>
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event raised to notify observers that a property has new value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises PropertyChanged event with supplied property name
        /// </summary>
        /// <param name="propertyName">Name of property (set automatically by compiler)</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Shortcut for setting property value and raising PropertyChanged in single call
        /// </summary>
        /// <typeparam name="T">Property type</typeparam>
        /// <param name="storage">Backing field passed by reference</param>
        /// <param name="value">New value of property</param>
        /// <param name="propertyName">Name of property (set automatically by compiler)</param>
        protected void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises PropertyChanged event with empty property name, effectively forcing refreshing all properties
        /// </summary>
        protected void Refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
