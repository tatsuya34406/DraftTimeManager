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
    public class PersonalResultUserListModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string SearchText { get; set; }
        public ObservableCollection<Users> UserList { get; set; }

        public PersonalResultUserListModel()
        {
            SearchText = string.Empty;
            Search();
        }

        public void Search()
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    UserList = new ObservableCollection<Users>(
                        conn.Table<Users>()
                            .Where(x => !x.Guest_Flg && !x.Delete_Flg)
                            .ToList());
                }
                else
                {
                    UserList = new ObservableCollection<Users>(
                        conn.Table<Users>()
                            .Where(x => x.User_Name.Contains(SearchText) && !x.Guest_Flg && !x.Delete_Flg)
                            .ToList());
                }
            }
        }
    }
}
