using Microsoft.Data.Sqlite;
using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class ClassCharacterTable : BaseDBTable, ITableUpload
    {
        List<CharacterCLS> CharacterList { get; set; }


        private bool HaveTrack = false;


        public ClassCharacterTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
        }

        public async void RetrieveData()
        {
            var Result = await GetCharCSVAsync("CharacterData.csv", "PRIMARY KEY (ClassName)");

            CharacterList = Result.Item1;
            TableParameters = Result.Item2;

        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            int counter = 0;
            if (ComFunc.IsOpenConnection(connection))
            {
                counter++;
                string insertChar = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);

                using (SqliteCommand insertCMD = new SqliteCommand(insertChar, connection, transaction))
                {
                    insertCMD.Connection = connection;
                    insertCMD.Transaction = transaction;
                    foreach (CharacterCLS charItem in CharacterList)
                    {
                        insertCMD.Parameters.Clear();
                        insertCMD.CommandText = insertChar;
                        insertCMD.Parameters.AddWithValue("@ClassName", charItem.ClassName);
                        insertCMD.Parameters.AddWithValue("@ClassType", charItem.ClassType);
                        insertCMD.Parameters.AddWithValue("@Faction", charItem.Faction);

                        insertCMD.Parameters.AddWithValue("@MainStat", charItem.MainStat);
                        insertCMD.Parameters.AddWithValue("@SecStat", charItem.SecStat);

                        insertCMD.Parameters.AddWithValue("@UEffect", charItem.UnionEffect);
                        insertCMD.Parameters.AddWithValue("@UEffectType", charItem.UnionEffectType);

                        try
                        {
                            insertCMD.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            string updateQuery = "UPDATE " + TableName + " " +
                                "SET " +
                                "ClassType = @ClassType, " +
                                "Faction = @Faction, " +
                                "MainStat = @MainStat, " +
                                "SecStat = @SecStat, " +
                                "UEffect = @UEffect, " +
                                "UEffectType = @UEffectType" +
                                "WHERE ClassName = @ClassName";
                            insertCMD.CommandText = updateQuery;
                            insertCMD.ExecuteNonQuery();

                            Console.WriteLine(counter.ToString());
                            Console.WriteLine(ex.ToString());
                        }
                    };
                }

            }
        }

        public static async Task<(List<CharacterCLS>, string)> GetCharCSVAsync(string FileName, string TableKey)
        {
            List<CharacterCLS> characterList = new List<CharacterCLS>();

            List<string> result = await ComFunc.CSVStringAsync(GVar.CharacterPath, FileName);
            int counter = 0;
            string tableSpec = "";


            foreach (string characterItem in result)
            {
                if (characterItem == "")
                {
                    return (characterList, tableSpec);
                }
                if (counter == 0)
                {
                    tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.CharacterCN, characterItem, TableKey);
                    counter += 1;
                    continue;
                }

                var temp = characterItem.Split(",");
                CharacterCLS tempChar = new CharacterCLS();
                tempChar.ClassName = temp[1];
                tempChar.Faction = temp[2];
                tempChar.ClassType = temp[3];
                tempChar.MainStat = temp[4];
                tempChar.SecStat = temp[5];
                tempChar.UnionEffect = temp[6];
                tempChar.UnionEffectType = temp[7];

                characterList.Add(tempChar);
                counter += 1;
            }
            return (characterList, tableSpec);
        }

        public override void InitTable(SqliteConnection connection, SqliteTransaction transaction)
        {

            Dictionary<string, CharacterCLS> CharTrackDB = DBRetrieve.GetAllCharTrackDB();
            HaveTrack = CharTrackDB.Keys.Count > 0 ? true : false;

            if (!HaveTrack)
            {
                base.InitTable(connection, transaction);
            }
        }
        public static Dictionary<string, CharacterCLS> GetAllCharDB()
        {
            Dictionary<string, CharacterCLS> charDict = new Dictionary<string, CharacterCLS>();

            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbConnection.Open();
                string getCharCmd =
                    "SELECT AC.ClassName, AC.ClassType, AC.Faction, AC.MainStat, AC.SecStat, AC.UEffect, AC.UEffectType, ClassMainWeapon.Weapon, ClassSecWeapon.Secondary "+
                    "FROM ClassCharacterData AS AC " +
                    "INNER JOIN ClassMainWeapon ON AC.ClassName = ClassMainWeapon.ClassName " +
                    "INNER JOIN ClassSecWeapon ON AC.ClassName = ClassSecWeapon.ClassName";

                using (SqliteCommand selectCmd = new SqliteCommand(getCharCmd, dbConnection))
                {
                    using (SqliteDataReader query = selectCmd.ExecuteReader())
                    {
                        while (query.Read())
                        {
                            string ClassName = query.GetString(0);
                            string MainWeapon = query.GetString(7);
                            string SecWeapon = query.GetString(8);
                            if (charDict.ContainsKey(query.GetString(0)))
                            {
                                charDict[ClassName].MainWeapon.Add(MainWeapon);
                                charDict[ClassName].SecondaryWeapon.Add(SecWeapon);
                                continue;
                            }

                            CharacterCLS tempChar = new CharacterCLS();
                            tempChar.ClassName = ClassName;
                            tempChar.ClassType = query.GetString(1);
                            tempChar.Faction = query.GetString(2);
                            tempChar.MainStat = query.GetString(3);
                            tempChar.SecStat = query.GetString(4);
                            tempChar.UnionEffect = query.GetString(5);
                            tempChar.UnionEffectType = query.GetString(6);

                            tempChar.MainWeapon = new List<string> { MainWeapon };
                            tempChar.SecondaryWeapon = new List<string> { SecWeapon };

                            charDict.Add(ClassName, tempChar);
                        }
                    }
                }
            }
            return charDict;
        }

    }
}
