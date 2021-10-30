using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes;

namespace MSEACalculator.CharacterRes.MesoRes
{
    public class BossMeso 
    {

        public Dictionary<int, Boss> bossDict { get { return DatabaseAccess.GetBossDB(); } }
        public Dictionary<string, Character> charDict { get { return DatabaseAccess.GetAllCharDB(); } }

        public BossMeso()
        {
            
        }


    }
}
