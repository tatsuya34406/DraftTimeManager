using DraftTimeManager.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DraftTimeManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DraftHistoryPage : ContentPage
    {
        private DraftHistoryModel Model { get; set; }

        public DraftHistoryPage()
        {
            InitializeComponent();
            
            Model = new DraftHistoryModel();
            BindingContext = Model;
            ListView.ItemsSource = Model.Histories;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            var History = (DraftHistoryModel.GroupedHistory)e.Item;
            await Navigation.PushAsync(new DraftHistoryPlayersPage(History.Draft_Id));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            Model.InitializeHistories();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}
