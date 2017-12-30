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
    public partial class OverallResultsPage : ContentPage
    {
        OverallResultModel model;
        public OverallResultsPage()
        {
            InitializeComponent();

            model = new OverallResultModel();
            this.BindingContext = model;

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }

        void OverallWinsCountListView_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
