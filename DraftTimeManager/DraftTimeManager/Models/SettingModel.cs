using System;
using System.Collections.Generic;
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
    public class SettingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<int> DefaultPicksList => Enumerable.Range(1, 15).Reverse().ToList();

        public int Volume { get; set; }
        public int Pick_Interval { get; set; }
        public int Picks { get; set; }

        public SettingModel()
        {
            SetInitialSetting();
        }

        public void SetInitialSetting()
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                var setting = conn.Table<Settings>().FirstOrDefault();
                if (setting == null)
                {
                    setting = new Settings() { Volume = 50, Pick_Interval = 10, Picks = 14 };
                }

                Volume = setting.Volume;
                Pick_Interval = setting.Pick_Interval;
                Picks = setting.Picks;
            }
        }
    }
}
