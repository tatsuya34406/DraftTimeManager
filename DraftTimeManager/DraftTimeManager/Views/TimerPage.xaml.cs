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
    public partial class TimerPage : ContentPage
    {
        private TimerModel model;

        public TimerPage()
        {
            InitializeComponent();

            model = new TimerModel();
            this.BindingContext = model;

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }

        private void btnStart_Clicked(object sender, EventArgs e)
        {
            model.TimerStart();
            rotateimage.RelRotateTo(10, 1000);
            Device.StartTimer(
                TimeSpan.FromSeconds(1),
                () => 
                {
                if (!model.IsTimerStart) return false;
                    rotateimage.RelRotateTo(10, 1000);
                    return true;
                });
        }

        private void btnPause_Clicked(object sender, EventArgs e)
        {
            model.TimerEnd();
        }

        private void btnReset_Clicked(object sender, EventArgs e)
        {
            model.Initialize();
        }

        private void PackStepper_ValueChanged(object sender, EventArgs e)
        {
            model.Pack = Convert.ToInt32(this.PackStepper.Value);
            model.SetPickTime();
        }

        private void PickStepper_ValueChanged(object sender, EventArgs e)
        {
            model.Pick = Convert.ToInt32(this.PickStepper.Value);
            model.SetPickTime();
        }

        protected override void OnDisappearing()
        {
            model.TimerEnd();
            base.OnDisappearing();
        }

        protected override async void OnAppearing()
        {
            var conn = new ConnectionModel();
            var setting = await conn.GetSettings();
            model.Initialize(setting);

            base.OnAppearing();
        }
    }
}