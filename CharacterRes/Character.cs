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

        public List<string> bossList { get; set; }

        public Character() { }

        public Character(string CN, string CT, string Faction)
        {
            this.className = CN;
            this.faction = Faction;
            this.classType = CT;
        }
        
        public Character(string CN, string CT, string Faction, List<string> bossList)
        {
            this.className = CN;
            this.faction = Faction;
            this.classType = CT;

            this.bossList = bossList;
        }

    }
}
