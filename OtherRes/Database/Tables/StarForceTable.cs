using Microsoft.Data.Sqlite;
using MSEACalculator.CalculationRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class StarForceTable : BaseDBTable, ITableUpload
    {

        public List<StarforceCLS> SFList { get; set; }

        
        private string[] sfTableSpec = { "(" +
                    "SFID int," +
                    "JobStat int," +
                    "NonWeapVDef int," +
                    "OverallVDef int," +
                    "CatAMaxHP int," +
                    "WeapMaxMP int," +
                    "WeapVATK int," +
                    "WeapVMATK int," +
                    "SJump int," +
                    "SSpeed int," +
                    "GloveVATK int, " +
                    "GloveVMATK int," +
                    "PRIMARY KEY (SFID)" +
                    ");" };

        private string[] addSFstatSpec = { "(" +
                "SFID int," +
                "LevelRank int," +
                "VStat int," +
                "NonWeapVATK int," +
                "NonWeapVMATK int," +
                "WeapVATK int," +
                "WeapVMATK int," +
                "PRIMARY KEY (SFID, LevelRank)" +
                ");" };

        private string[] SuperiorSFSpec = { "(" +
                    "SFID int," +
                    "LevelRank int," +
                    "VStat int," +
                    "WeapVAtk," +
                    "VDef," +
                    "PRIMARY KEY (SFID, LevelRank)" +
                    ");"};
        public StarForceTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            switch (TableName)
            {
                case "StarForceBaseData":
                    TableParameters = sfTableSpec[0];
                    break;
                case "StarForceAddData":
                    TableParameters = addSFstatSpec[0];
                    break;
                case "StarforceSuperiorData":
                    TableParameters = SuperiorSFSpec[0];
                    break;
            }
        }
        public async void RetrieveData()
        {
            switch (TableName)
            {
                case "StarForceBaseData":
                    SFList = await GetSFCSVAsync();
                    break;
                case "StarForceAddData":
                    SFList = await GetAddSFCSVAsync();
                    break;
                case "StarforceSuperiorData":
                    SFList = await GetSuperiorSFCSVAsync();
                    break;
            }
        }


        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {
                switch (TableName)
                {
                    case "StarForceBaseData":
                        string insertSF = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                        using (SqliteCommand insertCMD = new SqliteCommand(insertSF, connection, transaction))
                        {
                            foreach (StarforceCLS sF in SFList)
                            {
                                ErrorCounter++;
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@SFID", sF.SFLevel);
                                insertCMD.Parameters.AddWithValue("@JobStat", sF.JobStat);

                                insertCMD.Parameters.AddWithValue("@NonWeapVDef", sF.NonWeapVDef);
                                insertCMD.Parameters.AddWithValue("@OverallVDef", sF.OverallVDef);
                                
                                insertCMD.Parameters.AddWithValue("@CatAMaxHP", sF.CatAMaxHP);
                                insertCMD.Parameters.AddWithValue("@WeapMaxMP", sF.WeapMaxMP);
                                
                                insertCMD.Parameters.AddWithValue("@WeapVATK", sF.WeapVATK);
                                insertCMD.Parameters.AddWithValue("@WeapVMATK", sF.WeapVMATK);
                                
                                insertCMD.Parameters.AddWithValue("@SJump", sF.SJump);
                                insertCMD.Parameters.AddWithValue("@SSpeed", sF.SSpeed);
                                
                                insertCMD.Parameters.AddWithValue("GloveVATK", sF.GloveVATK);
                                insertCMD.Parameters.AddWithValue("@GloveVMATK", sF.GloveVMATK);

                                try
                                {
                                    insertCMD.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ErrorCounter.ToString());
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        break;
                    case "StarForceAddData":
                        string insertASF = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                        using (SqliteCommand insertCMD = new SqliteCommand(insertASF, connection, transaction))
                        {
                            foreach (StarforceCLS sF in SFList)
                            {   
                                ErrorCounter++;
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@SFID", sF.SFLevel);
                                insertCMD.Parameters.AddWithValue("@LevelRank", sF.LevelRank);
                                insertCMD.Parameters.AddWithValue("@VStat", sF.VStat);
                                insertCMD.Parameters.AddWithValue("@NonWeapVATK", sF.NonWeapATK);
                                insertCMD.Parameters.AddWithValue("@NonWeapVMATK", sF.NonWeapMATK);
                                insertCMD.Parameters.AddWithValue("@WeapVATK", sF.WeapVATK);
                                insertCMD.Parameters.AddWithValue("@WeapVMATK", sF.WeapVMATK);

                                try
                                {
                                    insertCMD.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ErrorCounter.ToString());
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        break;
                    case "StarforceSuperiorData":
                        string insertSSF = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                        using (SqliteCommand insertCMD = new SqliteCommand(insertSSF, connection, transaction))
                        {
                            foreach (StarforceCLS sF in SFList)
                            {   
                                ErrorCounter++;
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@SFID", sF.SFLevel);
                                insertCMD.Parameters.AddWithValue("@LevelRank", sF.LevelRank);
                                insertCMD.Parameters.AddWithValue("@VStat", sF.VStat);
                                insertCMD.Parameters.AddWithValue("@WeapVAtk", sF.WeapVATK);
                                insertCMD.Parameters.AddWithValue("@VDef", sF.VDef);
                            try
                                {
                                    insertCMD.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ErrorCounter.ToString());
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        break;
                }
            }
        }

        public static async Task<List<StarforceCLS>> GetSFCSVAsync()
        {
            List<StarforceCLS> SFList = new List<StarforceCLS>();

            StorageFile statTable = await GVar.storageFolder.GetFileAsync(GVar.CalculationsPath + "StarforceGains.csv");

            var stream = await statTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");

                    foreach (string SFEntry in result.Skip(1))
                    {
                        if (SFEntry == "")
                        {
                            return SFList;
                        }

                        var temp = SFEntry.Split(",");

                        StarforceCLS sFGain = new StarforceCLS();
                        sFGain.SFLevel = Convert.ToInt32(temp[1]);
                        sFGain.JobStat = Convert.ToInt32(temp[2]);
                        sFGain.NonWeapVDef = Convert.ToInt32(temp[3]);
                        sFGain.OverallVDef = Convert.ToInt32(temp[4]);
                        sFGain.CatAMaxHP = Convert.ToInt32(temp[5]);
                        sFGain.WeapMaxMP = Convert.ToInt32(temp[6]);
                        sFGain.WeapVATK = Convert.ToInt32(temp[7]);
                        sFGain.WeapVMATK = Convert.ToInt32(temp[8]);
                        sFGain.SJump = Convert.ToInt32(temp[9]);
                        sFGain.SSpeed = Convert.ToInt32(temp[10]);
                        sFGain.GloveVATK = Convert.ToInt32(temp[11]);
                        sFGain.GloveVMATK = Convert.ToInt32(temp[12]);

                        SFList.Add(sFGain);

                    }

                }
            }

            return SFList;
        }
        public static async Task<List<StarforceCLS>> GetAddSFCSVAsync()
        {
            List<StarforceCLS> SFList = new List<StarforceCLS>();

            StorageFile statTable = await GVar.storageFolder.GetFileAsync(GVar.CalculationsPath + "AddStarforceGains.csv");

            var stream = await statTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");

                    foreach (string SFEntry in result.Skip(1))
                    {
                        if (SFEntry == "")
                        {
                            return SFList;
                        }

                        var temp = SFEntry.Split(",");

                        StarforceCLS sFGain = new StarforceCLS();
                        sFGain.SFLevel = Convert.ToInt32(temp[1]);
                        sFGain.LevelRank = Convert.ToInt32(temp[2]);
                        sFGain.VStat = Convert.ToInt32(temp[3]);
                        sFGain.NonWeapATK = Convert.ToInt32(temp[4]);
                        sFGain.NonWeapMATK = Convert.ToInt32(temp[5]);
                        sFGain.WeapVATK = Convert.ToInt32(temp[6]);
                        sFGain.WeapVMATK = Convert.ToInt32(temp[7]);


                        SFList.Add(sFGain);

                    }

                }
            }

            return SFList;
        }
        public static async Task<List<StarforceCLS>> GetSuperiorSFCSVAsync()
        {
            List<StarforceCLS> SFList = new List<StarforceCLS>();

            StorageFile statTable = await GVar.storageFolder.GetFileAsync(GVar.CalculationsPath + "SuperiorStarforceGains.csv");

            var stream = await statTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");

                    foreach (string SFEntry in result.Skip(1))
                    {
                        if (SFEntry == "")
                        {
                            return SFList;
                        }

                        var temp = SFEntry.Split(",");

                        StarforceCLS sFGain = new StarforceCLS();
                        sFGain.SFLevel = Convert.ToInt32(temp[1]);
                        sFGain.LevelRank = Convert.ToInt32(temp[2]);
                        sFGain.VStat = Convert.ToInt32(temp[3]);
                        sFGain.WeapVATK = Convert.ToInt32(temp[4]);
                        sFGain.VDef = Convert.ToInt32(temp[5]);

                        SFList.Add(sFGain);
                    }
                }
            }
            return SFList;
        }

        public static List<StarforceCLS> GetAllStarforceDB()
        {
            List<StarforceCLS> result = new List<StarforceCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM StarForceBaseData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetInt32(0) == 16)
                            {
                                break;
                            }

                            StarforceCLS SF = new StarforceCLS();

                            SF.SFLevel     =  reader.GetInt32(0);
                            SF.JobStat     =  reader.GetInt32(1);
                            SF.NonWeapVDef =  reader.GetInt32(2);
                            SF.OverallVDef =  reader.GetInt32(3);
                            SF.CatAMaxHP   =  reader.GetInt32(4);
                            SF.WeapMaxMP   =  reader.GetInt32(5);
                            SF.WeapVATK    =  reader.GetInt32(6);
                            SF.WeapVMATK   =  reader.GetInt32(7);
                            SF.SJump       =  reader.GetInt32(8);
                            SF.SSpeed      =  reader.GetInt32(9);
                            SF.GloveVATK   = reader.GetInt32(10);
                            SF.GloveVMATK  = reader.GetInt32(11);

                            result.Add(SF);
                        }
                    }
                }
                string addQuery = "SELECT b.SFID, b.NonWeapVDef, b.OverallVDef, a.LevelRank, a.VStat, a.NonWeapVATK, a.NonWeapVMATK, a.WeapVATK, a.WeapVMATK " +
                    "FROM StarForceBaseData AS b " +
                    "INNER JOIN StarForceAddData as a ON " +
                    "b.SFID = a.SFID";

                using (SqliteCommand selectCMD = new SqliteCommand(addQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {

                        StarforceCLS SF = null;
                        bool startNew = false;
                        bool toAdd = false;
                        int prevLvl = 0;

                        while (reader.Read())
                        {
                            int clvl = reader.GetInt32(0);
                            if (prevLvl == 0)
                            {
                                prevLvl = clvl;
                                startNew = true;
                                goto StartNew;
                            }

                        StartNew:
                            if (startNew)
                            {
                                SF = new StarforceCLS();
                                SF.SFLevel = clvl;
                                SF.NonWeapVDef = reader.GetInt32(1);
                                SF.OverallVDef = reader.GetInt32(2);
                                startNew = false;
                            }


                            if (prevLvl !=  clvl)
                            {
                                prevLvl = clvl;
                                toAdd = true;
                                goto ToAdd;
                            }
                            else
                            {
                                int lvlrank = reader.GetInt32(3);
                                SF.LevelRank = lvlrank;
                                SF.VStatL.Add(lvlrank, reader.GetInt32(4));
                                SF.NonWeapVATKL.Add(lvlrank, reader.GetInt32(5));
                                SF.NonWeapVMATKL.Add(lvlrank, reader.GetInt32(6));
                                SF.WeapVATKL.Add(lvlrank, reader.GetInt32(7));
                                SF.WeapVMATKL.Add(lvlrank, reader.GetInt32(8));
                            }

                        ToAdd:
                            if (toAdd)
                            {
                                result.Add(SF);
                                toAdd = false;
                                startNew = true;
                                goto StartNew;
                            }
                        }

                        result.Add(SF);
                    }
                }


            }

            return result;
        }




    }
}
