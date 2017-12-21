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
    public class UserEditModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int UserId;
        public string UserName { get; set; }
        public string DCINumber { get; set; }

        public bool IsRegist => !string.IsNullOrWhiteSpace(UserName);

        public UserEditModel()
        {
            UserId = 0;
        }

        public UserEditModel(int userid)
        {
            using(var conn = new ConnectionModel().CreateConnection())
            {
                var user = conn.Get<Users>(userid);

                UserId = user.User_Id;
                UserName = user.User_Name;
                DCINumber = user.DCI_Num;
            }
        }

        public void UserRegist()
        {
            var user = new Users
            {
                User_Id = UserId,
                User_Name = UserName,
                DCI_Num = DCINumber,
                Guest_Flg = false,
            };

            using(var conn = new ConnectionModel().CreateConnection())
            {
                try
                {
                    conn.BeginTransaction();

                    if (user.User_Id == 0)
                    {
                        conn.Insert(user);
                    }
                    else
                    {
                        conn.Update(user);
                    }

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
