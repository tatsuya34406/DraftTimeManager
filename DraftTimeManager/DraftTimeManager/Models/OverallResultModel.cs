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
    public class OverallResultModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<GroupingItem> OverallScoreList { get; set; }

        public OverallResultModel()
        {
            OverallScoreList = new ObservableCollection<GroupingItem>();
            SetOverallWinsCount();
            SetOverallWinningPercentage();
            SetEnvironment();
        }

        public void SetOverallWinsCount()
        {
            using(var conn = new ConnectionModel().CreateConnection())
            {
                var users = conn.Table<Users>().Where(x => x.Guest_Flg == false && x.Delete_Flg == false).ToList();
                var results = conn.Table<EnvironmentUserScore>().ToList();

                var overallresult =
                    users.Select(x => new OverallScore
                    {
                        UserName = x.User_Name,
                        wins = results.Where(y => y.User_Id == x.User_Id).Select(y => y.Cnt_Win).Sum(),
                        loses = results.Where(y => y.User_Id == x.User_Id).Select(y => y.Cnt_Lose).Sum(),
                    }).ToList();

                var item = new GroupingItem()
                {
                    SectionLabel = "Overall Wins Count"
                };

                var rank = 1;
                foreach(var overall in overallresult.OrderByDescending(x => x.wins))
                {
                    overall.Rank = rank;
                    overall.Score = $"{overall.wins} wins";
                    item.Add(overall);
                    rank++;
                }

                OverallScoreList.Add(item);
            }
        }

        public void SetOverallWinningPercentage()
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                var users = conn.Table<Users>().Where(x => x.Guest_Flg == false && x.Delete_Flg == false).ToList();
                var results = conn.Table<EnvironmentUserScore>().ToList();

                var overallresult =
                    users.Select(x => new OverallScore
                    {
                        UserName = x.User_Name,
                        wins = results.Where(y => y.User_Id == x.User_Id).Select(y => y.Cnt_Win).Sum(),
                        loses = results.Where(y => y.User_Id == x.User_Id).Select(y => y.Cnt_Lose).Sum(),
                    }).ToList();

                var item = new GroupingItem()
                {
                    SectionLabel = "Overall Winning Percentage"
                };

                var rank = 1;
                foreach (var overall in overallresult.OrderByDescending(x => x.percentage))
                {
                    overall.Rank = rank;
                    overall.Score = $"{overall.percentage} %";
                    item.Add(overall);
                    rank++;
                }

                OverallScoreList.Add(item);
            }
        }

        public void SetEnvironment()
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                foreach(var env in conn.Table<Environments>().OrderByDescending(x => x.Env_Id))
                {
                    OverallScoreList.Add(SetEnvironmentWinsCount(env.Env_Id));
                    OverallScoreList.Add(SetEnvironmentWinningPercentage(env.Env_Id));
                }
            }
        }

        public GroupingItem SetEnvironmentWinsCount(int EnvironmentId)
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                var users = conn.Table<Users>().Where(x => x.Guest_Flg == false && x.Delete_Flg == false).ToList();
                var results = conn.Table<EnvironmentUserScore>().Where(x => x.Env_Id == EnvironmentId).ToList();
                var env = conn.Get<Environments>(EnvironmentId);

                var overallresult =
                    users.Select(x => new OverallScore
                    {
                        UserName = x.User_Name,
                        wins = results.Where(y => y.User_Id == x.User_Id).Select(y => y.Cnt_Win).Sum(),
                        loses = results.Where(y => y.User_Id == x.User_Id).Select(y => y.Cnt_Lose).Sum(),
                    }).ToList();

                var item = new GroupingItem()
                {
                    SectionLabel = $"{env.Env_Name} Wins Count"
                };

                var rank = 1;
                foreach (var overall in overallresult.OrderByDescending(x => x.percentage))
                {
                    overall.Rank = rank;
                    overall.Score = $"{overall.wins} wins";
                    item.Add(overall);
                    rank++;
                }

                return item;
            }
        }

        public GroupingItem SetEnvironmentWinningPercentage(int EnvironmentId)
        {
            using (var conn = new ConnectionModel().CreateConnection())
            {
                var users = conn.Table<Users>().Where(x => x.Guest_Flg == false && x.Delete_Flg == false).ToList();
                var results = conn.Table<EnvironmentUserScore>().Where(x => x.Env_Id == EnvironmentId).ToList();
                var env = conn.Get<Environments>(EnvironmentId);

                var overallresult =
                    users.Select(x => new OverallScore
                    {
                        UserName = x.User_Name,
                        wins = results.Where(y => y.User_Id == x.User_Id).Select(y => y.Cnt_Win).Sum(),
                        loses = results.Where(y => y.User_Id == x.User_Id).Select(y => y.Cnt_Lose).Sum(),
                    }).ToList();

                var item = new GroupingItem()
                {
                    SectionLabel = $"{env.Env_Name} Winning Percentage"
                };

                var rank = 1;
                foreach (var overall in overallresult.OrderByDescending(x => x.percentage))
                {
                    overall.Rank = rank;
                    overall.Score = $"{overall.percentage} %";
                    item.Add(overall);
                    rank++;
                }

                return item;
            }
        }
    }

    public class OverallScore
    {
        public int Rank { get; set; }
        public string UserName { get; set; }
        public int wins { get; set; }
        public int loses { get; set; }
        public decimal percentage => wins + loses == 0 ? 0m : Math.Round(((decimal)wins / ((decimal)wins + (decimal)loses)) * 100m, 3, MidpointRounding.AwayFromZero);
        public string Score { get; set; }
    }

    public class GroupingItem : ObservableCollection<OverallScore>
    {
        public string SectionLabel { get; set; }
    }
}
