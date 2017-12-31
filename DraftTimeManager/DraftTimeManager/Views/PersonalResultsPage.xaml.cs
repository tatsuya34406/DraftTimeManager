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
    public partial class PersonalResultsPage : ContentPage
    {
        PersonalResultModel model;
        public PersonalResultsPage()
        {
            InitializeComponent();

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }

        public PersonalResultsPage(int userid)
        {
            InitializeComponent();

            model = new PersonalResultModel(userid);
            this.BindingContext = model;

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }
    }
}
