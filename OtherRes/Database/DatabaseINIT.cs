using Dasync.Collections;
using Microsoft.Data.Sqlite;
using MSEACalculator.OtherRes.Database.Tables;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;


namespace MSEACalculator.OtherRes.Database
{
    public class DatabaseINIT
    {

        //Codes for First initialising of Data
        //From CSV (DefaultData folder) to Sqlite Database (Maplestory.db)
        public async static Task DBInit()
        {
            //string dbName = "Maplestory.db";
            await ApplicationData.Current.LocalFolder.CreateFileAsync("Maplestory.db", CreationCollisionOption.OpenIfExists);



            List<BaseDBTable> StaticTable = new List<BaseDBTable>();

            /////CHARACTER///////

            //TABLE FOR DEFAULT CHARACTER LIST
            
            StaticTable.Add(new AllCharacterTable("AllCharacterData"));

            //TABLE FOR UNION EFFECTS

            StaticTable.Add(new UnionTable("UnionEffects"));


            //TABLE FOR CLASS WEAP ASSOC

            //****SPECIAL IMPLEMENTATION 2 IN ONE TABLE CLASS****

            StaticTable.Add(new ClassWeaponTable("ClassMainWeapon"));
            StaticTable.Add(new ClassWeaponTable("ClassSecWeapon"));


            /////////BOSSING///////
            //StaticTable.Add(new BossListTable("BossListData"));





            //////EVENT///////

            ///////INVENTORY////////

            //TABLE FOR EQUIP SLOT AND TYPES

            StaticTable.Add(new EquipSlotTable("EquipSlot"));

            //TABLE FOR ALL EQUIPMENTS
            List<string> EquipTableNames = new List<string> { "EquipArmorData", "EquipAccessoriesData", "EquipAndroidData", "EquipMedalData", "EquipWeaponData", "EquipSecondaryData" };
            foreach (string Names in EquipTableNames)
            {
                StaticTable.Add(new EquipmentTable(Names));
            }

            StaticTable.Add(new EquipmentSetEffectsTable("SetEffectAt"));
            StaticTable.Add(new EquipmentSetEffectsTable("SetEffectCul"));

            //EQUIPMENT SET EFFECT

            /////CALCULATIONS/////

            //SPECIAL IMPLEMENTATION FOR STARFORCE DATA 2 IN 1
            //TABLE FOR STARFORCE STATS
            //StaticTable.Add(new StarForceTable("StarForceBaseData"));

            //StaticTable.Add(new StarForceTable("StarForceAddData"));

            //StaticTable.Add(new StarForceTable("StarforceSuperiorData"));

            ////ARCANE SYMBOL

            StaticTable.Add(new SymbolsTable("ArcaneSymbolData"));


            StaticTable.Add(new PotentialTable("PotentialMainData"));
            StaticTable.Add(new PotentialTable("PotentialMainCube"));
            StaticTable.Add(new PotentialTable("PotentialBonusData"));
            StaticTable.Add(new PotentialTable("PotentialBonusCube"));


            await Task.WhenAll(StaticTable.ParallelForEachAsync(ST => Task.Run(() =>((ITableUpload)ST).RetrieveData())));


            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbConnection.Open();
                using (var trans = dbConnection.BeginTransaction())
                {
                    try
                    {

                        foreach(BaseDBTable table in StaticTable)
                        {
                            table.InitTable(dbConnection, trans);
                            ((ITableUpload)table).UploadTable(dbConnection, trans);

                        }
                        
                        //staticTables.ForEach(x => initTable(x.tableName, x.tableSpecs, dbConnection, trans));

                        //await Task.WhenAll(staticTables.ParallelForEachAsync(item => Task.Run(() => InitCSVData(item.initMode, item.tableName, dbConnection, trans))));

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        trans.Rollback();
                    }
                }
            }
        }

        public async static Task BlankTablesInit()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("Maplestory.db", CreationCollisionOption.OpenIfExists);

            List<BaseDBTable> BlankTables = new List<BaseDBTable>();

            //TABLE FOR CHARACTER TO TRACK
            string charTrackSpec = "(" +
                "CharName string," +
                "UnionRank string," +
                "Level int," +
                "Starforce int," +
                "PRIMARY KEY (CharName)," +
                "FOREIGN KEY (CharName) REFERENCES AllCharacterData(ClassName) ON UPDATE CASCADE ON DELETE CASCADE" +
                ");";
            BlankTables.Add(new BaseDBTable("TrackCharacter", charTrackSpec));

