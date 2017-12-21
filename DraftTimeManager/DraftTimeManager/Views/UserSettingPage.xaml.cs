using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DraftTimeManager.Models;

namespace DraftTimeManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserSettingPage : ContentPage
    {
        UserSettingModel model;
        public UserSettingPage()
        {
            model = new UserSettingModel();
            this.BindingContext = model;

            InitializeComponent();

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
            switch(Device.RuntimePlatform)
            {
                case Device.iOS:
                    PlusIcon.Icon = new FileImageSource { File = "icon-plus.png" };
                    break;
                case Device.Android:
                    PlusIcon.Icon = new FileImageSource { File = "icon_plus.png" };
                    break;
                default:
                    break;
            }
        }

        private void UserSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            model.Search();
        }

        private void UserSearchBar_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(model.SearchText)) model.SearchReset();
        }

        private async void PlusIcon_Clicked(object sender, System.EventArgs e)
        {
            var page = new UserEditPage();

            await Navigation.PushAsync(page, true);
        }

        private async void SearchResultUser_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var page = new UserEditPage(model.SelectedUser.User_Id);

            await Navigation.PushAsync(page, true);
        }
    }
}
