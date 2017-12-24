using DraftTimeManager.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftTimeManager.Models
{
    public class DraftHistoryPlayersModel
    {
        private int Id { get; set; }

        public ObservableCollection<PlayerInfo> Players { get; set; }

        public DraftHistoryPlayersModel(int id)
        {
            Id = id;
            Players = new ObservableCollection<PlayerInfo>();
        }

        public async void InitializePlayers()
        {
            Players.Clear();

            using (var connection = await new ConnectionModel().CreateConnectionAsync())
            {
                var results = connection.Table<DraftResults>().Where(x => x.Draft_Id == Id).OrderBy(x => x.Rank).ToList();
                var user_ids = results.Select(x => x.User_Id).ToList();
                var users = connection.Table<Users>().Where(x => user_ids.Contains(x.User_Id) && !x.Delete_Flg).ToList();

                foreach (var result in results)
                {
                    var default_user = new Users() {
                        User_Id = 0,
                        User_Name = "Deleted User",
                        DCI_Num = "None"
                    };
                    var player = users.Where(x => x.User_Id == result.User_Id).DefaultIfEmpty(default_user).First();
                    var r1_opponent = users.Where(x => x.User_Id == result.R1_Vs_User).DefaultIfEmpty(default_user).First();
                    var r2_opponent = users.Where(x => x.User_Id == result.R2_Vs_User).DefaultIfEmpty(default_user).First();
                    var r3_opponent = users.Where(x => x.User_Id == result.R3_Vs_User).DefaultIfEmpty(default_user).First();
                    var player_info = new PlayerInfo()
                    {
                        Player_Id = player.User_Id,
                        Player_Name = player.User_Name,
                        DCI_Num = player.DCI_Num,
                        Rank = result.Rank,
                        R1_Opponent_Id = result.R1_Vs_User,
                        R1_Opponent_Name = r1_opponent.User_Name,
                        R1_Result = result.R1_Result,
                        R2_Opponent_Id = result.R2_Vs_User,
                        R2_Opponent_Name = r2_opponent.User_Name,
                        R2_Result = result.R2_Result,
                        R3_Opponent_Id = result.R2_Vs_User,
                        R3_Opponent_Name = r3_opponent.User_Name,
                        R3_Result = result.R3_Result
                    };

                    Players.Add(player_info);
                }
            }
        }
    }
}