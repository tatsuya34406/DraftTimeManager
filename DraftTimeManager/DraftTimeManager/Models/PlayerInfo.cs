using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftTimeManager.Models
{
    public class PlayerInfo
    {
        static private readonly Dictionary<int, string> ResultDictionary = new Dictionary<int, string>()
        {
            { 0, "LOSE" },
            { 1, "WIN" },
            { 2, "DRAW" }
        };

        static private readonly Dictionary<int, string> ResultColorDictionary = new Dictionary<int, string>()
        {
            { 0, "Blue" },
            { 1, "Red" },
            { 2, "Gray" }
        };

        static private readonly string UndefinedResult = "UNDEFINED";

        static private readonly string UndefinedResultColor = "Gray";

        public int Player_Id { get; set; }

        public string Player_Name { get; set; }

        public string DCI_Num { get; set; }

        public int Rank { get; set; }

        public int? R1_Opponent_Id { get; set; }

        public string R1_Opponent_Name { get; set; }

        public int? R1_Result { get; set; }

        public string R1_Result_Human
        {
            get
            {
                return GetResultHuman(R1_Result);
            }
        }

        public string R1_Result_Color
        {
            get
            {
                return GetResultColor(R1_Result);
            }
        }

        public int? R2_Opponent_Id { get; set; }

        public string R2_Opponent_Name { get; set; }

        public int? R2_Result { get; set; }

        public string R2_Result_Human
        {
            get
            {
                return GetResultHuman(R2_Result);
            }
        }

        public string R2_Result_Color
        {
            get
            {
                return GetResultColor(R2_Result);
            }
        }

        public int? R3_Opponent_Id { get; set; }

        public string R3_Opponent_Name { get; set; }

        public int? R3_Result { get; set; }

        public string R3_Result_Human
        {
            get
            {
                return GetResultHuman(R3_Result);
            }
        }

        public string R3_Result_Color
        {
            get
            {
                return GetResultColor(R3_Result);
            }
        }

        private string GetResultHuman(int? result)
        {
            if (result.HasValue)
            {
                return PlayerInfo.ResultDictionary.Single(x => x.Key == result).Value;
            }
            else
            {
                return PlayerInfo.UndefinedResult;
            }
        }

        private string GetResultColor(int? result)
        {
            if (result.HasValue)
            {
                return PlayerInfo.ResultColorDictionary.Single(x => x.Key == result).Value;
            }
            else
            {
                return PlayerInfo.UndefinedResultColor;
            }
        }
    }
}
