using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.BossRes;

namespace MSEACalculator.CharacterRes.MesoRes
{
    class BossMesoList
    {

        public string charName { get; set; }

        public Dictionary<string, Boss> bossNameLDict { get; set; }


        public BossMesoList() { }

        public BossMesoList(string cName, Dictionary<string, Boss> bossList)
        {
            this.charName = cName;
            this.bossNameLDict = bossList;
        }

    }
}
