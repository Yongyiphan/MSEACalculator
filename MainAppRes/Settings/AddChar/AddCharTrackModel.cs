using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes.Database;
using MSEACalculator.OtherRes.Database.Tables;
using System.Collections.Generic;
using System.Linq;

namespace MSEACalculator.MainAppRes.Settings.AddChar
{
    public class ACTModel
    {
        

        public Dictionary<string, CharacterCLS> AllCharDB { get; set; } = AllCharacterTable.GetAllCharDB();
        public Dictionary<string, CharacterCLS> AllCharTrackDB { get; set; } = DBRetrieve.GetAllCharTrackDB();

        public List<CharacterCLS> AllCharList { get; set; } = new List<CharacterCLS>();

        public ACTModel()
        {

            consolAllCharWeapon();
            displayNonTrackChar();
            //AllCharList = AllCharDict.Values.ToList();
        }

        public void consolAllCharWeapon()
        {
            foreach (CharacterCLS character in AllCharDB.Values)
            {
                if ((character.MainWeapon.Count > 1 && character.SecondaryWeapon.Count > 1) | character.MainWeapon.Count > 1 | character.SecondaryWeapon.Count >1 )
                {
                    var temp1 = character.MainWeapon;
                    var temp2 = character.SecondaryWeapon;
                    character.MainWeapon = temp1.Distinct().ToList();
                    character.SecondaryWeapon = temp2.Distinct().ToList();
                }
                

            }
        }

        public void displayNonTrackChar()
        {

            List<string> trackedChar = AllCharTrackDB.Values.Select(x => x.ClassName).ToList();
            foreach(CharacterCLS character in AllCharDB.Values)
            {
                if (trackedChar.Contains(character.ClassName))
                {
                    continue;
                }
                AllCharList.Add(character);
            }
        }
    }
}
