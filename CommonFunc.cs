using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using MSEACalculator.BossRes;
using MSEACalculator.StarforceRes;
using MSEACalculator.EventRes;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace MSEACalculator
{
    class CommonFunc
    {
        //public static async Task<Dictionary<int, Boss>> GetBossListAsync(string dataFolder = "DefaultData")
        //{

        //    Dictionary<int, Boss> AllBossList = new Dictionary<int, Boss>();



        //    //string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\Data\statGains.csv";
        //    StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        //    StorageFile statTable = await storageFolder.GetFileAsync(@"\" + dataFolder + @"\BossListData.csv");

        //    var stream = await statTable.OpenAsync(Windows.Storage.FileAccessMode.Read);

        //    ulong size = stream.Size;

        //    using (var inputStream = stream.GetInputStreamAt(0))
        //    {
        //        using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
        //        {
        //            uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
        //            string text = dataReader.ReadString(numBytesLoaded);

        //            var result = text.Split("\r\n");
        //            int counter = 0;
        //            foreach (string bossEntry in result.Skip(1))
        //            {
        //                if (bossEntry == "")
        //                {
        //                    return AllBossList;
        //                }

        //                var temp = bossEntry.Split(",");
        //                counter += 1;
        //                Boss tempboss = new Boss();

        //                tempboss.BossID = counter;
        //                tempboss.name = temp[0];
        //                tempboss.difficulty = temp[1];
        //                tempboss.entryType = temp[2];
        //                tempboss.entryLimit = Convert.ToInt32(temp[3]);
        //                tempboss.bossCrystalCount = Convert.ToInt32(temp[4]);
        //                tempboss.meso = Convert.ToInt32(temp[5]);
        //                AllBossList.Add(counter, tempboss);


        //            }

        //        }
        //    }



        //    return AllBossList;
        //}

        //public static async Task<Dictionary<int, SFGain>> GetSFListAsync()
        //{
        //    Dictionary<int, SFGain> SFList = new Dictionary<int, SFGain>();

        //    StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        //    StorageFile statTable = await storageFolder.GetFileAsync(@"\DefaultData\statGains.csv");

        //    var stream = await statTable.OpenAsync(Windows.Storage.FileAccessMode.Read);

        //    ulong size = stream.Size;

        //    using (var inputStream = stream.GetInputStreamAt(0))
        //    {
        //        using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
        //        {
        //            uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
        //            string text = dataReader.ReadString(numBytesLoaded);

        //            var result = text.Split("\r\n");
        //            int counter = 0;
        //            foreach (string SFEntry in result.Skip(1))
        //            {
        //                if (SFEntry == "")
        //                {
        //                    return SFList;
        //                }

        //                var sfitem = SFEntry.Split(",");
        //                if (counter >= 15)
        //                {
        //                    counter += 1;
        //                    SFGain tempSFitem2 = new SFGain();

        //                    tempSFitem2.StarForceLevel = counter;
        //                    tempSFitem2.MainStatL = sfitem[1].Split(";").ToList().Select(s => int.Parse(s)).ToList();
        //                    tempSFitem2.NonWeapDef = Convert.ToInt32(sfitem[2]);
        //                    tempSFitem2.OverallDef = Convert.ToInt32(sfitem[3]);
        //                    tempSFitem2.MaxHP = Convert.ToInt32(sfitem[4]);
        //                    tempSFitem2.MaxMP = Convert.ToInt32(sfitem[5]);
        //                    tempSFitem2.WATKL = sfitem[6].Split(";").ToList().Select(s => int.Parse(s)).ToList();
        //                    tempSFitem2.NonWATKL = sfitem[7].Split(";").ToList().Select(s => int.Parse(s)).ToList();
        //                    tempSFitem2.Speed = Convert.ToInt32(sfitem[8]);
        //                    tempSFitem2.Jump = Convert.ToInt32(sfitem[9]);
        //                    tempSFitem2.GloveAtk = Convert.ToInt32(sfitem[10]);


        //                    SFList.Add(counter, tempSFitem2);
        //                }
        //                else
        //                {
        //                    counter += 1;
        //                    SFGain tempSFitem1 = new SFGain();
        //                    tempSFitem1.StarForceLevel = counter;
        //                    tempSFitem1.MainStat = Convert.ToInt32(sfitem[1]);
        //                    tempSFitem1.NonWeapDef = Convert.ToInt32(sfitem[2]);
        //                    tempSFitem1.OverallDef = Convert.ToInt32(sfitem[3]);
        //                    tempSFitem1.MaxHP = Convert.ToInt32(sfitem[4]);
        //                    tempSFitem1.MaxMP = Convert.ToInt32(sfitem[5]);
        //                    tempSFitem1.WATK = Convert.ToInt32(sfitem[6]);
        //                    tempSFitem1.NonWATK = Convert.ToInt32(sfitem[7]);
        //                    tempSFitem1.Speed = Convert.ToInt32(sfitem[8]);
        //                    tempSFitem1.Jump = Convert.ToInt32(sfitem[9]);
        //                    tempSFitem1.GloveAtk = Convert.ToInt32(sfitem[10]);

        //                    SFList.Add(counter, tempSFitem1);

        //                }

        //            }

        //        }
        //    }



        //    return SFList;
        //}

        public int scrollUsedCal(int currentMainStat, int baseAtk, int itemlvl, Dictionary<int, SFGain> Stattable)
        {
            int scrollResult = 0;
            Dictionary<int, List<int>> spellTraceScroll = new Dictionary<int, List<int>>()
            {
                {100, new List<int>{1,3} },
                {70,  new List<int>{2,5} },
                {30,  new List<int>{3,7} },
                {15,  new List<int>{4,9} }

            };



            return scrollResult;
        }


        //public static async Task<List<EventRecords>> retrieveEventJson()
        //{
        //    List<EventRecords> eventList;

        //    string jsonFP = Path.Combine(ApplicationData.Current.LocalFolder.Path, @"DefaultData\Event.json");

        //    if (!File.Exists(jsonFP))
        //    {
        //        //if file dont exist create a blank file
        //        EventRecords tempR = new EventRecords();
        //        tempR.startDate = DateTime.Today;
        //        tempR.endDate = DateTime.Today;
        //        tempR.eventDuration = 0;
        //        tempR.availableDays = new Dictionary<string, bool>
        //        {
        //            {"Monday",false},
        //            {"Tuesday",false},
        //            {"Wednesday",false},
        //            {"Thursday",false},
        //            {"Friday",false},
        //            {"Saturday",false},
        //            {"Sunday",false}
        //        };

            
        //        List<EventRecords> temprecordList = new List<EventRecords>() { tempR}; 
            
        //        await DatabaseAccess.writetoEventJson("init", temprecordList);
        //        //Thread.Sleep(3000);
        //        using (FileStream fileStream = File.OpenRead(jsonFP))
        //        {
        //            eventList = await JsonSerializer.DeserializeAsync<List<EventRecords>>(fileStream);
        //        }

        //    }    

        //    using (FileStream fileStream = File.OpenRead(jsonFP))
        //    {
        //        eventList = await JsonSerializer.DeserializeAsync<List<EventRecords>>(fileStream);
        //    }



        //    return eventList;
        //}
        
        
    }
}
