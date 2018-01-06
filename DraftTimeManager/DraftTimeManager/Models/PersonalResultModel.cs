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
         
        public Users User { get; set; }
        public ObservableCollection<GroupingItem> PersonalScoreList { get; set; }

        public PersonalResultModel()
        {
            PersonalScoreList = new ObservableCollection<GroupingItem>();
        }

        public PersonalResultModel(int userid)
        {
            PersonalScoreList = new ObservableCollection<GroupingItem>();
            using(var conn = new ConnectionModel().CreateConnection())
            {
                User = conn.Get<Users>(userid);
            }
            SetOverallScore();
            SetEnvironmentsScore();
        }

        public void SetOverallScore()
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                var results = conn.Table<EnvironmentUserScore>().Where(x => x.User_Id == User.User_Id).ToList();

                PersonalScoreList.Add(GetGroupingItem($"Overall Score", results));
            }
        }

        public void SetEnvironmentsScore()
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                foreach (var env in conn.Table<Environments>().OrderByDescending(x => x.Env_Id))
                {
                    SetEnvironmentScore(env.Env_Id);
                }
            }
        }

        private void SetEnvironmentScore(int EnvironmentId)
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                var env = conn.Get<Environments>(EnvironmentId);
                var results = conn.Table<EnvironmentUserScore>().Where(x => x.User_Id == User.User_Id && x.Env_Id == EnvironmentId).ToList();

                PersonalScoreList.Add(GetGroupingItem($"{env.Env_Name} Score", results));
            }
        }

        private GroupingItem GetGroupingItem(string title, List<EnvironmentUserScore> list)
        {
            var wins = list.Select(x => x.Cnt_Win).Sum();
            var loses = list.Select(x => x.Cnt_Lose).Sum();

            var item = new GroupingItem()
            {
                SectionLabel = title
            };

            item.Add(new PersonalScore()
            {
                ScoreTitle = "Wins-Loses",
                Score = $"{wins}-{loses}"
            });

            item.Add(new PersonalScore()
            {
                ScoreTitle = "Winning Percentage",
                Score = wins + loses == 0 ? 0d.ToString("P") : ((double)wins / ((double)wins + (double)loses)).ToString("P")
            });

            item.Add(new PersonalScore()
            {
                ScoreTitle = "3-0 Counts",
                Score = list.Select(x => x.Cnt_3_0).Sum().ToString()
            });

            item.Add(new PersonalScore()
            {
                ScoreTitle = "2-1 Counts",
                Score = list.Select(x => x.Cnt_2_1).Sum().ToString()
            });

            item.Add(new PersonalScore()
            {
                ScoreTitle = "1-2 Counts",
                Score = list.Select(x => x.Cnt_1_2).Sum().ToString()
            });

            item.Add(new PersonalScore()
            {
                ScoreTitle = "0-3 Counts",
                Score = list.Select(x => x.Cnt_0_3).Sum().ToString()
            });

            return item;
        }

        public class PersonalScore
        {
            public string ScoreTitle { get; set; }
            public string Score { get; set; }
        }

        public class GroupingItem : ObservableCollection<PersonalScore>
        {
            public string SectionLabel { get; set; }
        }
    }
}
