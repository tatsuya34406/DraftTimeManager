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
            this.lblPickStatus.BindingContext = 
                this.lblTimeCount.BindingContext = 
                this.btnStart.BindingContext = model;
        }

        private void btnStart_Clicked(object sender, EventArgs e)
        {
            model.Initialize();
            model.IsBtnEnabled = false;
            var timeunit = 1;

            Device.StartTimer(
                TimeSpan.FromSeconds(timeunit),
                () =>
                {
                    return model.TimeMove(timeunit);
                });
        }
    }
}