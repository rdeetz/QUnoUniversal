// <copyright file="ViewModelBase.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.ViewModel
{
    using System.ComponentModel;

    /// <summary>
    /// Represents the base class for all view models.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Called when a property changes.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that has changed.
        /// </param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler tmp = this.PropertyChanged;

            if (tmp != null)
            {
                PropertyChangedEventArgs eventArgs = new PropertyChangedEventArgs(propertyName);
                tmp(this, eventArgs);
            }

            return;
        }
    }
}
