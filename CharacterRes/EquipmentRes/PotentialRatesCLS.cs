using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class PotentialRatesCLS
    {
        public int MinLvl { get; set; } = 0;
        public int MaxLvl { get; set; } = 300;
        public double Initial { get; set; }
        public double GameCube { get; set; }
        public double CashCube { get; set; }
        public double Probability { get; set; }

        public PotentialRatesCLS()
        {

        }
    }

}
