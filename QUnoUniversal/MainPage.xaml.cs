// <copyright file="MainPage.xaml.cs" company="Mooville">
//   Copyright © 2021 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal
{
    using System;
    using System.Collections.Generic;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Resources;
    using Windows.Storage;
    using Windows.Storage.Pickers;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Navigation;
    using Windows.System;
    using Mooville.QUno.Model;
    using Mooville.QUno.Universal.Model;
    using Mooville.QUno.Universal.ViewModel;

    public sealed partial class MainPage : Page
    {
        private readonly MainViewModel viewModel;
        private readonly string fileFilter;
        private readonly string fileSuggestedName;

        public MainPage()
        {
            this.InitializeComponent();

            if (!DesignMode.DesignModeEnabled)
            {
                var resourceLoader = ResourceLoader.GetForCurrentView();
                this.fileFilter = resourceLoader.GetString("FileFilter");
                this.fileSuggestedName = resourceLoader.GetString("FileSuggestedName");

                this.viewModel = new MainViewModel();
                this.DataContext = this.viewModel;
                this.NavigationCacheMode = NavigationCacheMode.Required;

                this.buttonNew.Click += this.ButtonNew_Click;
                this.buttonOpen.Click += this.ButtonOpen_Click;
                this.buttonSave.Click += this.ButtonSave_Click;
                this.buttonSettings.Click += this.ButtonSettings_Click;
                this.buttonPlay.Click += this.ButtonPlay_Click;
                this.buttonDraw.Click += this.ButtonDraw_Click;
                this.buttonNewGame.Click += this.ButtonNewGame_Click;
                this.listHumanHand.SelectionChanged += this.ListHumanHand_SelectionChanged;
                this.listHumanHand.KeyUp += this.ListHumanHand_KeyUp;
                this.listHumanHand.DoubleTapped += this.ListHumanHand_DoubleTapped;
                this.buttonPlay.IsEnabled = false;
            }
        }

        public MainViewModel ViewModel
        {
            get
            {
                return this.viewModel;
            }
        }

        private async void StartGameUI()
        {
            NewGameDialog newGameDialog = new NewGameDialog();
            ContentDialogResult result = await newGameDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var players = newGameDialog.Players;
                this.viewModel.StartGame(players);
            }

            return;
        }

        private async void PlayCardUI()
        {
            Card card = this.listHumanHand.SelectedItem as Card;

            // For some reason, there is a case where the DataTemplate for the 
            // selected card shows something different than the underlying 
            // Card object, so the buttons were being disabled (if the card 
            // cannot play) but the other gestures were allowing it to be played, 
            // which resulted in a crash. So here we guard against that.
            if (this.viewModel.CanPlayCard(card))
            {
                if (card != null)
                {
                    if (card.Color == Color.Wild)
                    {
                        WildColorDialog wildColorDialog = new WildColorDialog();
                        ContentDialogResult result = await wildColorDialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            Color? wildColor = wildColorDialog.SelectedColor as Color?;
                            this.viewModel.PlayCard(card, wildColor);
                        }
                    }
                    else
                    {
                        this.viewModel.PlayCard(card, null);
                    }
                }
            }

            return;
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            this.StartGameUI();

            return;
        }

        private async void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            fileOpenPicker.FileTypeFilter.Add(@".quno");

            StorageFile file = await fileOpenPicker.PickSingleFileAsync();

            if (file != null)
            {
                UniversalGameSerializer serializer = new UniversalGameSerializer();
                var game = await serializer.LoadFromFileAsync(file);
                this.viewModel.OpenGame(game);
            }

            return;
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker fileSavePicker = new FileSavePicker();
            fileSavePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            fileSavePicker.FileTypeChoices.Add(this.fileFilter, new List<string>() { ".quno" });
            fileSavePicker.SuggestedFileName = this.fileSuggestedName;

            StorageFile file = await fileSavePicker.PickSaveFileAsync();

            if (file != null)
            {
                UniversalGameSerializer serializer = new UniversalGameSerializer();
                serializer.SaveToFileAsync(this.viewModel.Game, file);
            }

            return;
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));

            return;
        }

        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            this.StartGameUI();

            return;
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            this.PlayCardUI();

            return;
        }

        private void ButtonDraw_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.DrawCard();

            return;
        }

        private void ListHumanHand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // There is a bug here where if a card is selected that cannot play, 
            // and then if you draw a card and play continues and comes back to you, 
            // and the card that is selected can now play, the Play button will not 
            // be enabled because the selection hasn't yet changed. It's an easy 
            // workaround for the user, because you can just select a different card, 
            // but it still sort of annoying.
            Card card = this.listHumanHand.SelectedItem as Card;
            this.buttonPlay.IsEnabled = this.viewModel.CanPlayCard(card);

            return;
        }

        private void ListHumanHand_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                this.PlayCardUI();
            }
            else if (e.Key == VirtualKey.Space)
            {
                this.viewModel.DrawCard();
            }

            return;
        }

        private void ListHumanHand_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            this.PlayCardUI();

            return;
        }
    }
}
