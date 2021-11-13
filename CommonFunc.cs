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
using Windows.UI.Popups;

namespace MSEACalculator
{
    class CommonFunc
    {
        

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

        public static string returnUnionRank(string charName,int lvl)
        {
            string rank = "";

            if(charName == "Zero")
            {
                if(lvl >= 130 && lvl < 160)
                {
                    rank = "B";
                }
                else if(lvl >=160 && lvl < 180)
                {
                    rank = "A";
                }
                else if(lvl >=180 && lvl < 200)
                {
                    rank = "S";
                }
                else if(lvl >=200 && lvl < 250)
                {
                    rank = "SS";
                }
                else if(lvl >= 250)
                {
                    rank = "SSS";
                }
            }
            else
            {
                if (lvl >= 60 && lvl < 100)
                {
                    rank = "B";
                }
                else if (lvl >= 100 && lvl < 140)
                {
                    rank = "A";
                }
                else if (lvl >= 140 && lvl < 200)
                {
                    rank = "S";
                }
                else if (lvl >= 200 && lvl < 250)
                {
                    rank = "SS";
                }
                else if (lvl >= 250)
                {
                    rank = "SSS";
                }

            }


            return rank;
        }

        public static bool validateIntInput(object para)
        {
            int pValue;

            return int.TryParse(para.ToString(), out pValue);
        }

        public static async void errorDia(string message)
        {
            var errorDia = new MessageDialog(message);
            await errorDia.ShowAsync();
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
