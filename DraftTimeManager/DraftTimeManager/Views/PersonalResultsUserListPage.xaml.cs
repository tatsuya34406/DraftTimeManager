using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DraftTimeManager.Models;
using DraftTimeManager.Entities;

namespace DraftTimeManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonalResultsUserListPage : ContentPage
    {
        PersonalResultUserListModel model;
        public PersonalResultsUserListPage()
        {
            InitializeComponent();

            model = new PersonalResultUserListModel();
            this.BindingContext = model;

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }

        private void UserSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            model.Search();
        }

        private void UserSearchBar_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(model.SearchText)) model.Search();
        }

        private async void SearchResultUser_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (SearchResultUser.SelectedItem == null) return;

            var page = new PersonalResultsPage(((Users)SearchResultUser.SelectedItem).User_Id);

            await Navigation.PushAsync(page, true);

            SearchResultUser.SelectedItem = null;
        }
    }
}
