using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes;
using MSEACalculator.OtherRes.Database;
using MSEACalculator.OtherRes.Database.Tables;

namespace MSEACalculator.CharacterRes.MesoRes
{
    public class BossMesoModel :INPCObject
    {

        public Dictionary<int, BossCLS> bossDict { get { return BossListTable.GetBossDB(); } }
        public Dictionary<string, CharacterCLS> charDict { get { return DBRetrieve.GetAllCharTrackDB(); } }

        public List<string> bossNameList { get; set; } = new List<string>();
        public List<string> charNameList { get; set; } = new List<string>();

        public List<int> daysList { get; set; } = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
        public List<string> displayType { get; set; } = new List<string>() { "Daily", "Weekly", "Monthly" };

        private int dayMultipler;
        public int DayMultiplier { get { return dayMultipler = daysList[0]; } set { dayMultipler = value; } }

        private string mesoViewBy;
        public string MesoViewBy { get { return mesoViewBy = displayType[1]; } set { mesoViewBy = value; } }

        public BossMesoModel()
        {
            InitLoadFields();
        }


        public void InitLoadFields()
        {
            foreach (BossCLS bossItem in bossDict.Values)
            {

                if (!bossNameList.Contains(bossItem.BossName))
                {
                    bossNameList.Add(bossItem.BossName);
                };
            }
            foreach (CharacterCLS charItem in charDict.Values)
            {
                charNameList.Add(charItem.ClassName);
            }

        }

    }
}
