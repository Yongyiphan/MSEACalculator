using MSEACalculator.CharacterRes.EquipmentRes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes
{
    public class IfXenon
    {

        public List<string> XenonClassType { get; set; } =  new List<string>() { "Pirate", "Thief"};





        public List<string> ReturnJobStatEquip(string SetName)
        {
            List<string> results = null;
            if (SetName.Contains("Pirate"))
            {
                results = new List<string>() { "STR", "DEX"};
            }
            else
            {
                results = new List<string>() { "DEX", "LUK"};
            } 

            return results;
        }

    }
}
