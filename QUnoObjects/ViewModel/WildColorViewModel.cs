// <copyright file="WildColorViewModel.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.ViewModel
{
    using System.Collections.Generic;
    using Mooville.QUno.Model;

    /// <summary>
    /// Represents the choose a wild color view.
    /// </summary>
    public class WildColorViewModel : ViewModelBase
    {
        private List<Color> colors;
        private Color? selectedColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="WildColorViewModel"/> class.
        /// </summary>
        public WildColorViewModel()
        {
            this.colors = new List<Color>();
            this.colors.Add(Color.Red);
            this.colors.Add(Color.Blue);
            this.colors.Add(Color.Yellow);
            this.colors.Add(Color.Green);
        }

        /// <summary>
        /// Gets the colors that can be chosen.
        /// </summary>
        /// <value>
        /// The colors that can be chosen.
        /// </value>
        public IList<Color> Colors
        {
            get
            {
                return this.colors;
            }
        }

        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        /// <value>
        /// The selected color.
        /// </value>
        public Color? SelectedColor
        {
            get
            {
                return this.selectedColor;
            }

            set
            {
                this.selectedColor = value;
                this.OnPropertyChanged("SelectedColor");
            }
        }
    }
}
