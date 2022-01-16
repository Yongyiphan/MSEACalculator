using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.CharacterRes;
using MSEACalculator.StarforceRes;
using MSEACalculator.UnionRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;

namespace MSEACalculator.OtherRes.Database
{
    public class ImportCSV
    {
        public static async Task<List<Boss>> GetBossCSVAsync()
        {

            List<Boss> AllBossList = new List<Boss>();

            StorageFile statTable = await GVar.storageFolder.GetFileAsync(GVar.CalculationsPath + "BossListData.csv");

            var stream = await statTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    int counter = 0;
                    foreach (string bossEntry in result.Skip(1))
                    {
                        if (bossEntry == "")
                        {
                            return AllBossList;
                        }

                        var temp = bossEntry.Split(",");
                        counter += 1;
                        Boss tempboss = new Boss();
                        tempboss.BossID = counter;
                        tempboss.BossName = temp[1];
                        tempboss.Difficulty = temp[2];
                        tempboss.EntryType = temp[3];
                        tempboss.Meso = Convert.ToInt32(temp[4].Replace(",","")); 



                        AllBossList.Add(tempboss);


                    }

                }
            }



            return AllBossList;
        }

        public static async Task<List<SFGain>> GetSFCSVAsync()
        {
            List<SFGain> SFList = new List<SFGain>();

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

                        SFGain sFGain = new SFGain();
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
        public static async Task<List<SFGain>> GetAddSFCSVAsync()
        {
            List<SFGain> SFList = new List<SFGain>();

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

                        SFGain sFGain = new SFGain();
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

        public static async Task<List<Character>> GetCharCSVAsync()
        {
            List<Character> characterList = new List<Character>();


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
                        Character tempChar = new Character();
                        tempChar.ClassName = temp[1];
                        tempChar.Faction = temp[2];
                        tempChar.ClassType = temp[3];
                        tempChar.MainStat = temp[4];
                        tempChar.SecStat = temp[5];
                        tempChar.unionEffect = temp[6];
                        tempChar.unionEffectType = temp[7];

                        characterList.Add(tempChar);

                    }
                }
            }


            return characterList;
        }

        public static async Task<List<UnionModel>> GetUnionECSVAsync()
        {
            List<UnionModel> unionList = new List<UnionModel>();

            StorageFile UnionTable = await GVar.storageFolder.GetFileAsync(GVar.CharacterPath + "UnionData.csv");

            var stream = await UnionTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    int counter = 0;
                    foreach (string unionItems in result.Skip(1))
                    {
                        if (unionItems == "")
                        {
                            return unionList;
                        }
                        var temp = unionItems.Split(",");
                        counter += 1;
                        UnionModel tempUnion = new UnionModel();
                        tempUnion.Effect = temp[1];
                        tempUnion.RankB = Convert.ToInt32(temp[2]);
                        tempUnion.RankA = Convert.ToInt32(temp[3]);
                        tempUnion.RankS = Convert.ToInt32(temp[4]);
                        tempUnion.RankSS = Convert.ToInt32(temp[5]);
                        tempUnion.RankSSS = Convert.ToInt32(temp[6]);
                        tempUnion.EffectType = temp[7];


                        //Stat = temp[0],
                        //    StatType = temp[1],
                        //    RankB = Convert.ToInt32(temp[2]),
                        //    RankA = Convert.ToInt32(temp[3]),
                        //    RankS = Convert.ToInt32(temp[4]),
                        //    RankSS = Convert.ToInt32(temp[5]),
                        //    RankSSS = Convert.ToInt32(temp[6])
                        unionList.Add(tempUnion);
                    }
                }
            }

            return unionList;
        }

        public static async Task<List<EquipModel>> GetArmorCSVAsync()
        {
            List<EquipModel> equipList = new List<EquipModel>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.EquipmentPath + "ArmorData.csv");

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
                    foreach (string equipItem in result.Skip(1))
                    {
                        if (equipItem == "")
                        {
                            return equipList;
                        }

                        var temp = equipItem.Split(",");
                        counter += 1;
                        EquipModel equip = new EquipModel();
                        equip.EquipSet = temp[1];
                        equip.ClassType = temp[2];
                        equip.EquipSlot = temp[3];
                        equip.EquipLevel = Convert.ToInt32(temp[4]);
                        equip.BaseStats.MS = Convert.ToInt32(temp[5]);
                        equip.BaseStats.SS = Convert.ToInt32(temp[6]);
                        equip.BaseStats.AllStat = Convert.ToInt32(temp[7]);
                        equip.BaseStats.HP = Convert.ToInt32(temp[8]);
                        equip.BaseStats.MP = Convert.ToInt32(temp[9]);
                        equip.BaseStats.DEF = Convert.ToInt32(temp[10]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[11]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[12]);
                        equip.BaseStats.SPD = Convert.ToInt32(temp[13]);
                        equip.BaseStats.JUMP = Convert.ToInt32(temp[14]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[15]);

                        equipList.Add(equip);

                    }
                }
            }
            return equipList;

        }

        public static async Task<List<EquipModel>> GetAccessoriesCSVAsync()
        {
            List<EquipModel> AccessoriesList = new List<EquipModel>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.EquipmentPath + "AccessoriesData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string accItem in result.Skip(1))
                    {
                        if (accItem == "")
                        {
                            return AccessoriesList;
                        }

                        var temp = accItem.Split(",");

                        EquipModel equip = new EquipModel();
                        equip.EquipName = temp[1];
                        equip.EquipSet = temp[2];
                        equip.ClassType = temp[3];
                        equip.EquipSlot = temp[4];

                        equip.EquipLevel = Convert.ToInt32(temp[5]);
                        equip.BaseStats.MS = Convert.ToInt32(temp[6]);
                        equip.BaseStats.SS = Convert.ToInt32(temp[7]);
                        equip.BaseStats.AllStat = Convert.ToInt32(temp[8]);

                        equip.BaseStats.SpecialHP = temp[9];
                        equip.BaseStats.SpecialMP = temp[10];
                        equip.BaseStats.DEF = Convert.ToInt32(temp[11]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[12]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[13]);

                        equip.BaseStats.SPD = Convert.ToInt32(temp[14]);
                        equip.BaseStats.JUMP = Convert.ToInt32(temp[15]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[16]);


                        AccessoriesList.Add(equip);


                    }
                }
            }


            return AccessoriesList;
        }

        public static async Task<List<EquipModel>> GetWeaponCSVAsync()
        {
            List<EquipModel> WeaponList = new List<EquipModel>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.EquipmentPath + "WeaponData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string weapItem in result.Skip(1))
                    {
                        if (weapItem == "")
                        {
                            return WeaponList;
                        }

                        var temp = weapItem.Split(",");

                        EquipModel equip = new EquipModel();
                        equip.EquipSet = temp[1];
                        equip.WeaponType = temp[2];
                        equip.EquipLevel = Convert.ToInt32(temp[3]);
                        equip.BaseStats.ATKSPD = Convert.ToInt32(temp[4]);
                        equip.BaseStats.MS = Convert.ToInt32(temp[5]);
                        equip.BaseStats.SS = Convert.ToInt32(temp[6]);
                        equip.BaseStats.HP = Convert.ToInt32(temp[7]);
                        equip.BaseStats.DEF = Convert.ToInt32(temp[8]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[9]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[10]);
                        equip.BaseStats.SPD = Convert.ToInt32(temp[11]);
                        equip.BaseStats.BD = Convert.ToInt32(temp[12]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[13]);




                        WeaponList.Add(equip);


                    }
                }
            }


            return WeaponList;


        }
        public static async Task<List<EquipModel>> GetSecondaryCSVAsync()
        {
            List<EquipModel> WeaponList = new List<EquipModel>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.EquipmentPath + "SecondaryWeapData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string weapItem in result.Skip(1))
                    {
                        if (weapItem == "")
                        {
                            return WeaponList;
                        }

                        var temp = weapItem.Split(",");

                        EquipModel equip = new EquipModel();
                        equip.ClassType = temp[1];
                        equip.WeaponType = temp[2];
                        equip.EquipName = temp[3];
                        equip.EquipLevel = Convert.ToInt32(temp[4]);
                        equip.BaseStats.MS = Convert.ToInt32(temp[5]);
                        equip.BaseStats.SS = Convert.ToInt32(temp[6]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[7]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[8]);
                        equip.BaseStats.AllStat = Convert.ToInt32(temp[9]);
                        equip.BaseStats.DEF = Convert.ToInt32(temp[10]);
                        equip.BaseStats.HP = Convert.ToInt32(temp[11]);
                        equip.BaseStats.MP = Convert.ToInt32(temp[12]);
                        equip.BaseStats.ATKSPD = Convert.ToInt32(temp[13]);


                        WeaponList.Add(equip);


                    }
                }
            }


            return WeaponList;


        }

        public static async Task<Dictionary<int, List<string>>> GetClassMWeaponCSVAsync()
        {
            Dictionary<int, List<string>> CWdict = new Dictionary<int, List<string>>();
            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CharacterPath + "ClassMainWeapon.csv");

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
                    foreach (string CI in result.Skip(1))
                    {
                        if (CI == "")
                        {
                            return CWdict;
                        }
                        var temp = CI.Split(',');
                        var tempL = new List<string>() { temp[1], temp[2] };
                        CWdict.Add(counter, tempL);

                        counter++;
                    }
                }
            }


            return CWdict;
        }
        public static async Task<Dictionary<int, List<string>>> GetClassSWeaponCSVAsync()
        {
            Dictionary<int, List<string>> CWdict = new Dictionary<int, List<string>>();
            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CharacterPath + "ClassSecWeapon.csv");

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
                    foreach (string CI in result.Skip(1))
                    {
                        if (CI == "")
                        {
                            return CWdict;
                        }
                        var temp = CI.Split(',');
                        var tempL = new List<string>() { temp[1], temp[2] };
                        CWdict.Add(counter, tempL);

                        counter++;
                    }
                }
            }


            return CWdict;
        }

        public static async Task<List<PotentialStats>> GetPotentialCSVAsync()
        {
            List<PotentialStats> PotentialList = new List<PotentialStats>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CalculationsPath + "PotentialData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string potItem in result.Skip(1))
                    {
                        if (potItem == "")
                        {
                            Console.WriteLine("Hello");
                            return PotentialList;
                        }

                        var temp = potItem.Split(",");

                        PotentialStats Pot = new PotentialStats();

                        Pot.EquipGrpL = temp[1].Contains(";") ? temp[1].Split(';').ToList() : new List<string> { temp[1]};
                        Pot.Grade = temp[2];
                        Pot.Prime = temp[3];
                        Pot.StatType = temp[4];
                        Pot.StatIncrease = temp[5];
                        Pot.MinLvl = Convert.ToInt32(temp[6]);
                        Pot.MaxLvl = Convert.ToInt32(temp[7]);
                        Pot.StatValue = temp[8];
                        Pot.Duration = Convert.ToInt32(temp[9]);


                        PotentialList.Add(Pot);


                    }
                }
            }


            return PotentialList;


        }
    }
}
