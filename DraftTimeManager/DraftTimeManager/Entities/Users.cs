using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftTimeManager.Entities
{
    [Table("Users")]
    public class Users
    {
        [PrimaryKey, AutoIncrement, Column("User_Id")]
        public int User_Id { get; set; }

        [NotNull, Column("User_Name")]
        public string User_Name { get; set; }

        [Column("DCI_Num")]
        public string DCI_Num { get; set; }

        [Column("Guest_Flg")]
        public bool Guest_Flg { get; set; }
    }
}
