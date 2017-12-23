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
    public class EnvironmentEditModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int EnvId;
        public string EnvName { get; set; }
        public int Picks { get; set; }

        public bool IsRegist => !string.IsNullOrWhiteSpace(EnvName);

        public EnvironmentEditModel()
        {
            EnvId = 0;
            EnvName = string.Empty;
            Picks = 14;
        }

        public EnvironmentEditModel(int envid)
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                var env = conn.Get<Environments>(envid);

                EnvId = env.Env_Id;
                EnvName = env.Env_Name;
                Picks = env.Picks;
            }
        }

        public void EnvironmentRegist()
        {
            var env = new Environments
            {
                Env_Id = EnvId,
                Env_Name = EnvName,
                Picks = Picks,
                Default_Flg = false,
                Delete_Flg = false
            };

            using (var conn = new ConnectionModel().CreateConnection())
            {
                try
                {
                    conn.BeginTransaction();

                    if (env.Env_Id == 0)
                    {
                        conn.Insert(env);
                    }
                    else
                    {
                        conn.Update(env);
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
