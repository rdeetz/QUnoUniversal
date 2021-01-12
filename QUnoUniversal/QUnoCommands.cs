// <copyright file="QUnoCommands.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal
{
    using System.Windows.Input;
    using Windows.UI.Xaml.Input;

    public static class QUnoCommands
    {
        private static readonly ICommand open = new StandardUICommand(StandardUICommandKind.Open);

        public static ICommand Open
        {
            get
            {
                return QUnoCommands.open;
            }
        }
    }
}
