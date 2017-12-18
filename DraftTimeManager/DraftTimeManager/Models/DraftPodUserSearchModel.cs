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

        public List<JoinUsers> AllUserList { get; set; }
        public ObservableCollection<JoinUsers> UserList { get; set; }
        public string SearchText { get; set; }
        public List<Users> JoinUserList => AllUserList.Where(x => x.JoinFlg).Cast<Users>().ToList();

        public DraftPodUserSearchModel(List<Users> joinlist)
        {
            SearchText = "";
            using (var conn = new ConnectionModel().CreateConnection())
            {
                AllUserList = conn.Table<Users>()
                    .Where(x => !x.Guest_Flg)
                    .Select(x => new JoinUsers
                    {
                        User_Id = x.User_Id,
                        User_Name = x.User_Name,
                        DCI_Num = x.DCI_Num,
                        Guest_Flg = x.Guest_Flg,
                        JoinFlg = false
                    }).OrderBy(x => x.User_Name).ToList();

                foreach (var user in AllUserList)
                {
                    if (joinlist.Any(x => x.User_Id == user.User_Id))
                        user.JoinFlg = true;
                }

                UserList = new ObservableCollection<JoinUsers>(AllUserList);
            }
        }

        public void Search()
        {
            UserList = new ObservableCollection<JoinUsers>(AllUserList.Where(x => x.User_Name.Contains(SearchText)));
        }

        public void SearchReset()
        {
            UserList = new ObservableCollection<JoinUsers>(AllUserList);
        }

        public void TempSaveJoinUsers()
        {
            var TmpAllUserList = AllUserList;
            foreach(var user in UserList)
            {
                TmpAllUserList.RemoveAll(x => x.User_Id == user.User_Id);
                TmpAllUserList.Add(user);
            }
            AllUserList = TmpAllUserList.OrderBy(x => x.User_Name).ToList();
        }

        public class JoinUsers : Users
        {
            public bool JoinFlg { get; set; }
        }
    }
}
