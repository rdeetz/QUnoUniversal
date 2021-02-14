// <copyright file="PlayerTemplateSelector.cs" company="Mooville">
//   Copyright © 2020 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal.ViewModel
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class PlayerTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item)
        {
            var random = new Random();
            var index = random.Next(1, 5);
            var templateTemplate = @"PlayerItemTemplate{0}";
            var templateKey = String.Format(templateTemplate, index);

            var window = Window.Current;
            var frame = window.Content as Frame;
            var page = frame.Content as MainPage;
            var template = page.Resources[templateKey] as DataTemplate;

            return template;
        }
    }
}
