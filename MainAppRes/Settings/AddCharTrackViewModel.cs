using MSEACalculator.CharacterRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.MainAppRes.Settings
{
    public class AddCharTrackViewModel
    {
        public Dictionary<string, Character> allCharDict { get; set; } = DatabaseAccess.GetAllCharDB();
        public Dictionary<string, Character> CharTrackDict { get; set; } = DatabaseAccess.GetAllCharTrackDB();

        public List<string> allCharList { get; set; } 
        public List<string> allCharTrackList { get; set; } 

        public AddCharTrackViewModel()
        {
            initFields();
        }

        private void initFields()
        {
            allCharList = new List<string>();
            allCharTrackList = new List<string>();
            foreach(Character character in allCharDict.Values)
            {
                if (!allCharList.Contains(character.className))
                {
                    allCharList.Add(character.className);
                }
            }
            foreach(Character character in CharTrackDict.Values)
            {
                if (!allCharTrackList.Contains(character.className))
                {
                    allCharTrackList.Add(character.className);
                }
            }
        }


    }
}
