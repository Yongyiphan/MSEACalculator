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
    public class AllCharacterTable : BaseDBTable , ITableUpload
    {
        List<CharacterCLS> CharacterList { get; set; }

        private bool HaveTrack = false;

        private string[] CharacterTableSpec = { "(" +
                            "ClassName string," +
                            "ClassType string," +
                            "Faction string," +
                            "MainStat string," +
                            "SecStat string," +
                            "UnionE string," +
                            "UnionET string," +
                            "PRIMARY KEY (ClassName)" +
                            ");" };

        public AllCharacterTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            TableParameters = CharacterTableSpec[0];
        }

        public async void RetrieveData()
        {
            CharacterList = await GetCharCSVAsync();
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            int counter = 0;
            if(ComFunc.IsOpenConnection(connection))
            {
                counter++;
                string insertChar = ComFunc.InsertSQLStringBuilder(TableName,TableParameters);

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

                        insertCMD.Parameters.AddWithValue("@UnionE", charItem.UnionEffect);
                        insertCMD.Parameters.AddWithValue("@UnionET", charItem.UnionEffectType);

                        try
                        {
                            insertCMD.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            string updateQuery = "UPDATE " + TableName + "" +
                                "SET " +
                                "ClassType = @ClassType," +
                                "Faction = @Faction," +
                                "MainStat = @MainStat," +
                                "SecStat = @SecStat," +
                                "UnionE = @UnionE," +
                                "UnionET = @UnionET" +
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

        public static async Task<List<CharacterCLS>> GetCharCSVAsync()
        {
            List<CharacterCLS> characterList = new List<CharacterCLS>();


            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CharacterPath + "CharacterData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    int counter = 0;
                    foreach (string characterItem in result.Skip(1))
                    {
                        if (characterItem == "")
                        {
                            return characterList;
                        }

                        var temp = characterItem.Split(",");
                        counter += 1;
                        CharacterCLS tempChar = new CharacterCLS();
                        tempChar.ClassName = temp[1];
                        tempChar.Faction = temp[2];
                        tempChar.ClassType = temp[3];
                        tempChar.MainStat = temp[4];
                        tempChar.SecStat = temp[5];
                        tempChar.UnionEffect = temp[6];
                        tempChar.UnionEffectType = temp[7];

                        characterList.Add(tempChar);

                    }
                }
            }


            return characterList;
        }

        public static Dictionary<string, CharacterCLS> GetAllCharDB()
        {
            Dictionary<string, CharacterCLS> charDict = new Dictionary<string, CharacterCLS>();

            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbConnection.Open();
                string getCharCmd = "SELECT " +
                    "AC.ClassName, AC.ClassType, AC.Faction, AC.MainStat, AC.SecStat, AC.UnionE, AC.UnionET, ClassMainWeapon.WeaponType, ClassSecWeapon.WeaponType " +
                    "FROM AllCharacterData AS AC " +
                    "INNER JOIN ClassMainWeapon ON AC.ClassName = ClassMainWeapon.ClassName " +
                    "INNER JOIN ClassSecWeapon ON AC.ClassName = ClassSecWeapon.ClassName";

                using (SqliteCommand selectCmd = new SqliteCommand(getCharCmd, dbConnection))
                {
                    using (SqliteDataReader query = selectCmd.ExecuteReader())
                    {
                        while (query.Read())
                        {
                            if (charDict.ContainsKey(query.GetString(0)))
                            {
                                charDict[query.GetString(0)].MainWeapon.Add(query.GetString(7));
                                charDict[query.GetString(0)].SecondaryWeapon.Add(query.GetString(8));
                                continue;
                            }

                            CharacterCLS tempChar = new CharacterCLS();
                            tempChar.ClassName = query.GetString(0);
                            tempChar.ClassType = query.GetString(1);
                            tempChar.Faction = query.GetString(2);
                            tempChar.MainStat = query.GetString(3);
                            tempChar.SecStat = query.GetString(4);
                            tempChar.UnionEffect = query.GetString(5);
                            tempChar.UnionEffectType = query.GetString(6);

                            tempChar.MainWeapon = new List<string> { query.GetString(7) };
                            tempChar.SecondaryWeapon = new List<string> { query.GetString(8) };

                            charDict.Add(query.GetString(0), tempChar);
                        }
                    }
                }
            }
            return charDict;
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
    }
}
