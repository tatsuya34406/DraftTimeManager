using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftTimeManager.Entities
{
    [Table("OpponentUserScore")]
    public class OpponentUserScore
    {
        [Indexed(Name = "OppoScoreIdx", Order = 1, Unique = true), Column("User_Id")]
        public string User_Id { get; set; }

        [Indexed(Name = "OppoScoreIdx", Order = 2, Unique = true), Column("Vs_User_Id")]
        public string Vs_User_Id { get; set; }

        [Column("Cnt_Win")]
        public int Cnt_Win { get; set; }

        [Column("Cnt_Lose")]
        public int Cnt_Lose { get; set; }
    }
}
