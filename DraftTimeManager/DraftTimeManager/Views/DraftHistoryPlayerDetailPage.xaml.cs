using DraftTimeManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DraftTimeManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DraftHistoryPlayerDetailPage : ContentPage
    {
        private DraftHistoryPlayerDetailModel Model { get; set; }

        public DraftHistoryPlayerDetailPage(DraftHistoryPlayersModel.PlayerInfo playerInfo)
        {
            InitializeComponent();

            Model = new DraftHistoryPlayerDetailModel(playerInfo);
            BindingContext = Model.PlayerInfo;
        }
    }
}