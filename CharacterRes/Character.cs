using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes.MesoRes;


namespace MSEACalculator.CharacterRes
{
    public class Character
    {

        

        public string className { get; set; }

        public string faction { get; set; }

        public string classType { get; set; }

        public List<Boss> bossList { get; set; } = new List<Boss>();

        public string unionEffect { get; set; }

        public string unionEffectType { get; set; }

        public string unionRank { get; set; }

        public int level { get; set; }

        public Character() { }

        //FOR RETREIVING W/O UNION
        public Character(string cn, string ct, string faction)
        {
            this.className = cn;
            this.classType = ct;
            this.faction = faction;

        }


        //FOR INIT
        public Character(string CN, string CT, string Faction, string uEffect, string uEffectType)
        {
            this.className = CN;
            this.faction = Faction;
            this.classType = CT;
            this.unionEffect = uEffect;
            this.unionEffectType = uEffectType;
        }
        
        
        //FOR TRACKING CHARACTER
        public Character(string CN, string CT, string Faction, string uEffect, string uEffectType, string uRank, int level)
        {
            this.className = CN;
            this.faction = Faction;
            this.classType = CT;
            this.unionEffect = uEffect;
            this.unionEffectType = uEffectType;
            this.unionRank = uRank;
            this.level = level;
        }

        //FOR BOSSING
        public Character(string CN,  List<Boss> bossList)
        {
            this.className = CN;

            this.bossList = bossList;
        }


        
        
    }
}
