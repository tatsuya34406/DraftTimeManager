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
    public partial class EnvironmentEditPage : ContentPage
    {
        public event EventHandler EnvironmentUpdate;
        EnvironmentEditModel model;
        public EnvironmentEditPage()
        {
            model = new EnvironmentEditModel();
            this.BindingContext = model;

            InitializeComponent();

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }

        public EnvironmentEditPage(int envid)
        {
            model = new EnvironmentEditModel(envid);
            this.BindingContext = model;

            InitializeComponent();

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }

        async void btnRegist_Clicked(object sender, System.EventArgs e)
        {
            if (model.IsRegist)
            {
                model.EnvironmentRegist();
                EnvironmentUpdate(model, e);
                await Navigation.PopAsync(true);
            }
        }
    }
}
