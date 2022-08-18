using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.OtherRes.Database.Tables;
using MSEACalculator.OtherRes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CalculationRes
{
    public class EquipStatModel
    {
        
        public Dictionary<string, CharacterCLS> CharacterStore { get => ClassCharacterTable.GetAllCharDB(); }

        public Dictionary<string, List<EquipCLS>> EquipmentStore { get => EquipmentTable.GetEquipmentDB(); }

        public Dictionary<string, Dictionary<int, StarforceCLS>> StarforceStore { get => StarForceTable.GetAllStarforceDB(); }

        public Dictionary<string, Dictionary<(int, int), int>> SFLimits { get => StarForceTable.SFLimitsDB(); }

        public List<string> ClassType { get => GVar.ClassType; }

        public Dictionary<string, List<string>> ClassJob { get; set; }= new Dictionary<string, List<string>>() { { "None", new List<string>() }, {"Any", new List<string>() } };

        public List<string> ToDisplayCategory { get; set; } = new List<string>() { "Medal", "Ring"};

        public EquipStatModel()
        {
            CharToClass();
        }

        private void CharToClass()
        {
            foreach(CharacterCLS Char in CharacterStore.Values)
            {
                if (Char.ClassType == "SPECIAL")
                {
                    ClassJob.Add("Xenon", new List<string>() { Char.ClassName });
                    continue;
                }
                if (ClassJob.ContainsKey(Char.ClassType))
                {
                    ClassJob[Char.ClassType].Add(Char.ClassName);
                    continue;
                }
                ClassJob.Add(Char.ClassType, new List<string>() { Char.ClassName });
               
            }
        }
    }
}
