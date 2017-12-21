using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using DraftTimeManager.Interfaces;
using DraftTimeManager.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PropertyChanged;

namespace DraftTimeManager.Models
{
    public class UserSettingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string SearchText { get; set; }
        public ObservableCollection<Users> UserList { get; set; }
        public Users SelectedUser { get; set; }

        public UserSettingModel()
        {
            SearchReset();
        }

        public void Search()
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                UserList = new ObservableCollection<Users>(
                    conn.Table<Users>().Where(x => x.User_Name.Contains(SearchText) && !x.Guest_Flg).ToList());
            }
        }

        public void SearchReset()
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                UserList = new ObservableCollection<Users>(
                    conn.Table<Users>().Where(x => !x.Guest_Flg).ToList());
            }
        }
    }
}
