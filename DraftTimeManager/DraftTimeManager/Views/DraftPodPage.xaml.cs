using System;
using System.Collections.Generic;

using Xamarin.Forms;

using DraftTimeManager.Models;

namespace DraftTimeManager.Views
{
    public partial class DraftPodPage : ContentPage
    {
        DraftPodModel model;
        public DraftPodPage()
        {
            InitializeComponent();

            model = new DraftPodModel();

            this.BindingContext = model;
            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");

        }

        private void PickerPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            model.SetGuestList();

        }

        private void PickerGuest_SelectedIndexChanged(object sender, EventArgs e)
        {
            model.SetGuest();
        }

        private async void btnAddUser_Clicked(object sender, EventArgs e)
        {
            var searchModel = new DraftPodUserSearchModel(model.PlayerList);
            var page = new DraftPodUserSearchPage(searchModel);
            page.UserSelected += UserSelected;

            await Navigation.PushAsync(page, true);
        }

        private void UserSelected(object sender, EventArgs e)
        {
            var searchModel = (DraftPodUserSearchModel)sender;
            model.AddDraftJoinUsers(searchModel);
        }

        private async void btnRegist_Clicked(object sender, EventArgs e)
        {
            if (!model.IsAbleToRegist)
            {
                await DisplayAlert(
                    "Can not create Draft Pod.",
                    $"Must be {model.SelectPlayerNumber} people.",
                    "OK");
                return;
            }

            model.RegistDraftPod();

            await DisplayAlert(
                    "Create Pod!",
                    "Move to Draft Timer.",
                    "OK");
            return;
        }
    }
}
