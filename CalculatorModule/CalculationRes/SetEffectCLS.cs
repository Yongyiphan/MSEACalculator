using MSEACalculator.CharacterRes.EquipmentRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CalculationRes
{
    public class SetEffectCLS
    {


        public string EquipSet { get; set; } = string.Empty;
        public string ClassType { get; set; } = string.Empty;
        public int SetAt { get; set; } = 0;

        public EquipStatsCLS SetEffect = new EquipStatsCLS();
    }
}
