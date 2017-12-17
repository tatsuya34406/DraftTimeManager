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
    public class DraftPodModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        int minPlayer = 4;
        int maxPlayer = 8;
        int tempGuest = 0;

        public ObservableCollection<int> PlayerNumbers { get; set; }
        public ObservableCollection<int> GuestNumbers { get; set; }
        public ObservableCollection<Environments> EnvironmentList { get; set; }
        public ObservableCollection<Users> DraftJoinUsers { get; set; }

        public int SelectPlayerNumber { get; set; }
        public int SelectGuestNumber { get; set; }
        public string SelectEnvironment { get; set; }

        public DraftPodModel()
        {
            SelectPlayerNumber = 8;
            SelectGuestNumber = 8;
            PlayerNumbers = new ObservableCollection<int>(Enumerable.Range(minPlayer, maxPlayer - (minPlayer - 1)).Reverse());
            GuestNumbers = new ObservableCollection<int>(Enumerable.Range(1, maxPlayer).Reverse());
            using(var conn = new ConnectionModel().CreateConnection())
            {
                EnvironmentList = new ObservableCollection<Environments>(conn.Table<Environments>().ToList());   
                DraftJoinUsers = new ObservableCollection<Users>(conn.Table<Users>().Where(x => x.Guest_Flg).ToList());   
            }
        }

        public void SetGuestList()
        {
            tempGuest = SelectGuestNumber;
            GuestNumbers = new ObservableCollection<int>(Enumerable.Range(1, SelectPlayerNumber).Reverse());
        }

        public void SetGuest()
        {
            if (SelectGuestNumber != 0) return;
            SelectGuestNumber = SelectPlayerNumber < tempGuest ? SelectPlayerNumber : tempGuest;
            DraftJoinUsers.ToList().Where(x => x.Guest_Flg).OrderBy(x => x.User_Id).Skip(1);
        }

        public void AddDraftJoinUsers(DraftPodUserSearchModel addmodel)
        {
            DraftJoinUsers.Add(addmodel.SelectedUser);
        }
    }
}
