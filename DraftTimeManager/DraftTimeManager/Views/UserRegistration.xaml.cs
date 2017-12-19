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
    public partial class UserRegistration : ContentPage
    {
        UserRegistrationModel model;
        public UserRegistration()
        {
            model = new UserRegistrationModel();
            this.BindingContext = model;

            InitializeComponent();

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }

        private void UserSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            model.Search();
        }

        private void UserSearchBar_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(model.SearchText)) model.SearchReset();
        }
    }
}
