using System;
using System.Linq;
using SQLite;
using System.Collections.Generic;
using System.Text;

using DraftTimeManager.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using PCLStorage;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DraftTimeManager.Models
{
    public class MatchInfo
    {
        public string PlayerA_Name { get; set; }

        public int PlayerA_ID { get; set; }

        public int PlayerA_PickNo { get; set; }

        public string PlayerB_Name { get; set; }

        public int PlayerB_ID { get; set; }

        public int CurrentRound { get; set; }

        public bool buttonEnable { get; set; }

        public Color ButtonColor { get; set; }

        public int TournamentGroup { get; set; }
    }

    public class MatchTableModel
    {
        public List<DraftResults> PlayerList{ get;set;}
        public ObservableCollection<Users> userList { get; set; }

        public MatchTableModel(bool randomFlg)
        {
            var random = new Random();

            var connectionModel = new ConnectionModel();

            PlayerList = (List<DraftResults>)Application.Current.Properties["TempData"];
            userList = (ObservableCollection<Users>)Application.Current.Properties["Users"];

            if (PlayerList.Count() > 4 && PlayerList.Count() < 8)
            {
                userList.Add(new Users() { User_Id = 9, User_Name = "Bye", Guest_Flg = true });
            }

            if(PlayerList.Count() > 4)
            {
                while(8 - PlayerList.Count() != 0)
                {
                    PlayerList.Add(new DraftResults
                    {
                        Draft_Id = PlayerList[0].Draft_Id,
                        User_Id = 9,
                        Env_Id = PlayerList[0].Env_Id,
                        Pick_No = PlayerList.Count() + 1,
                        Draft_Date = PlayerList[0].Draft_Date
                    });
                }
            }


            if (randomFlg)
            {
                int[] tournamentNo = Enumerable.Range(1, PlayerList.Count()).OrderBy(n => Guid.NewGuid()).Take(PlayerList.Count()).ToArray();

                foreach(var player in PlayerList.Select((c, i) => new { Contents = c, Index = i }))
                {
                    player.Contents.Tournament_No = tournamentNo[player.Index];
                }

                Application.Current.Properties["TempData"] = PlayerList;
            }
            else
            {
                if (PlayerList.Count() == 4)
                {
                    foreach (var player in PlayerList)
                    {
                        if (player.Pick_No == 1 || player.Pick_No == 4)
                        {
                            player.Tournament_No = player.Pick_No;
                        }
                        else
                        {
                            if (player.Pick_No % 2 == 0)
                            {
                                player.Tournament_No = player.Pick_No + 1;
                            }
                            else
                            {
                                player.Tournament_No = player.Pick_No - 1;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var player in PlayerList)
                    {
                        if (player.Pick_No == 1 || player.Pick_No == 3 || player.Pick_No == 6 || player.Pick_No == 8)
                        {
                            player.Tournament_No = player.Pick_No;
                        }
                        else
                        {
                            if (player.Pick_No % 2 == 0)
                            {
                                player.Tournament_No = player.Pick_No + 3;
                            }
                            else
                            {
                                player.Tournament_No = player.Pick_No - 3;
                            }
                        }
                    }
                }

                Application.Current.Properties["TempData"] = PlayerList;
            }
        }

        public void createPairing()
        {
            foreach (var player in PlayerList)
            {
                bool matchFlg = false;
                int currentRound = 0;

                for (int i = 1; i < 4; i++)
                {
                    if (matchFlg = opponentCheck(player, i))
                    {
                        currentRound = i;
                        break;
                    }
                    else if (matchFinishedCheck(player, i))
                    {
                        break;
                    }
                }

                if (!matchFlg)
                {
                    continue;
                }

                switch (currentRound)
                {
                    case 1:
                        if (player.Tournament_No % 2 == 1)
                        {
                            player.R1_Vs_User = PlayerList.Where(m => m.Tournament_No == player.Tournament_No + 1).Select(m => m.User_Id).First();
                            PlayerList.Where(m => m.Tournament_No == player.Tournament_No + 1).Select(m => m.R1_Vs_User = player.User_Id).ToList();
                            if(player.User_Id == 9)
                            {
                                player.R1_Result = 0;
                                PlayerList.Where(m => m.Tournament_No == player.Tournament_No + 1).Select(m => m.R1_Result = 1).ToList();
                            }
                            else if(player.R1_Vs_User == 9)
                            {
                                player.R1_Result = 1;
                                PlayerList.Where(m => m.Tournament_No == player.Tournament_No + 1).Select(m => m.R1_Result = 0).ToList();
                            }
                            break;
                        }
                        else
                        {
                            player.R1_Vs_User = PlayerList.Where(m => m.Tournament_No == player.Tournament_No - 1).Select(m => m.User_Id).First();
                            PlayerList.Where(m => m.Tournament_No == player.Tournament_No - 1).Select(m => m.R1_Vs_User = player.User_Id).ToList();
                            if (player.User_Id == 9)
                            {
                                player.R1_Result = 0;
                                PlayerList.Where(m => m.Tournament_No == player.Tournament_No - 1).Select(m => m.R1_Result = 1).ToList();
                            }
                            else if (player.R1_Vs_User == 9)
                            {
                                player.R1_Result = 1;
                                PlayerList.Where(m => m.Tournament_No == player.Tournament_No - 1).Select(m => m.R1_Result = 0).ToList();
                            }
                            break;
                        }
                    case 2:
                        if (player.Tournament_No < 5)
                        {
                            if (PlayerList.Where(m => m.Tournament_No < 5).Where(m => m.R1_Result == player.R1_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.User_Id).Count() != 0)
                            {
                                player.R2_Vs_User = PlayerList.Where(m => m.Tournament_No < 5).Where(m => m.R1_Result == player.R1_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.User_Id).First();
                                PlayerList.Where(m => m.Tournament_No < 5).Where(m => m.R1_Result == player.R1_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.R2_Vs_User = player.User_Id).ToList();
                                if (player.User_Id == 9)
                                {
                                    player.R2_Result = 0;
                                    PlayerList.Where(m => m.Tournament_No < 5).Where(m => m.R1_Result == player.R1_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.R2_Result = 1).ToList();
                                }
                                else if (player.R2_Vs_User == 9)
                                {
                                    player.R2_Result = 1;
                                    PlayerList.Where(m => m.Tournament_No < 5).Where(m => m.R1_Result == player.R1_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.R2_Result = 0).ToList();
                                }
                            }
                            break;
                        }
                        else
                        {
                            if(PlayerList.Where(m => m.Tournament_No > 4).Where(m => m.R1_Result == player.R1_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.User_Id).Count()!=0)
                            {
                                player.R2_Vs_User = PlayerList.Where(m => m.Tournament_No > 4).Where(m => m.R1_Result == player.R1_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.User_Id).First();
                                PlayerList.Where(m => m.Tournament_No > 4).Where(m => m.R1_Result == player.R1_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.R2_Vs_User = player.User_Id).ToList();
                                if (player.User_Id == 9)
                                {
                                    player.R2_Result = 0;
                                    PlayerList.Where(m => m.Tournament_No > 4).Where(m => m.R1_Result == player.R1_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.R2_Result = 1).ToList();
                                }
                                else if (player.R2_Vs_User == 9)
                                {
                                    player.R2_Result = 1;
                                    PlayerList.Where(m => m.Tournament_No > 4).Where(m => m.R1_Result == player.R1_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.R2_Result = 0).ToList();
                                }
                            }

                            break;
                        }
                    case 3:
                        {
                            if(PlayerList.Where(m => m.R1_Result == player.R1_Result).Where(m => m.R2_Result == player.R2_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.User_Id).Count()!=0)
                            {
                                player.R3_Vs_User = PlayerList.Where(m => m.R1_Result == player.R1_Result).Where(m => m.R2_Result == player.R2_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.User_Id).First();
                                PlayerList.Where(m => m.R1_Result == player.R1_Result).Where(m => m.R2_Result == player.R2_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.R3_Vs_User = player.User_Id).ToList();
                                if (player.User_Id == 9)
                                {
                                    player.R3_Result = 0;
                                    PlayerList.Where(m => m.R1_Result == player.R1_Result).Where(m => m.R2_Result == player.R2_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.R3_Result = 1).ToList();
                                }
                                else if (player.R3_Vs_User == 9)
                                {
                                    player.R3_Result = 1;
                                    PlayerList.Where(m => m.R1_Result == player.R1_Result).Where(m => m.R2_Result == player.R2_Result).Where(m => m.Tournament_No != player.Tournament_No).Select(m => m.R3_Result = 0).ToList();
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

            Application.Current.Properties["TempData"] = PlayerList;
        }

        public List<MatchInfo> MatchButtonCreate()
        {
            List<int> usedList = new List<int>();
            List<MatchInfo> matchList = new List<MatchInfo>();

            foreach (var player in PlayerList)
            {
                if (usedList.Contains(player.User_Id))
                {
                    continue;
                }

                MatchInfo match = new MatchInfo();
                match.PlayerA_ID = player.User_Id;
                match.PlayerA_Name = userList.Where(m => m.User_Id == match.PlayerA_ID).Select(m => m.User_Name).First();
                match.PlayerA_PickNo = player.Pick_No;
                usedList.Add(player.User_Id);

                for (int i = 3; i > 0; i--)
                {
                    var vs = typeof(DraftResults).GetProperty($"R{i}_Vs_User");
                    if (opponentCheck(player, i))
                    {
                        continue;
                    }
                    else if (matchFinishedCheck(player, i))
                    {
                        match.PlayerB_ID = (int)vs.GetValue(player);
                        match.PlayerB_Name = userList.Where(m => m.User_Id == match.PlayerB_ID).Select(m => m.User_Name).First();
                        match.buttonEnable = true;
                        match.CurrentRound = i;
                        if(i <3)
                        {
                            if(player.Tournament_No < 5)
                            {
                                match.ButtonColor = Color.SkyBlue;
                                match.TournamentGroup = 0;
                            }
                            else
                            {
                                match.ButtonColor = Color.Pink;
                                match.TournamentGroup = 1;
                            }
                        }
                        else
                        {
                            match.ButtonColor = Color.MediumPurple;
                        }

                        usedList.Add(match.PlayerB_ID);
                        matchList.Add(match);
                        break;
                    }
                    else
                    {
                        match.PlayerB_ID = (int)vs.GetValue(player);
                        match.PlayerB_Name = userList.Where(m => m.User_Id == match.PlayerB_ID).Select(m => m.User_Name).First();
                        match.buttonEnable = false;
                        match.CurrentRound = i;
                        if (i < 3)
                        {
                            if (player.Tournament_No < 5)
                            {
                                match.ButtonColor = Color.SkyBlue;
                                match.TournamentGroup = 0;
                            }
                            else
                            {
                                match.ButtonColor = Color.Pink;
                                match.TournamentGroup = 1;
                            }
                        }
                        usedList.Add(match.PlayerB_ID);
                        matchList.Add(match);
                        break;
                    }
                }
            }

            return matchList.Where(x => x.PlayerA_ID != x.PlayerB_ID).OrderBy(x=>x.TournamentGroup).ToList();
        }


        public void MatchResultUpdate(int setPlayer,int result)
        {
            var player = PlayerList.Where(m => m.User_Id == setPlayer).First();
            for (int i = 3; i > 0; i--)
            {
                var vs = typeof(DraftResults).GetProperty($"R{i}_Result");
                if (opponentCheck(player, i))
                {
                    continue;
                }

                vs.SetValue(player,result);
                break;
            }
            Application.Current.Properties["TempData"] = PlayerList;
        }

        private void TempDraftResultUpdate(string player, string column, string value)
        {
            var connectionModel = new ConnectionModel();
            using (var connection = connectionModel.CreateConnection())
            {
                SQLiteCommand command = connection.CreateCommand($"UPDATE DraftResults SET {column} = {value} WHERE User_Id = {player}");
                command.ExecuteNonQuery();
            }
        }

        private void MatchResultUpdateTable(string player,int result,string UpdateRound)
        {
            var connectionModel = new ConnectionModel();
            using (var connection = connectionModel.CreateConnection())
            {
                SQLiteCommand command = connection.CreateCommand($"UPDATE DraftResults SET {UpdateRound} = {result} WHERE User_Id = {player}");
                command.ExecuteNonQuery();
            }
        }

        private void MatchUserUpdateTable(string player1, string player2, int round)
        {
            var connectionModel = new ConnectionModel();
            using (var connection = connectionModel.CreateConnection())
            {
                SQLiteCommand command = connection.CreateCommand($"UPDATE DraftResults SET R{round}_Vs_User = {player1} WHERE User_Id = {player2}:UPDATE DraftResults SET R{round}_Vs_User = {player2} WHERE User_Id = {player1}");
                command.ExecuteNonQuery();
            }
        }

        private bool opponentCheck(DraftResults player,int round)
        {
            switch(round)
            {
                case 1:
                    return player.R1_Vs_User == null;
                case 2:
                    return player.R2_Vs_User == null;
                case 3:
                    return player.R3_Vs_User == null;
                default:
                    return false;
            }
        }

        private bool matchFinishedCheck(DraftResults player, int round)
        {
            switch (round)
            {
                case 1:
                    return player.R1_Result == null;
                case 2:
                    return player.R2_Result == null;
                case 3:
                    return player.R3_Result == null;
                default:
                    return false;
            }
        }


    }
}
