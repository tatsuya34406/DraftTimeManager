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
    public class EnvironmentSettingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string SearchText { get; set; }
        public ObservableCollection<Environments> EnvironmentsList { get; set; }

        public EnvironmentSettingModel()
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
                    EnvironmentsList = new ObservableCollection<Environments>(
                        conn.Table<Environments>()
                            .Where(x => !x.Default_Flg && !x.Delete_Flg)
                            .ToList());
                }
                else
                {
                    EnvironmentsList = new ObservableCollection<Environments>(
                        conn.Table<Environments>()
                            .Where(x => x.Env_Name.Contains(SearchText) && !x.Default_Flg && !x.Delete_Flg)
                            .ToList());
                }
            }
        }

        public void EnvironmentDelete(Environments env)
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                try
                {
                    conn.BeginTransaction();

                    env.Delete_Flg = true;
                    conn.Update(env);

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
