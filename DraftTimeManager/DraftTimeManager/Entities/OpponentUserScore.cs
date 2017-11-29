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
        [PrimaryKey, Column("User_Id")]
        public string User_Id { get; set; }

        [PrimaryKey, Column("Vs_User_Id")]
        public string Vs_User_Id { get; set; }

        [Column("Cnt_Win")]
        public int Cnt_Win { get; set; }

        [Column("Cnt_Lose")]
        public int Cnt_Lose { get; set; }
    }
}
