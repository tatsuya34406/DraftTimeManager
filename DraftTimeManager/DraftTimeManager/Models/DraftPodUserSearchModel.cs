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
    public class DraftPodUserSearchModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Users> UserList { get; set; }
        public Users SelectedUser { get; set; }
        public string SearchText { get; set; }

        public DraftPodUserSearchModel()
        {
            SearchText = "";
            using (var conn = new ConnectionModel().CreateConnection())
            {
                UserList = new ObservableCollection<Users>(conn.Table<Users>().Where(x => !x.Guest_Flg).ToList());
            }
        }

        public void Search()
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                UserList = new ObservableCollection<Users>(
                    conn.Table<Users>().Where(x => !x.Guest_Flg && x.User_Name.Contains(SearchText)).ToList()
                );
            }
        }
    }
}
