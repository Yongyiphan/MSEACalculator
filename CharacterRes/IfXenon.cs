using MSEACalculator.CharacterRes.EquipmentRes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes
{
    public class IfXenon
    {

        public List<string> XenonClassType { get; set; } =  new List<string>() { "Pirate", "Thief"};


        


        public EquipStatsCLS UpdateMainStat(EquipStatsCLS EquipStat, string ClassType, int Stat1, int Stat2)
        {
            switch (ClassType)
            {
                case "Pirate":
                    EquipStat.STR += Stat1;
                    EquipStat.DEX += Stat2;
                    break;
                case "Thief":
                    EquipStat.DEX += Stat1;
                    EquipStat.LUK += Stat2;
                    break;
            }
            return EquipStat;
        }


    }
}
