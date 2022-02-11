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

        public string ClassName { get; set; }

        public string Faction { get; set; }

        public string ClassType { get; set; }

        public List<BossCLS> bossList { get; set; } = new List<BossCLS>();

        public string unionEffect { get; set; }

        public string unionEffectType { get; set; }

        public string unionRank { get; set; }

        public int level { get; set; }

        public string MainStat { get; set; }
        public string SecStat { get; set; }

        public List<string> MainWeapon { get; set; }
        public List<string> SecondaryWeapon { get; set; }

        public string CurrentMainWeapon { get; set; }

        public int Starforce { get; set; }

        //EACH CHAR HAS OWN SET OF EQUIPMENT LIST
        //EACH EQUIPMENT IN LIST HAS OWN BASE / FLAME / SCROLL STATS
        //:. 
        List<EquipCLS> EquipmentList { get; set; } = new List<EquipCLS>();

        public CharacterCLS() { }

        //FOR RETREIVING W/O UNION
        public CharacterCLS(string cn, string ct, string faction)
        {
            this.ClassName = cn;
            this.ClassType = ct;
            this.Faction = faction;

        }



        //FOR INIT
        public CharacterCLS(string CN, string CT, string Faction, string uEffect, string uEffectType, string MS, string SS)
        {
            this.ClassName = CN;
            this.Faction = Faction;
            this.ClassType = CT;
            this.unionEffect = uEffect;
            this.unionEffectType = uEffectType;
            this.MainStat = MS;
            this.SecStat = SS;
        }
        
        
        //FOR TRACKING CHARACTER
        public CharacterCLS(string CN, string uRank, int level, int sf)
        {
            this.ClassName = CN;
            this.unionRank = uRank;
            this.level = level;
            this.Starforce = sf;
        }

        //FOR BOSSING
        public CharacterCLS(string CN,  List<BossCLS> bossList)
        {
            this.ClassName = CN;

            this.bossList = bossList;
        }


        
        
    }
}
