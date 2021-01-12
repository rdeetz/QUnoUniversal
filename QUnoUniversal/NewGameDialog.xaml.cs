// <copyright file="NewGameDialog.xaml.cs" company="Mooville">
//   Copyright © 2020 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Resources;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Mooville.QUno.Model;
    using Mooville.QUno.Universal.ViewModel;
    using Mooville.QUno.ViewModel;

    public sealed partial class NewGameDialog : ContentDialog
    {
        private readonly UniversalNewGameViewModel viewModel;
        private readonly ISettingsProvider settingsProvider;
        private const int MaxNumberOfComputerPlayers = 9; // This constant should go in the base NewGameViewModel.

        public NewGameDialog()
        {
            this.InitializeComponent();

            this.settingsProvider = new UniversalSettingsProvider();

            if (!DesignMode.DesignModeEnabled)
            {
                this.settingsProvider.LoadSettings();

                var resourceLoader = ResourceLoader.GetForCurrentView();
                var playerNameTemplate = resourceLoader.GetString("PlayerNameTemplate");

                this.viewModel = new UniversalNewGameViewModel(this.settingsProvider.DefaultHumanPlayerName, this.settingsProvider.DefaultComputerPlayers, playerNameTemplate);
                this.DataContext = this.viewModel;

                this.PrimaryButtonClick += this.ContentDialog_PrimaryButtonClick;
                this.SecondaryButtonClick += this.ContentDialog_SecondaryButtonClick;
                this.buttonAdd.Click += this.ButtonAdd_Click;
                this.buttonRemove.Click += this.ButtonRemove_Click;
                this.listComputerPlayers.SelectionChanged += this.ListComputerPlayers_SelectionChanged;
                this.viewModel.ComputerPlayers.CollectionChanged += this.ComputerPlayers_CollectionChanged;
            }
        }

        public UniversalNewGameViewModel ViewModel
        {
            get
            {
                return this.viewModel;
            }
        }

        public IEnumerable<Player> Players
        {
            get
            {
                return this.viewModel.Players;
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

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.AddComputerPlayer();

            return;
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.RemoveComputerPlayer(this.listComputerPlayers.SelectedItem as Player);

            return;
        }

        private void ListComputerPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.buttonRemove.IsEnabled = this.listComputerPlayers.SelectedItem != null;

            return;
        }

        private void ComputerPlayers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.buttonAdd.IsEnabled = this.viewModel.ComputerPlayers.Count < NewGameDialog.MaxNumberOfComputerPlayers; // This logic should go in the base NewGameViewModel.

            return;
        }
    }
}
