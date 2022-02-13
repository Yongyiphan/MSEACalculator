using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.CharacterRes.MesoRes;


namespace MSEACalculator.CharacterRes
{
    public class CharacterCLS
    {
        //BASIC INFORMATION BEGIN
        public string ClassName { get; set; }
        public string Faction { get; set; }
        public string ClassType { get; set; }
        public int Level { get; set; } = 1;
        public string MainStat { get; set; }
        public string SecStat { get; set; }
        //POSSIBLE MAIN WEAPONS
        public List<string> MainWeapon { get; set; }
        //POSSIBLE SECONDARY WEAPONS
        public List<string> SecondaryWeapon { get; set; }
        //BASIC INFORMATION END

        //UNION INFORMATION BEGIN
        public string UnionEffect { get; set; }

        public string UnionEffectType { get; set; }

        public string UnionRank { get; set; }
        //UNION INFORMATION END


        //BOSSING INFORMATION BEGIN
        public List<BossCLS> BossingList { get; set; } = new List<BossCLS>();
        //BOSSING INFORMATION END



        //EQUIPMENT INFORMATION BEGIN
        public string CurrentMainWeapon { get; set; }
        public string CurrentSecondaryWeapon { get; set; }
        public int Starforce { get; set; }

        //EACH CHAR HAS OWN SET OF EQUIPMENT LIST
        //EACH EQUIPMENT IN LIST HAS OWN BASE / FLAME / SCROLL STATS
        //:. 
        List<EquipCLS> EquipmentList { get; set; } = new List<EquipCLS>();

        //EQUIPMENT INFORMATION END
        public CharacterCLS() { }



        
        
    }
}
