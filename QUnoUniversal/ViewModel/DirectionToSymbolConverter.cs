// <copyright file="DirectionToSymbolConverter.cs" company="Mooville">
//   Copyright © 2020 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal.ViewModel
{
    using System;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Data;
    using Mooville.QUno.Model;

    public class DirectionToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var symbol = Symbol.Refresh;

            Direction d = (Direction)value;

            switch (d)
            {
                case Direction.Clockwise:
                    symbol = Symbol.Forward;
                    break;
                case Direction.Counterclockwise:
                    symbol = Symbol.Back;
                    break;
                default:
                    break;
            }

            return symbol;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
