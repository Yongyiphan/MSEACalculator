using Dasync.Collections;
using Microsoft.Data.Sqlite;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.OtherRes.Database.Tables;
using MSEACalculator.OtherRes.Interface;
using MSEACalculator.StarforceRes;
using MSEACalculator.UnionRes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            string[] CharacterTableSpec = { "(" +
                            "ClassName string," +
                            "ClassType string," +
                            "Faction string," +
                            "MainStat string," +
                            "SecStat string," +
                            "UnionE string," +
                            "UnionET string," +
                            "PRIMARY KEY (ClassName)" +
                            ");" };
            StaticTable.Add(new AllCharacterTable("AllCharacterData", CharacterTableSpec[0]));

            //TABLE FOR UNION EFFECTS
            string[] unionETableSpecs = { "(" +
                    "Effect string," +
                    "EffectType string," +
                    "B int," +
                    "A int," +
                    "S int," +
                    "SS int," +
                    "SSS int," +
                    "PRIMARY KEY(Effect, EffectType)" +
                    ");" };
            StaticTable.Add(new UnionTable("UnionEffects", unionETableSpecs[0]));


            //TABLE FOR CLASS WEAP ASSOC
            
            //****SPECIAL IMPLEMENTATION 2 IN ONE TABLE CLASS****
            string ClassWeaponSpec = "(" +
                "ClassName string," +
                "WeaponType string," +
                "PRIMARY KEY (ClassName, WeaponType)" +
                ");";
            StaticTable.Add(new ClassWeaponTable("ClassMainWeapon", ClassWeaponSpec));
            StaticTable.Add(new ClassWeaponTable("ClassSecWeapon", ClassWeaponSpec));





            ///////BOSSING///////


            string BossListSpec = "(" +
                "BossID int," +
                "BossName string NOT NULL," +
                "Difficulty string NOT NULL," +
                "EntryType string NOT NULL," +
                "Meso int NOT NULL," +
                "PRIMARY KEY(BossID)" +
                ");";
            StaticTable.Add(new BossListTable("BossListData", BossListSpec));





            //////EVENT///////

            ///////INVENTORY////////

            //TABLE FOR EQUIP SLOT AND TYPES
            string EquipSlotTableSpec = "(" +
                "EquipSlot string," +
                "EquipType string," +
                "PRIMARY KEY(EquipSlot)" +
                ");";
            StaticTable.Add(new EquipSlotTable("EquipSlot", EquipSlotTableSpec));

            //TABLE FOR ALL ARMOR
            string[] ArmorTableSpec = { "(" +
                    "EquipSet string," +
                    "ClassType string," +
                    "EquipSlot string," +
                    "EquipLevel int," +
                    "MainStat int," +
                    "SecStat int," +
                    "AllStat int," +
                    "HP int," +
                    "MP int," +
                    "DEF int," +
                    "ATK int," +
                    "MATK int," +
                    "SPD int," +
                    "JUMP int," +
                    "IED int," +
                    "PRIMARY KEY (EquipSet, ClassType, EquipSlot)" +
                    ");" };
            StaticTable.Add(new EquipArmorTable("ArmorData", ArmorTableSpec[0]));

            //TABLE FOR ACCESSORIES
            string[] AccessoriesTableSpec = { "(" +
                    "EquipName string," +
                    "EquipSet string," +
                    "ClassType  string," +
                    "EquipSlot string," +
                    "EquipLevel int," +
                    "MainStat int," +
                    "SecStat int," +
                    "AllStat int," +
                    "HP string," +
                    "MP string," +
                    "DEF int," +
                    "ATK int," +
                    "MATK int," +
                    "SPD int," +
                    "JUMP int," +
                    "IED int," +
                    "PRIMARY KEY (EquipName, EquipSet, ClassType, EquipSlot) " +
                    ");" };
            StaticTable.Add(new EquipAccessoriesTable("AccessoriesData", AccessoriesTableSpec[0]));

            //TABLE FOR WEAPON
            //MAIN WEAPON
            string[] MainWeapTableSpec = { "(" +
                    "EquipSet string," +
                    "WeaponType string," +
                    "EquipLevel int," +
                    "ATKSPD int," +
                    "MainStat int," +
                    "SecStat int," +
                    "HP int," +
                    "DEF int," +
                    "ATK int," +
                    "MATK int," +
                    "SPD int," +
                    "BDMG int," +
                    "IED int," +
                    "PRIMARY KEY (EquipSet, WeaponType) " +
                    ");" };
            StaticTable.Add(new EquipMainWeaponTable("WeaponData", MainWeapTableSpec[0]));

            //SECONDARY WEAPON
            string[] SecWeapTableSpec = { "(" +
                    "ClassName string," +
                    "EquipName string," +
                    "WeaponType string," +
                    "EquipLevel int," +
                    "MainStat int," +
                    "SecStat int," +
                    "ATK int," +
                    "MATK int," +
                    "AllStat int," +
                    "DEF int," +
                    "HP int," +
                    "MP int," +
                    "ATKSPD int," +
                    "PRIMARY KEY (ClassName, EquipName, WeaponType, EquipLevel) " +
                    ");" };
            StaticTable.Add(new EquipSecWeaponTable("SecondaryData", SecWeapTableSpec[0]));

            /////CALCULATIONS/////

            //SPECIAL IMPLEMENTATION FOR STARFORCE DATA 2 IN 1
            //TABLE FOR STARFORCE STATS
            string[] sfTableSpec = { "(" +
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
            StaticTable.Add(new StarForceTable("StarForceBaseData", sfTableSpec[0]));

            //TABLE FOR STARFORCE STATS ADDONS
            string[] addSFstatSpec = { "(" +
                    "SFID int," +
                    "LevelRank int," +
                    "VStat int," +
                    "NonWeapVATK int," +
                    "NonWeapVMATK int," +
                    "WeapVATK int," +
                    "WeapVMATK int," +
                    "PRIMARY KEY (SFID, LevelRank)" +
                    ");" };
            StaticTable.Add(new StarForceTable("StarForceAddData", addSFstatSpec[0]));

            ////ARCANE SYMBOL
            string[] ArcaneSymSpec = { "(" +
                        "Name string, " +
                        "SubMap string, " +
                        "CurrentLevel int," +
                        "CurrentExp int," +
                        "CurrentLimit int, " +
                        "BaseGain int," +
                        "PQGain int, " +
                        "PQGainLimit int, " +
                        "SymbolExchangeRate int," +
                        "CostLvlMod int," +
                        "CostMod int," +
                        "PRIMARY KEY(Name)" +
                        ");" };
            StaticTable.Add(new SymbolsTable("ArcaneSymbolData", ArcaneSymSpec[0]));

            string[] PotSpec = { "(" +
                        "EquipGrp string," +
                        "Grade string," +
                        "GradeT string," +
                        "StatT string," +
                        "Stat nvarchar," +
                        "MinLvl int," +
                        "MaxLvl int," +
                        "ValueI nvarchar," +
                        "Duration int," +
                        "Chance double," +
                        "PRIMARY KEY (EquipGrp, Grade, GradeT, Stat, MinLvl, MaxLvl, ValueI)" +
                        ");" };
            StaticTable.Add(new PotentialTable("PotentialData", PotSpec[0]));


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
                "PRIMARY KEY(CharName)" +
                ");";
            BlankTables.Add(new BaseDBTable("TrackCharacter", charTrackSpec));

            //TABLE FOR CHAR'S BOSS TRACKING
            string bossMesoGainsTableSpec = "(" +
                "CharName string," +
                "BossName string," +
                "BossID int," +
                "PRIMARY KEY(CharName, BossID)," +
                "FOREIGN KEY (CharName) REFERENCES CharacterTrack(CharName) ON DELETE CASCADE," +
                "FOREIGN KEY (BossID) REFERENCES BossList(BossID)" +
                ");";
            BlankTables.Add(new BaseDBTable("TrackBossMesoGains", bossMesoGainsTableSpec));

            string[] charEquipScrollSpec = {"(" +
                "CharName string," +
                "EquipSlot string," +
                "EquipSet string," +
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
                "PRIMARY KEY (CharName, EquipSlot, EquipSet)," +
                "FOREIGN KEY (CharName) REFERENCES CharacterTrack(CharName) ON DELETE CASCADE" +
                ");" };
            BlankTables.Add(new BaseDBTable("TrackCharEquipScroll", charEquipScrollSpec[0]));

            string[] charEquipFlameSpec = {"(" +
                "CharName string," +
                "EquipSlot string," +
                "EquipSet string," +
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
                "DMG int," +
                "PRIMARY KEY (CharName, EquipSlot, EquipSet)," +
                "FOREIGN KEY (CharName) REFERENCES CharacterTrack(CharName) ON DELETE CASCADE" +
                ");" };
            BlankTables.Add(new BaseDBTable("TrackCharEquipFlame", charEquipFlameSpec[0]));

            //EquipSet == WeaponType if recording Weapon
            string[] CharEquipPot = { "(" +
                    "CharName string," +
                    "EquipSlot string," +
                    "EqiuipSet string," +
                    "PotType string," +
                    "FirstID int," +
                    "SecondID int," +
                    "ThirdID int" +
                    ");" };
            BlankTables.Add(new BaseDBTable("TrackCharEquipPot", CharEquipPot[0]));


            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbConnection.Open();
                using (var trans = dbConnection.BeginTransaction())
                {
                    try
                    {
                        //INIT Blank Tables <- Tables with foreign key FIRST
                        //blankTables.ForEach(x => initTable(x.tableName, x.tableSpecs, dbConnection, trans));

                        BlankTables.ForEach(T => T.InitTable(dbConnection, trans));
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



