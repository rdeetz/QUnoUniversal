// <copyright file="App.xaml.cs" company="Mooville">
//   Copyright © 2021 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.Storage;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;
    using Mooville.QUno.Model;
    using Mooville.QUno.Universal.Model;

    public sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;
            Game game = null;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += this.OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    game = await this.LoadTempGame();
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);

                    if (game != null)
                    {
                        this.SetActiveGame(game);
                    }
                }

                Window.Current.Activate();
            }

            return;
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            var game = this.GetActiveGame();
            
            if (game != null)
            {
                this.SaveTempGame(game);
            }

            deferral.Complete();

            return;
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load page " + e.SourcePageType.FullName);
        }

        private Game GetActiveGame()
        {
            Game game = null;
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null)
            {
                MainPage mainPage = rootFrame.Content as MainPage;

                if (mainPage != null)
                {
                    game = mainPage.ViewModel.Game;

                    if (game != null)
                    {
                        if (game.IsGameOver)
                        {
                            // If we got all the way here, we have an real game, 
                            // but that game might be over, so if it is, return null.
                            game = null;
                        }
                    }
                }
            }

            return game;
        }

        private void SetActiveGame(Game game)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null)
            {
                MainPage mainPage = rootFrame.Content as MainPage;

                if (mainPage != null)
                {
                    mainPage.ViewModel.OpenGame(game);
                }
            }

            return;
        }

        private async void SaveTempGame(Game game)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile tempFile = await folder.CreateFileAsync(@"temp.quno", CreationCollisionOption.ReplaceExisting);
            UniversalGameSerializer serializer = new UniversalGameSerializer();
            serializer.SaveToFileAsync(game, tempFile);

            return;
        }

        private async Task<Game> LoadTempGame()
        {
            Game game = null;
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            try
            {
                StorageFile tempFile = await folder.GetFileAsync(@"temp.quno");

                try
                {
                    UniversalGameSerializer serializer = new UniversalGameSerializer();
                    game = await serializer.LoadFromFileAsync(tempFile);
                }
                catch (Exception)
                {
                    // Nothing to do here if we can't deserialize the game, 
                    // so allow this method to return null and thus start a new game.
                }
                finally
                {
                    await tempFile.DeleteAsync();
                }
            }
            catch (FileNotFoundException)
            {
                // There must not be saved game, so allow this method to retun null.
            }

            return game;
        }
    }
}
