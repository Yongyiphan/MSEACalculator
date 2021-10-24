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

        //public List<string> charNameList { get { return getCharNameList(charList); } }
        //public List<string> difficultyList { get { return getDifficulty(bossList); } }
        //public List<string> bossNameList { get { return getBossNameList(bossList); } }


        public BossMeso()
        {
            
        }


        //private List<string> getCharNameList(Dictionary<string, Character> charList)
        //{

        //    var temp = new List<string>();

        //    foreach (Character charItem in charList.Values)
        //    {
        //        if (!temp.Contains(charItem.className))
        //        {
        //            temp.Add(charItem.className);
        //        }
        //    }


        //    return temp;
        //}
        //private List<string> getDifficulty(Dictionary<int, Boss> bossList)
        //{

        //    var temp = new List<string>();

        //    foreach (Boss bossItem in bossList.Values)
        //    {
        //        if (!difficultyList.Contains(bossItem.difficulty))
        //        {
        //            difficultyList.Add(bossItem.difficulty);
        //        };
        //    }
        //    return temp;
        //}
        
        //private List<string> getBossNameList(Dictionary<int, Boss> bossList)
        //{

        //    var temp = new List<string>();

        //    foreach (Boss bossItem in bossList.Values)
        //    {
        //        if (!difficultyList.Contains(bossItem.name))
        //        {
        //            difficultyList.Add(bossItem.name);
        //        };
        //    }
        //    return temp;
        //}

    }
}