            //TABLE FOR CHAR'S BOSS TRACKING
            string bossMesoGainsTableSpec = "(" +
                "CharName string," +
                "BossName string," +
                "BossID int," +
                "PRIMARY KEY (CharName, BossID)," +
                "FOREIGN KEY (CharName) REFERENCES TrackCharacter(CharName) ON UPDATE CASCADE ON DELETE CASCADE," +
                "FOREIGN KEY (BossID) REFERENCES BossListData(BossID) ON UPDATE CASCADE ON DELETE CASCADE" +
                ");";
            BlankTables.Add(new BaseDBTable("TrackBossMesoGains", bossMesoGainsTableSpec));

            string[] EQueryStartEnd = { "(", ");" };
            string[] TEquipKey = {
                    "CharName string," +
                    "ClassType string," +
                    "EquipSlot string," +
                    "EquipSet string,"
            };
            
                       //ClassType, EquipSlot, EquipSet == ROWID
            string[] charEquipSpec = {
                "Starforce int," + 
                "PRIMARY KEY (CharName, EquipSlot)," +
                "FOREIGN KEY (CharName) REFERENCES TrackCharacter(CharName) ON DELETE CASCADE"
            };

            BlankTables.Add(new BaseDBTable("TrackCharEquip", String.Join("", EQueryStartEnd[0], TEquipKey[0], charEquipSpec[0], EQueryStartEnd[1])));

             string[] SubTableReferenceKey =
            {
                "PRIMARY KEY (CharName, EquipSlot)," +
                "FOREIGN KEY (CharName, EquipSlot) REFERENCES TrackCharEquip(CharName, EquipSlot) ON UPDATE CASCADE ON DELETE CASCADE"
            };

            string[] CharWeap = { "(" +
                "CharName string," +
                "MainWeapon string," +
                "SecWeapon string," +
                "PRIMARY KEY (CharName)," +
                "FOREIGN KEY (CharName) REFERENCES TrackCharacter(CharName) ON UPDATE CASCADE ON DELETE CASCADE" +
                ");" };
            BlankTables.Add(new BaseDBTable("TrackCharWeapons", CharWeap[0]));


            string[] charEquipScrollSpec = {
                "SlotCount int," +
                "ScrollPerc int," +
                "STR int," +
                "DEX int," +
                "INT int," +
                "LUK int," +
                "HP int," +
                "MP int," +
                "DEF int," +
                "ATK int," +
                "MATK int," +
                "SPD int," +
                "JUMP int,"
              };
            BlankTables.Add(new BaseDBTable("TrackCharEquipScroll", string.Join("", EQueryStartEnd[0], TEquipKey[0], charEquipScrollSpec[0],SubTableReferenceKey[0], EQueryStartEnd[1])));

            string[] charEquipFlameSpec = {
                "STR int," +
                "DEX int," +
                "INT int," +
                "LUK int," +
                "HP int," +
                "MP int," +
                "DEF int," +
                "ATK int," +
                "MATK int," +
                "SPD int," +
                "JUMP int," +
                "AllStat int," +
                "BossDMG int," +
                "DMG int,"
            };
            BlankTables.Add(new BaseDBTable("TrackCharEquipFlame", string.Join("", EQueryStartEnd[0], TEquipKey[0], charEquipFlameSpec[0],SubTableReferenceKey[0], EQueryStartEnd[1])));

            //EquipSet == WeaponType if recording Weapon
            string[] CharEquipPot = {
                "MGrade int," +
                "MFirstID int," +
                "MSecondID int," +
                "MThirdID int," +
                "AGrade int," +
                "AFirstID int," +
                "ASecondID int," +
                "AThirdID int,"
               };
            BlankTables.Add(new BaseDBTable("TrackCharEquipPot", string.Join("", EQueryStartEnd[0], TEquipKey[0], CharEquipPot[0], SubTableReferenceKey[0], EQueryStartEnd[1])));

            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbConnection.Open();
                using (var trans = dbConnection.BeginTransaction())
                {
                    try
                    {
                        foreach(BaseDBTable table in BlankTables)
                        {
                            table.InitTable(dbConnection, trans);
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        trans.Rollback();
                    }

                }


            }
        }


        //public static bool testDBCon()
        //{
        //    using(SqliteConnection dbConnection = new SqliteConnection($"Filename={GlobalVars.databasePath}"))
        //    {
        //        dbConnection.Open();
        //        return true;
        //    }

        //    return false;
        //}


    }

    
}



