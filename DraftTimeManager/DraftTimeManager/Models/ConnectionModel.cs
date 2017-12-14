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
        public SQLiteConnection conn;
        public ConnectionModel()
        {
            var _conn = CreateConnection();
            _conn.Wait();

            conn = _conn.Result;
        }

        private async Task<SQLiteConnection> CreateConnection()
        {
            const string DatabaseFileName = "draftmanager.db3";
            // ルートフォルダを取得する
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            // DBファイルの存在チェックを行う
            var result = await rootFolder.CheckExistsAsync(DatabaseFileName);
            if (result == ExistenceCheckResult.NotFound)
            {
                // 存在しなかった場合、新たにDBファイルを作成しテーブルも併せて新規作成する
                IFile file = await rootFolder.CreateFileAsync(DatabaseFileName, CreationCollisionOption.ReplaceExisting);
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
                IFile file = await rootFolder.CreateFileAsync(DatabaseFileName, CreationCollisionOption.OpenIfExists);
                return new SQLiteConnection(file.Path);
            }
        }
    }
}
