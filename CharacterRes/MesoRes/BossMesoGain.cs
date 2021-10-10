using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.BossRes;

namespace MSEACalculator.CharacterRes.MesoRes
{
    class BossMesoGain
    {
        public string charName { get; set; }




        public List<BossRes.Boss> bossList { get; set; }

        public BossMesoGain() { }
        public BossMesoGain(string charName, List<Boss> bossList) 
        {
            this.charName = charName;
            this.bossList = bossList;
        }


    }
}
