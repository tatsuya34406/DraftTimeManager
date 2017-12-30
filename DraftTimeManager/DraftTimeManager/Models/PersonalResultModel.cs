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
    public class PersonalResultModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<GroupingItem> OverallScoreList { get; set; }

        public PersonalResultModel()
        {
        }

        public class PersonalScore
        {
            public int Rank { get; set; }
            public string UserName { get; set; }
            public int wins { get; set; }
            public int loses { get; set; }
            public decimal percentage => wins + loses == 0 ? 0m : Math.Round(((decimal)wins / ((decimal)wins + (decimal)loses)) * 100m, 3, MidpointRounding.AwayFromZero);
            public string Score { get; set; }
        }

        public class GroupingItem : ObservableCollection<PersonalScore>
        {
            public string SectionLabel { get; set; }
        }
    }
}
