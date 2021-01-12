// <copyright file="WildColorDialog.xaml.cs" company="Mooville">
//   Copyright © 2020 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal
{
    using System;
    using Windows.ApplicationModel;
    using Windows.UI.Xaml.Controls;
    using Mooville.QUno.Model;
    using Mooville.QUno.ViewModel;

    public sealed partial class WildColorDialog : ContentDialog
    {
        private WildColorViewModel viewModel;

        public WildColorDialog()
        {
            this.InitializeComponent();

            if (!DesignMode.DesignModeEnabled)
            {
                this.viewModel = new WildColorViewModel();
                this.DataContext = this.viewModel;

                this.PrimaryButtonClick += this.ContentDialog_PrimaryButtonClick;
                this.SecondaryButtonClick += this.ContentDialog_SecondaryButtonClick;
                this.listWildColors.SelectionChanged += this.ListWildColors_SelectionChanged;
                this.IsPrimaryButtonEnabled = false;
            }
        }

        public WildColorViewModel ViewModel
        {
            get
            {
                return this.viewModel;
            }
        }

        // WildColorViewModel has a SelectedColor property of type Color? but 
        // that property cannot be directly used in XAML since x:Bind is 
        // strongly typed and expects a property of type object.
        public object SelectedColor
        {
            get
            {
                return this.viewModel.SelectedColor;
            }

            set
            {

                this.viewModel.SelectedColor = value as Color?;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            return;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            return;
        }

        private void ListWildColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.IsPrimaryButtonEnabled = (listWildColors.SelectedItem != null);

            return;
        }
    }
}
