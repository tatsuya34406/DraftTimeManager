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
    public partial class UserEditPage : ContentPage
    {
        UserEditModel model;
        public UserEditPage()
        {
            model = new UserEditModel();
            this.BindingContext = model;

            InitializeComponent();

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }

        public UserEditPage(int userid)
        {
            model = new UserEditModel(userid);
            this.BindingContext = model;

            InitializeComponent();

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }

        async void btnRegist_Clicked(object sender, System.EventArgs e)
        {
            if (model.IsRegist)
            {
                model.UserRegist();
                await Navigation.PopAsync(true);
            }
        }
    }
}
