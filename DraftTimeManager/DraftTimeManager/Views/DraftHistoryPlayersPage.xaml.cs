using DraftTimeManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DraftTimeManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DraftHistoryPlayersPage : ContentPage
    {
        private DraftHistoryPlayersModel Model { get; set; }

        public DraftHistoryPlayersPage(int id)
        {
            InitializeComponent();

            Model = new DraftHistoryPlayersModel(id);
            BindingContext = Model;
            ListView.ItemsSource = Model.Players;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            var playerInfo = (DraftHistoryPlayersModel.PlayerInfo)e.Item;
            await Navigation.PushAsync(new DraftHistoryPlayerDetailPage(playerInfo));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            Model.InitializePlayers();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}