﻿using System;
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
    public partial class UserEditPage : ContentPage
    {
        public event EventHandler UserUpdate;
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
                UserUpdate(model, e);
                await Navigation.PopAsync(true);
            }
        }
    }
}
