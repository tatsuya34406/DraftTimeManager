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
        bool GuestNumbersResetFlg = false;

        public List<Users> GuestUsersList { get; set; }
        public List<Users> PlayerList { get; set; }

        public ObservableCollection<int> PlayerNumbers { get; set; }
        public ObservableCollection<int> GuestNumbers { get; set; }
        public ObservableCollection<Environments> EnvironmentList { get; set; }
        public ObservableCollection<Users> DraftJoinUsers { get; set; }

        public int SelectPlayerNumber { get; set; }
        public int SelectGuestNumber { get; set; }
        public Environments SelectEnvironment { get; set; }

        public bool IsAbleToRegist => DraftJoinUsers.Count() == SelectPlayerNumber;

        public DraftPodModel()
        {
            SelectPlayerNumber = 8;
            SelectGuestNumber = 8;
            PlayerNumbers = new ObservableCollection<int>(Enumerable.Range(minPlayer, maxPlayer - (minPlayer - 1)).Reverse());
            GuestNumbers = new ObservableCollection<int>(Enumerable.Range(0, maxPlayer + 1).Reverse());
            using(var conn = new ConnectionModel().CreateConnection())
            {
                EnvironmentList = new ObservableCollection<Environments>(conn.Table<Environments>().OrderBy(x => x.Env_Id).ToList());
                GuestUsersList = conn.Table<Users>().Where(x => x.Guest_Flg).Where(x => x.User_Id != 9).OrderBy(x => x.User_Id).ToList();
                DraftJoinUsers = new ObservableCollection<Users>(GuestUsersList);
                PlayerList = new List<Users>();

                SelectEnvironment = EnvironmentList.FirstOrDefault();
            }
        }

        public void SetGuestList()
        {
            tempGuest = SelectGuestNumber;
            GuestNumbers = new ObservableCollection<int>(Enumerable.Range(0, SelectPlayerNumber + 1).Reverse());
            GuestNumbersResetFlg = true;
        }

        public void SetGuest()
        {
            if (GuestNumbersResetFlg)
            {
                SelectGuestNumber = SelectPlayerNumber < tempGuest ? SelectPlayerNumber : tempGuest;
                GuestNumbersResetFlg = false;
            }
            else
            {
                using (var conn = new ConnectionModel().CreateConnection())
                {
                    GuestUsersList = conn.Table<Users>()
                                         .Where(x => x.Guest_Flg)
                                         .Where(x => x.User_Id!=9)
                                         .OrderBy(x => x.User_Id)
                                         .Take(SelectGuestNumber).ToList();
                    DraftJoinUsers = new ObservableCollection<Users>(GuestUsersList.Concat(PlayerList));
                }
            }
        }

        public void AddDraftJoinUsers(DraftPodUserSearchModel addmodel)
        {
            PlayerList = addmodel.JoinUserList;
            DraftJoinUsers = new ObservableCollection<Users>(GuestUsersList.Concat(PlayerList));
        }

        public void RegistDraftPod()
        {
            List<DraftResults> insertdata = new List<DraftResults>();

            using (var conn = new ConnectionModel().CreateConnection())
            {
                var draftid = conn.Table<DraftResults>().Any()
                                  ? conn.Table<DraftResults>().Max(x => x.Draft_Id) + 1
                                  : 0;
                var draftdate = DateTime.Now;

                insertdata.AddRange(DraftJoinUsers.Select(x => new DraftResults
                {
                    Draft_Id = draftid,
                    User_Id = x.User_Id,
                    Env_Id = SelectEnvironment.Env_Id,
                    Draft_Date = draftdate
                }));

                int[] pickno = Enumerable.Range(1, insertdata.Count()).OrderBy(n => Guid.NewGuid()).Take(insertdata.Count()).ToArray();

                foreach(var item in insertdata.Select((u,i) => new{User = u,Index = i}))
                {
                    item.User.Pick_No = pickno[item.Index];
                }

                Application.Current.Properties["TempData"] = insertdata;
                Application.Current.Properties["Users"] = DraftJoinUsers;

            }
        }
    }
}
