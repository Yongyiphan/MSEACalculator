using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes;

namespace MSEACalculator.CharacterRes.MesoRes
{
    public class BossMeso :INPCObject
    {

        public Dictionary<int, Boss> bossDict { get { return DatabaseAccess.GetBossDB(); } }
        public Dictionary<string, Character> charDict { get { return DatabaseAccess.GetAllCharTrackDB(); } }

        public List<string> bossNameList { get; set; } = new List<string>();
        public List<string> charNameList { get; set; } = new List<string>();

        public List<int> daysList { get; set; } = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
        public List<string> displayType { get; set; } = new List<string>() { "Daily", "Weekly", "Monthly" };

        private int dayMultipler;
        public int DayMultiplier { get { return dayMultipler = daysList[0]; } set { dayMultipler = value; } }

        private string mesoViewBy;
        public string MesoViewBy { get { return mesoViewBy = displayType[1]; } set { mesoViewBy = value; } }

        public BossMeso()
        {
            InitLoadFields();
        }


        public void InitLoadFields()
        {
            foreach (Boss bossItem in bossDict.Values)
            {

                if (!bossNameList.Contains(bossItem.name))
                {
                    bossNameList.Add(bossItem.name);
                };
            }
            foreach (Character charItem in charDict.Values)
            {
                charNameList.Add(charItem.ClassName);
            }

        }

    }
}
