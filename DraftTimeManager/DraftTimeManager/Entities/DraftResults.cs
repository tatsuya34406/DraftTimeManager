using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftTimeManager.Entities
{
    [Table("DraftResults")]
    public class DraftResults
    {
        [Indexed(Name = "DraftResultIdx", Order = 1, Unique = true), Column("Draft_Id")]
        public int Draft_Id { get; set; }

        [Indexed(Name = "DraftResultIdx", Order = 2, Unique = true), Column("User_Id")]
        public int User_Id { get; set; }

        [Column("Env_Id")]
        public int Env_Id { get; set; }

        [Column("R1_Vs_User")]
        public int? R1_Vs_User { get; set; }

        [Column("R1_Result")]
        public int? R1_Result { get; set; }

        [Column("R2_Vs_User")]
        public int? R2_Vs_User { get; set; }

        [Column("R2_Result")]
        public int? R2_Result { get; set; }

        [Column("R3_Vs_User")]
        public int? R3_Vs_User { get; set; }

        [Column("R3_Result")]
        public int? R3_Result { get; set; }

        [Column("Rank")]
        public int Rank { get; set; }

        [Column("Tournament_No")]
        public int Tournament_No { get; set; }

        [Column("Pick_No")]
        public int Pick_No { get; set; }

        [Column("Draft_Date")]
        public DateTime Draft_Date { get; set; }
    }
}
