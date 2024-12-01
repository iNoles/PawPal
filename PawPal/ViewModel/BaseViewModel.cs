using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PawPal.ViewModel
{
    /// <summary>
    /// A base class for ViewModels that implements INotifyPropertyChanged
    /// to support data binding and property change notifications.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event to notify when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event to notify the UI of changes.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. Defaults to the caller's name.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the value of a property and raises the PropertyChanged event if the value changes.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="backingStore">A reference to the field storing the property value.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="propertyName">The name of the property that is being set. Defaults to the caller's name.</param>
        /// <returns>True if the value was changed; otherwise, false.</returns>
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
