namespace Mooville.QUno.Universal.ViewModel
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;
    using Mooville.QUno.Model;

    public class ValueToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var symbol = String.Empty;

            Value v = (Value)value;

            switch (v)
            {
                case Value.Wild:
                    symbol = "Wi";
                    break;
                case Value.WildDrawFour:
                    symbol = "Wi+4";
                    break;
                case Value.DrawTwo:
                    symbol = "+2";
                    break;
                case Value.Reverse:
                    symbol = "Rev";
                    break;
                case Value.Skip:
                    symbol = "Sk";
                    break;
                case Value.Zero:
                    symbol = "0";
                    break;
                case Value.One:
                    symbol = "1";
                    break;
                case Value.Two:
                    symbol = "2";
                    break;
                case Value.Three:
                    symbol = "3";
                    break;
                case Value.Four:
                    symbol = "4";
                    break;
                case Value.Five:
                    symbol = "0";
                    break;
                case Value.Six:
                    symbol = "6";
                    break;
                case Value.Seven:
                    symbol = "7";
                    break;
                case Value.Eight:
                    symbol = "8";
                    break;
                case Value.Nine:
                    symbol = "9";
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
