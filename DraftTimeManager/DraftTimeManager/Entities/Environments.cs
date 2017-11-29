using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftTimeManager.Entities
{
    [Table("Environments")]
    public class Environments
    {
        [PrimaryKey, Column("Env_Id")]
        public string Env_Id { get; set; }

        [NotNull, Column("Env_Name")]
        public string Env_Name { get; set; }
    }
}
