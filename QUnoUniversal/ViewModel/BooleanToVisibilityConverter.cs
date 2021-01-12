// <copyright file="BooleanToVisibilityConverter.cs" company="Mooville">
//   Copyright © 2020 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal.ViewModel
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility visibility = Visibility.Collapsed;

            if ((value is bool) && (bool)value)
            {
                visibility = Visibility.Visible;

                if ((parameter is string) && ((string)parameter).Equals("Reverse"))
                {
                    visibility = Visibility.Collapsed;
                }
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool visible = false;

            if ((value is Visibility) && ((Visibility)value == Visibility.Visible))
            {
                visible = true;

                if ((parameter is string) && ((string)parameter).Equals("Reverse"))
                {
                    visible = false;
                }
            }

            return visible;
        }
    }
}
