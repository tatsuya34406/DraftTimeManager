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
    public partial class EnvironmentSettingPage : ContentPage
    {
        EnvironmentSettingModel model;
        public EnvironmentSettingPage()
        {
            model = new EnvironmentSettingModel();
            this.BindingContext = model;

            InitializeComponent();

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
            switch (Device.RuntimePlatform)
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

        private void EnvSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            model.Search();
        }

        private void EnvSearchBar_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(model.SearchText)) model.Search();
        }

        private async void PlusIcon_Clicked(object sender, EventArgs e)
        {
            var page = new EnvironmentEditPage();
            page.EnvironmentUpdate += UpdateList;

            await Navigation.PushAsync(page, true);

            SearchResultEnv.SelectedItem = null;
        }

        private async void SearchResultEnv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (SearchResultEnv.SelectedItem == null) return;

            var page = new EnvironmentEditPage(((Environments)((ListView)sender).SelectedItem).Env_Id);
            page.EnvironmentUpdate += UpdateList;

            await Navigation.PushAsync(page, true);

            SearchResultEnv.SelectedItem = null;
        }

        private async void MenuItemDelete_Clicked(object sender, EventArgs e)
        {
            var env = (Environments)((MenuItem)sender).CommandParameter;
            if (await DisplayAlert("DELETE?", env.Env_Name, "Delete", "Cancel"))
            {
                model.EnvironmentDelete(env);
                model.Search();
            }

            SearchResultEnv.SelectedItem = null;
        }

        private void UpdateList(object sender, EventArgs e)
        {
            model.Search();
        }
    }
}
