using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftTimeManager.Entities
{
    [Table("EnvironmentUserScore")]
    public class EnvironmentUserScore
    {
        [Indexed(Name = "EnvUserScoreIdx", Order = 1, Unique = true), Column("User_Id")]
        public string User_Id { get; set; }

        [Indexed(Name = "EnvUserScoreIdx", Order = 2, Unique = true), Column("Env_Id")]
        public string Env_Id { get; set; }

        [Column("Cnt_3_0")]
        public int Cnt_3_0 { get; set; }

        [Column("Cnt_2_1")]
        public int Cnt_2_1 { get; set; }

        [Column("Cnt_1_2")]
        public int Cnt_1_2 { get; set; }

        [Column("Cnt_0_3")]
        public int Cnt_0_3 { get; set; }

        [Column("Cnt_Win")]
        public int Cnt_Win { get; set; }

        [Column("Cnt_Lose")]
        public int Cnt_Lose { get; set; }
    }
}
