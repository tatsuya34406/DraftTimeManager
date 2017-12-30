using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DraftTimeManager.Views
{
    public partial class PersonalResultsPage : ContentPage
    {
        public PersonalResultsPage()
        {
            InitializeComponent();

            background.Source = ImageSource.FromResource("DraftTimeManager.Images.cork-wallet.png");
        }
    }
}
