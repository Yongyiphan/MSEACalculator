using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes.MesoRes;


namespace MSEACalculator.CharacterRes
{
    class Character
    {
        public string className { get; set; }

        public string faction { get; set; }

        public string classType { get; set; }

        public List<Boss> bossList { get; set; }

        public string unionEffect { get; set; }

        public string unionEffectType { get; set; }


        public Character() { }

        //FOR RETREIVING W/O UNION
        public Character(string cn, string ct, string faction)
        {
            this.className = cn;
            this.classType = ct;
            this.faction = faction;
        }

            //FOR INIT OR RETRIEVE ALL
            public Character(string CN, string CT, string Faction, string uEffect, string uEffectType)
        {
            this.className = CN;
            this.faction = Faction;
            this.classType = CT;
            this.unionEffect = uEffect;
            this.unionEffectType = uEffectType;
        }

        //FOR BOSSING
        public Character(string CN,  List<Boss> bossList)
        {
            this.className = CN;

            this.bossList = bossList;
        }


        
        
    }
}
