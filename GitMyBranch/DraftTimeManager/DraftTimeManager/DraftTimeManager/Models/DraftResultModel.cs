using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using DraftTimeManager.Entities;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DraftTimeManager.Models
{
    public class DraftResultModel
    {
        public Grid grid;
        ObservableCollection<Users> userList;
        ObservableCollection<EnvironmentUserScore> envScore;
        List<OpponentUserScore> opponentScore;

        List<DraftResults> sortedPlayerList;

        List<string> Rank = new List<string>() {"1st","2nd","3rd","4th","5th","6th","7th","8th" };

        public DraftResultModel(List<DraftResults> playerList)
        {
            grid = new Grid()
            {
                ColumnDefinitions = {
                    new ColumnDefinition { Width = GridLength.Star }
                },
            };

            userList = (ObservableCollection<Users>)Application.Current.Properties["Users"];

            if (playerList.Count() == 4)
            {
                for (int i = 2; i >= 0; i--)
                {
                    var resultlist = playerList.Where(m => (m.R1_Result + m.R2_Result) == i);

                    if (sortedPlayerList == null)
                    {
                        sortedPlayerList = resultlist.Where(m => m.User_Id != 9).OrderByDescending(m => m.R1_Result).ThenByDescending(m => m.R2_Result).ToList();
                    }
                    else
                    {
                        sortedPlayerList.AddRange(resultlist.Where(m => m.User_Id != 9).OrderByDescending(m => m.R1_Result).ThenByDescending(m => m.R2_Result).ToList());
                    }
                }
            }
            else
            {
                for (int i = 3; i >= 0; i--)
                {
                    var resultlist = playerList.Where(m => (m.R1_Result + m.R2_Result + m.R3_Result) == i);

                    if (sortedPlayerList == null)
                    {
                        sortedPlayerList = resultlist.Where(m => m.User_Id != 9).OrderByDescending(m => m.R1_Result).ThenByDescending(m => m.R2_Result).ThenByDescending(m => m.R3_Result).ToList();
                    }
                    else
                    {
                        sortedPlayerList.AddRange(resultlist.Where(m => m.User_Id != 9).OrderByDescending(m => m.R1_Result).ThenByDescending(m => m.R2_Result).ThenByDescending(m => m.R3_Result).ToList());
                    }
                }
            }

            foreach(var player in sortedPlayerList.Select((c, i) => new { Contents = c, Index = i }))
            {
                player.Contents.Rank = player.Index + 1;
                grid.RowDefinitions.Add(new RowDefinition(){Height=GridLength.Star});

                var label = new Label()
                {
                    Text = $"{Rank[player.Index]} {userList.Where(m => m.User_Id == player.Contents.User_Id).Select(m=>m.User_Name).First()}",
                    VerticalTextAlignment = TextAlignment.Center,
                    BackgroundColor = (player.Index + 1) % 2 == 1 ? Color.SkyBlue : Color.White,
                    FontSize = 24,
                };
                grid.Children.Add(label,0,player.Index);
            }

            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });

        }

        public void updateDraftResult()
        {
            int roundCount = sortedPlayerList.Count() == 4 ? 2 : 3;
            foreach(var player in sortedPlayerList)
            {
                updateEnvResult(player);

                for (int i = 1; i <= roundCount;i++)
                {
                    updateUserResult(player,i);
                }
            }

            using(var connection = new ConnectionModel().CreateConnection())
            {
                try
                {
                    connection.BeginTransaction();

                    connection.InsertAll(sortedPlayerList);

                    connection.Commit();
                }
                catch
                {
                    connection.Rollback();
                }
            }

            Application.Current.Properties.Remove("TempData");
            Application.Current.Properties.Remove("Users");
        }

        private void updateEnvResult(DraftResults player)
        {
            int winCount = 0;
            int loseCount = 0;

            if(player.R1_Result ==1)
            {
                winCount++;
            }
            else
            {
                loseCount++;
            }

            if (player.R2_Result == 1)
            {
                winCount++;
            }
            else
            {
                loseCount++;
            }

            if (player.R3_Result != null)
            {
                if (player.R3_Result == 1)
                {
                    winCount++;
                }
                else
                {
                    loseCount++;
                }
            }

            using(var connection = new ConnectionModel().CreateConnection())
            {
                envScore = new ObservableCollection<EnvironmentUserScore>(connection.Table<EnvironmentUserScore>().Where(m => m.User_Id ==player.User_Id).Where(m => m.Env_Id == player.Env_Id).ToList());
            }

            if(envScore.Count()==0)
            {
                EnvironmentUserScore newEnvScore = new EnvironmentUserScore()
                {
                    User_Id = player.User_Id,
                    Env_Id = player.Env_Id,
                    Cnt_3_0 = 0,
                    Cnt_2_1 = 0,
                    Cnt_1_2 = 0,
                    Cnt_0_3 = 0,
                    Cnt_Win = 0,
                    Cnt_Lose = 0,
                };

                if(sortedPlayerList.Count() == 8)
                {
                    switch(winCount)
                    {
                        case 3:
                            newEnvScore.Cnt_3_0++;
                            break;
                        case 2:
                            newEnvScore.Cnt_2_1++;
                            break;
                        case 1:
                            newEnvScore.Cnt_1_2++;
                            break;
                        case 0:
                            newEnvScore.Cnt_0_3++;
                            break;
                        default:
                            break;
                    }
                }

                newEnvScore.Cnt_Win += winCount;
                newEnvScore.Cnt_Lose += loseCount;

                using (var connection = new ConnectionModel().CreateConnection())
                {
                    try
                    {
                        connection.BeginTransaction();

                        connection.Insert(newEnvScore);

                        connection.Commit();
                    }
                    catch
                    {
                        connection.Rollback();
                    }
                }
            }
            else
            {
                if (sortedPlayerList.Count() == 8)
                {
                    switch (winCount)
                    {
                        case 3:
                            envScore[0].Cnt_3_0++;
                            break;
                        case 2:
                            envScore[0].Cnt_2_1++;
                            break;
                        case 1:
                            envScore[0].Cnt_1_2++;
                            break;
                        case 0:
                            envScore[0].Cnt_0_3++;
                            break;
                        default:
                            break;
                    }
                }

                envScore[0].Cnt_Win += winCount;
                envScore[0].Cnt_Lose += loseCount;

                using (var connection = new ConnectionModel().CreateConnection())
                {
                    try
                    {
                        connection.BeginTransaction();

                        connection.InsertOrReplace(envScore[0]);

                        connection.Commit();
                    }
                    catch
                    {
                        connection.Rollback();
                    }
                }
            }
        }

        private void updateUserResult(DraftResults player,int Round)
        {
            var vs = typeof(DraftResults).GetProperty($"R{Round}_Vs_User");
            var result = typeof(DraftResults).GetProperty($"R{Round}_Result");

            using (var connection = new ConnectionModel().CreateConnection())
            {
                int opponent = (int)vs.GetValue(player);
                opponentScore = new List<OpponentUserScore>(connection.Table<OpponentUserScore>().Where(m => m.User_Id == player.User_Id).Where(m => m.Vs_User_Id == opponent).ToList());

                if (opponentScore.Count() == 0)
                {
                    OpponentUserScore newOpponentScore = new OpponentUserScore()
                    {
                        User_Id = player.User_Id,
                        Vs_User_Id = (int)vs.GetValue(player),
                        Cnt_Win = 0,
                        Cnt_Lose = 0,
                    };

                    if ((int)result.GetValue(player) == 1)
                    {
                        newOpponentScore.Cnt_Win++;
                    }
                    else
                    {
                        newOpponentScore.Cnt_Lose++;
                    }

                    try
                    {
                        connection.BeginTransaction();

                        connection.Insert(newOpponentScore);

                        connection.Commit();
                    }
                    catch
                    {
                        connection.Rollback();
                    }
                }
                else
                {
                    if ((int)result.GetValue(player) == 1)
                    {
                        opponentScore[0].Cnt_Win++;
                    }
                    else
                    {
                        opponentScore[0].Cnt_Lose++;
                    }

                    try
                    {
                        connection.BeginTransaction();

                        connection.InsertOrReplace(opponentScore[0]);

                        connection.Commit();
                    }
                    catch
                    {
                        connection.Rollback();
                    }
                }
            }
        }
    }
}
