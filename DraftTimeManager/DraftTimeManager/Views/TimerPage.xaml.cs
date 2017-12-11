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
                this.btnStart.BindingContext = 
                this.btnPause.BindingContext = model;
        }

        private void btnStart_Clicked(object sender, EventArgs e)
        {
            model.TimerStart();
        }

        private void btnPause_Clicked(object sender, EventArgs e)
        {
            
        }

        protected override void OnDisappearing()
        {
            model.TimerEnd();
            base.OnDisappearing();
        }
    }
}