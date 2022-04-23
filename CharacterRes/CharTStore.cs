using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes
{
    public class CharTStore : EventArgs
    {
        
        public CharTStore (CharacterCLS character)
        {
            //if (CurrentCharacter != null) { CurrentCharacter = character; }
            CurrentCharacter = character;
        }

        public CharacterCLS CurrentCharacter { get; set; }
    }
}
