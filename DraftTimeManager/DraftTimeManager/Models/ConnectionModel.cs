using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using DraftTimeManager.Entities;

namespace DraftTimeManager.Models
{
    public class ConnectionModel
    {
        string DatabaseFileName { get; }
        public ConnectionModel()
        {
            DatabaseFileName = "draftmanager.db3";
        }

        public SQLiteConnection CreateConnection()
        {
            var conn = CreateConnectionAsync();
            conn.Wait();
            return conn.Result;
        }

        public async Task<SQLiteConnection> CreateConnectionAsync()
        {
            // ルートフォルダを取得する
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            // DBファイルの存在チェックを行う
            var result = await rootFolder.CheckExistsAsync(DatabaseFileName).ConfigureAwait(false);
            if (result == ExistenceCheckResult.NotFound)
            {
                // 存在しなかった場合、新たにDBファイルを作成しテーブルも併せて新規作成する
                IFile file = await rootFolder.CreateFileAsync(DatabaseFileName, CreationCollisionOption.ReplaceExisting).ConfigureAwait(false);
                var connection = new SQLiteConnection(file.Path);
                connection.CreateTable<Users>();
                connection.CreateTable<Environments>();
                connection.CreateTable<EnvironmentUserScore>();
                connection.CreateTable<OpponentUserScore>();
                connection.CreateTable<TempDraftResults>();
                connection.CreateTable<DraftResults>();
                connection.CreateTable<Settings>();
                return connection;
            }
            else
            {
                // 存在した場合、そのままコネクションを作成する
                IFile file = await rootFolder.CreateFileAsync(DatabaseFileName, CreationCollisionOption.OpenIfExists).ConfigureAwait(false);
                return new SQLiteConnection(file.Path);
            }
        }

        public async Task<Settings> GetSettings()
        {
            using (var connection = await CreateConnectionAsync())
            {
                var setting = connection.Table<Settings>().FirstOrDefault();

                return setting;
            }
        }

        #region initialize
        public async void DBInitialize()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            IFile new_file = await rootFolder.CreateFileAsync(DatabaseFileName, CreationCollisionOption.ReplaceExisting);
            using (var connection = new SQLiteConnection(new_file.Path))
            {
                try
                {
                    connection.BeginTransaction();

                    connection.CreateTable<Users>();
                    connection.CreateTable<Environments>();
                    connection.CreateTable<EnvironmentUserScore>();
                    connection.CreateTable<OpponentUserScore>();
                    connection.CreateTable<TempDraftResults>();
                    connection.CreateTable<DraftResults>();
                    connection.CreateTable<Settings>();

                    connection.InsertAll(InitialUsers());
                    connection.InsertAll(InitialEnvironments());
                    connection.Insert(InitialSettings());

                    connection.InsertAll(TestUsers());
                    connection.InsertAll(TestDraftResults());
                    connection.InsertAll(TestEnvironmentUserScore());
                    connection.InsertAll(TestOpponentUserScore());

                    connection.Commit();
                }
                catch
                {
                    connection.Rollback();
                }
            }
        }

        private List<Users> InitialUsers()
        {
            return Enumerable.Range(1, 8).Select(x => new Users
            {
                User_Id = x,
                User_Name = "Guest" + x,
                Guest_Flg = true,
                Delete_Flg = false
            }).ToList();
        }

        private List<Environments> InitialEnvironments()
        {
            return new List<Environments>()
            {
                new Environments() { Env_Id = 1, Env_Name = "KLD-KLD-KLD", Picks = 14, Default_Flg = true, Delete_Flg = false },
                new Environments() { Env_Id = 2, Env_Name = "KLD-AER-AER", Picks = 14, Default_Flg = true, Delete_Flg = false },
                new Environments() { Env_Id = 3, Env_Name = "AKH-AKH-AKH", Picks = 14, Default_Flg = true, Delete_Flg = false },
                new Environments() { Env_Id = 4, Env_Name = "AKH-HOU-HOU", Picks = 14, Default_Flg = true, Delete_Flg = false },
                new Environments() { Env_Id = 5, Env_Name = "XLN-XLN-XLN", Picks = 14, Default_Flg = true, Delete_Flg = false },
            };
        }

        private Settings InitialSettings()
        {
            return new Settings() { Volume = 50, Pick_Interval = 10, Picks = 14 };
        }

