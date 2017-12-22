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

        public UserSettingModel()
        {
            SearchText = string.Empty;
            Search();
        }

        public void Search()
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                if(string.IsNullOrWhiteSpace(SearchText))
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
                            .Where(x => x.User_Name.Contains(SearchText.Trim()) && !x.Guest_Flg && !x.Delete_Flg)
                            .ToList());
                }
            }
        }

        public void UserDelete(Users user)
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                try
                {
                    conn.BeginTransaction();

                    user.Delete_Flg = true;
                    conn.Update(user);

                    conn.Commit();
                }
                catch
                {
                    conn.Rollback();
                }
            }
        }
    }
}
