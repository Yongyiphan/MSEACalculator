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
    public class Character
    {

        

        public string ClassName { get; set; }

        public string Faction { get; set; }

        public string ClassType { get; set; }

        public List<Boss> bossList { get; set; } = new List<Boss>();

        public string unionEffect { get; set; }

        public string unionEffectType { get; set; }

        public string unionRank { get; set; }

        public int level { get; set; }

        public string MainStat { get; set; }
        public string SecStat { get; set; }

        public List<string> MainWeapon { get; set; }
        public List<string> SecondaryWeapon { get; set; }

        public string CurrentMainWeapon { get; set; }
        public string CurrentSecondaryWeapon { get; set; }

        public int Starforce { get; set; }

        //EACH CHAR HAS OWN SET OF EQUIPMENT LIST
        //EACH EQUIPMENT IN LIST HAS OWN BASE / FLAME / SCROLL STATS
        //:. 
        List<EquipModel> EquipmentList { get; set; } = new List<EquipModel>();

        public Character() { }

        //FOR RETREIVING W/O UNION
        public Character(string cn, string ct, string faction)
        {
            this.ClassName = cn;
            this.ClassType = ct;
            this.Faction = faction;

        }



        //FOR INIT
        public Character(string CN, string CT, string Faction, string uEffect, string uEffectType, string MS, string SS)
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
        public Character(string CN, string uRank, int level, int sf)
        {
            this.ClassName = CN;
            this.unionRank = uRank;
            this.level = level;
            this.Starforce = sf;
        }

        //FOR BOSSING
        public Character(string CN,  List<Boss> bossList)
        {
            this.ClassName = CN;

            this.bossList = bossList;
        }


        
        
    }
}
