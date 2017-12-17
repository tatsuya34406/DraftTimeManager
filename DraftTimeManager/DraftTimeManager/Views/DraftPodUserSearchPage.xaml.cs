using System;
using System.Collections.Generic;

using Xamarin.Forms;

using DraftTimeManager.Models;

namespace DraftTimeManager.Views
{
    public partial class DraftPodUserSearchPage : ContentPage
    {
        public event EventHandler UserSelected;

        public DraftPodUserSearchModel model;
        public DraftPodUserSearchPage(DraftPodUserSearchModel searchModel)
        {
            InitializeComponent();

            model = searchModel;
            this.BindingContext = model;

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }

        private void UserSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            model.Search();
        }

        private void SearchResultUser_ItemSelected(object sender, EventArgs e)
        {
            
        }

        protected override void OnDisappearing()
        {
            if(UserSelected != null)
            {
                UserSelected(model, EventArgs.Empty);
            }

            base.OnDisappearing();
        }
    }
}
