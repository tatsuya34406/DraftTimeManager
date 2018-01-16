using System;
using System.Collections.Generic;
using System.Linq;

using DraftTimeManager.Entities;
using DraftTimeManager.Models;

using Xamarin.Forms;

namespace DraftTimeManager.Views
{
    public partial class DraftResult : ContentPage
    {
        private DraftResultModel model;
        public DraftResult(List<DraftResults> playerList)
        {
            InitializeComponent();

            model = new DraftResultModel(playerList);

            CreateResult();
        }

        private void CreateResult()
        {
            Button finishButton = new Button()
            {
                Text = "Finish",
            };

            finishButton.Clicked += (s, e) =>
            {
                model.updateDraftResult();
                Application.Current.MainPage = new DraftTimeManager.Views.Menu();
            };

            model.grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });

            model.grid.Children.Add(finishButton,0,model.grid.Children.Count());

            this.Content = model.grid;

        }
    }
}
