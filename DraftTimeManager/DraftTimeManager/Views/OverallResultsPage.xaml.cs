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

        async void OverallWinsCountListView_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (OverallWinsCountListView.SelectedItem == null) return;

            var page = new PersonalResultsPage(((OverallResultModel.OverallScore)e.SelectedItem).UserId);

            await Navigation.PushAsync(page, true);

            OverallWinsCountListView.SelectedItem = null;
        }
    }
}
