using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftTimeManager.Models
{
    public class DraftHistoryPlayerDetailModel
    {
        public PlayerInfo PlayerInfo { get; set; }

        public DraftHistoryPlayerDetailModel(PlayerInfo playerInfo)
        {
            PlayerInfo = playerInfo;
        }
    }
}
