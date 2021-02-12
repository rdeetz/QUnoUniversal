// <copyright file="ColorToBrushConverter.cs" company="Mooville">
//   Copyright © 2020 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal.ViewModel
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;
    using Mooville.QUno.Model;

    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var brushKey = String.Empty;

            if (value != null)
            {
                Color color = (Color)value;

                switch (color)
                {
                    case Color.Red:
                        brushKey = "CardRedBrush";
                        break;
                    case Color.Blue:
                        brushKey = "CardBlueBrush";
                        break;
                    case Color.Yellow:
                        brushKey = "CardYellowBrush";
                        break;
                    case Color.Green:
                        brushKey = "CardGreenBrush";
                        break;
                    case Color.Wild:
                        brushKey = "CardBlackBrush";
                        break;
                }
            }
            else
            {
                brushKey = "CardTransparentBrush";
            }

            Brush brush = Application.Current.Resources[brushKey] as Brush;

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
