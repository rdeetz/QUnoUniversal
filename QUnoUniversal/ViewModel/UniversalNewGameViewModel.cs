// <copyright file="UniversalNewGameViewModel.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using Mooville.QUno.Model;
    using Mooville.QUno.ViewModel;

    public class UniversalNewGameViewModel : NewGameViewModel
    {
        public UniversalNewGameViewModel(string humanPlayerName, int numberOfComputerPlayers, string playerNameTemplate)
            : base(humanPlayerName, numberOfComputerPlayers, playerNameTemplate)
        {
        }

        #region Public Properties

        public ObservableCollection<Player> ComputerPlayers
        {
            get
            {
                return base.ComputerPlayersCollection;
            }
        }

        #endregion
    }
}
