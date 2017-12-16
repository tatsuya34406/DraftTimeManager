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
    }
}
