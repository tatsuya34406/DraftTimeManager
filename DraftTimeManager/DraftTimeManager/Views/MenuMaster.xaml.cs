using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PropertyChanged;

namespace DraftTimeManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuMaster : ContentPage
    {
        public ListView ListView;

        public MenuMaster()
        {
            InitializeComponent();

            BindingContext = new MenuMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MenuMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MenuMenuItem> MenuItems { get; set; }

            public MenuMasterViewModel()
            {
                MenuItems = new ObservableCollection<MenuMenuItem>(new[]
                {
                    new MenuMenuItem { Id = 0, Title = "DraftTimer", TargetType = typeof(TimerPage) },
                    new MenuMenuItem { Id = 1, Title = "PodCreate" },
                    new MenuMenuItem { Id = 2, Title = "UserRegistration", TargetType = typeof(UserSettingPage) },
                    new MenuMenuItem { Id = 3, Title = "EnvironmentRegistration", TargetType = typeof(EnvironmentSettingPage) },
                    new MenuMenuItem { Id = 4, Title = "OverallResults", TargetType = typeof(OverallResultsPage) },
                    new MenuMenuItem { Id = 5, Title = "PersonalResults" },
                    new MenuMenuItem { Id = 6, Title = "DraftHistory", TargetType = typeof(DraftHistoryPage) },
                    new MenuMenuItem { Id = 7, Title = "Settings", TargetType = typeof(SettingPage) },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            #endregion
        }
    }
}
