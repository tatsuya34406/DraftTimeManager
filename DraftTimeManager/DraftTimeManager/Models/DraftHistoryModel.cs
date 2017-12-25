using DraftTimeManager.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftTimeManager.Models
{
    public class DraftHistoryModel
    {
        public ObservableCollection<GroupedHistory> Histories;

        public class GroupedHistory
        {
            public int Draft_Id { get; set; }

            public DateTime Draft_Date { get; set; }

            public string Draft_Date_Human
            {
                get
                {
                    return Draft_Date.ToString("yyyy/M/d H:m");
                }
            }
        }

        public DraftHistoryModel()
        {
            Histories = new ObservableCollection<GroupedHistory>();
        }

        public async void InitializeHistories()
        {
            Histories.Clear();

            using (var connection = await new ConnectionModel().CreateConnectionAsync())
            {
                var groupedHistory = connection.Table<DraftResults>()
                    .GroupBy(x => x.Draft_Id)
                    .Select(g =>
                        new GroupedHistory() {
                            Draft_Id = g.Key,
                            Draft_Date = g.Min(x => x.Draft_Date)
                        }
                    );

                foreach (var result in groupedHistory)
                {
                    Histories.Add(result);
                }
            }
        }
    }
}
