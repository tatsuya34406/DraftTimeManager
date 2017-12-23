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
    public partial class SettingPage : ContentPage
    {
        SettingModel model;
        public SettingPage()
        {
            model = new SettingModel();
            this.BindingContext = model;

            InitializeComponent();

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }

        void btnReset_Clicked(object sender, System.EventArgs e)
        {
            model.SetInitialSetting();
        }

        protected override void OnDisappearing()
        {
            model.SaveSetting();

            base.OnDisappearing();
        }
    }
}
