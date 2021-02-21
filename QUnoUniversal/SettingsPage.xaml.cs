// <copyright file="SettingsPage.xaml.cs" company="Mooville">
//   Copyright © 2021 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Mooville.QUno.Universal.ViewModel;
    using Mooville.QUno.ViewModel;
    using Windows.ApplicationModel;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class SettingsPage : Page
    {
        private readonly ISettingsProvider settingsProvider;

        public SettingsPage()
        {
            this.InitializeComponent();

            this.settingsProvider = new UniversalSettingsProvider();

            if (!DesignMode.DesignModeEnabled)
            {
                this.Loaded += this.Page_Loaded;
                this.Unloaded += this.Page_Unloaded;
                this.buttonBack.Click += this.ButtonBack_Click;
                this.textDefaultHumanPlayerName.TextChanged += this.TextDefaultHumanPlayerName_TextChanged;
                this.textVersion.Tapped += this.TextVersion_Tapped;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.buttonBack.IsEnabled = this.Frame.CanGoBack;

            base.OnNavigatedTo(e);

            return;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string versionText = version.ToString();
            this.textVersion.Text = String.Format(CultureInfo.CurrentCulture, textVersion.Text, versionText);

            this.settingsProvider.LoadSettings();
            this.textDefaultHumanPlayerName.Text = this.settingsProvider.DefaultHumanPlayerName;
            this.sliderDefaultComputerPlayers.Value = this.settingsProvider.DefaultComputerPlayers;

            return;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.settingsProvider.DefaultHumanPlayerName = this.textDefaultHumanPlayerName.Text.Trim();
            this.settingsProvider.DefaultComputerPlayers = (int)this.sliderDefaultComputerPlayers.Value;
            this.settingsProvider.SaveSettings();

            return;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }

            return;
        }

        private void TextDefaultHumanPlayerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.buttonBack.IsEnabled = !String.IsNullOrEmpty(textDefaultHumanPlayerName.Text.Trim());

            return;
        }

        private void TextVersion_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (textLivy.Visibility == Visibility.Collapsed)
            {
                textLivy.Visibility = Visibility.Visible;
            }
            else
            {
                textLivy.Visibility = Visibility.Collapsed;
            }

            return;
        }
    }
}
