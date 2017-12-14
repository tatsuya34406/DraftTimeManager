using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftTimeManager.Entities
{
    [Table("Settings")]
    public class Settings
    {
        [NotNull, Column("Volume")]
        public int Volume { get; set; }

        [NotNull, Column("Pick_Interval")]
        public int Pick_Interval { get; set; }

        [NotNull, Column("Picks")]
        public int Picks { get; set; }
    }
}