        private List<Users> TestUsers()
        {
            return new List<Users>()
            {
                new Users() { User_Name = "Tomohisa Itaya" },
                new Users() { User_Name = "Tatsuya Nakata" },
                new Users() { User_Name = "Takashi Takani" },
                new Users() { User_Name = "Reo Yoshimoto" },
                new Users() { User_Name = "Shinsuke Ojima" },
                new Users() { User_Name = "Ren Ishikawa" },
                new Users() { User_Name = "Hoge Huga", Delete_Flg = true },
                new Users() { User_Name = "Foo Bar", Delete_Flg = true },
            };
        }

        private List<DraftResults> TestDraftResults()
        {
            var now = DateTime.Now;
            return new List<DraftResults>()
            {
                new DraftResults() {
                    Draft_Id = 1, Env_Id = 5, User_Id = 9,
                    R1_Vs_User = 10, R1_Result = 1, R2_Vs_User = 11, R2_Result = 1, R3_Vs_User = 13, R3_Result = 1,
                    Rank = 1, Pick_No = 1, Tournament_No = 1, Draft_Date = now },
                new DraftResults() {
                    Draft_Id = 1, Env_Id = 5, User_Id = 10,
                    R1_Vs_User = 9, R1_Result = 0, R2_Vs_User = 12, R2_Result = 1, R3_Vs_User = 14, R3_Result = 1,
                    Rank = 3, Pick_No = 2, Tournament_No = 1, Draft_Date = now },
                new DraftResults() {
                    Draft_Id = 1, Env_Id = 5, User_Id = 11,
                    R1_Vs_User = 12, R1_Result = 1, R2_Vs_User = 9, R2_Result = 0, R3_Vs_User = 15, R3_Result = 1,
                    Rank = 5, Pick_No = 3, Tournament_No = 1, Draft_Date = now },
                new DraftResults() {
                    Draft_Id = 1, Env_Id = 5, User_Id = 12,
                    R1_Vs_User = 11, R1_Result = 0, R2_Vs_User = 10, R2_Result = 0, R3_Vs_User = 16, R3_Result = 1,
                    Rank = 7, Pick_No = 4, Tournament_No = 1, Draft_Date = now },
                new DraftResults() {
                    Draft_Id = 1, Env_Id = 5, User_Id = 13,
                    R1_Vs_User = 14, R1_Result = 1, R2_Vs_User = 15, R2_Result = 1, R3_Vs_User = 9, R3_Result = 0,
                    Rank = 2, Pick_No = 5, Tournament_No = 1, Draft_Date = now },
                new DraftResults() {
                    Draft_Id = 1, Env_Id = 5, User_Id = 14,
                    R1_Vs_User = 13, R1_Result = 0, R2_Vs_User = 16, R2_Result = 1, R3_Vs_User = 10, R3_Result = 0,
                    Rank = 4, Pick_No = 6, Tournament_No = 1, Draft_Date = now },
                new DraftResults() {
                    Draft_Id = 1, Env_Id = 5, User_Id = 15,
                    R1_Vs_User = 16, R1_Result = 1, R2_Vs_User = 13, R2_Result = 0, R3_Vs_User = 11, R3_Result = 0,
                    Rank = 6, Pick_No = 7, Tournament_No = 1, Draft_Date = now },
                new DraftResults() {
                    Draft_Id = 1, Env_Id = 5, User_Id = 16,
                    R1_Vs_User = 15, R1_Result = 0, R2_Vs_User = 14, R2_Result = 0, R3_Vs_User = 12, R3_Result = 0,
                    Rank = 8, Pick_No = 8, Tournament_No = 1, Draft_Date = now },
            };
        }

        private List<EnvironmentUserScore> TestEnvironmentUserScore()
        {
            return new List<EnvironmentUserScore>()
            {
                new EnvironmentUserScore() { Env_Id = 5, User_Id = 9,
                    Cnt_3_0 = 1, Cnt_2_1 = 0, Cnt_1_2 = 0, Cnt_0_3 = 0, Cnt_Win = 3, Cnt_Lose = 0 },
                new EnvironmentUserScore() { Env_Id = 5, User_Id = 10,
                    Cnt_3_0 = 0, Cnt_2_1 = 1, Cnt_1_2 = 0, Cnt_0_3 = 0, Cnt_Win = 2, Cnt_Lose = 1 },
            };
        }

        private List<OpponentUserScore> TestOpponentUserScore()
        {
            return new List<OpponentUserScore>()
            {
                new OpponentUserScore() { User_Id = 9, Vs_User_Id = 10, Cnt_Win = 1, Cnt_Lose = 0},
                new OpponentUserScore() { User_Id = 10, Vs_User_Id = 9, Cnt_Win = 0, Cnt_Lose = 1},
            };
        }

        #endregion initialize
    }
}
