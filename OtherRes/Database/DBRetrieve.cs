using Microsoft.Data.Sqlite;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.CharacterRes;
using MSEACalculator.CalculationRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database
{
    public class DBRetrieve
    {

        ///////Retrieving FULL Data from Maplestory.db


        
        public static Dictionary<string, CharacterCLS> GetAllCharTrackDB()
        {
            Dictionary<string, CharacterCLS> charDict = new Dictionary<string, CharacterCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string getCharCMD = "SELECT * FROM TrackCharacter";

                using (SqliteCommand selectCMD = new SqliteCommand(getCharCMD, dbCon))
                {
                    using (SqliteDataReader result = selectCMD.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            CharacterCLS tempChar = new CharacterCLS();
                            tempChar.ClassName = result.GetString(0);
                            tempChar.UnionRank = result.GetString(1);
                            tempChar.Level = result.GetInt32(2);
                            tempChar.Starforce = result.GetInt32(3);

                            charDict.Add(result.GetString(0), tempChar);
                        }
                    }
                }
            }
            return charDict;
        }

        
        

        
        
        
        

    }
}
