using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using MSEACalculator.BossRes;
using MSEACalculator.OtherRes;

namespace MSEACalculator.CharacterRes.MesoRes
{
    public class MesoViewModel
    {


        //Init Variables
        public List<string> bossNameList { get; set; } = new List<string>();
        public List<string> difficultyList { get; set; } = new List<string>();
        public List<string> charNameList { get; set; } = new List<string>();

        private string _selectedMule;

        public string selectedMule
        {
            get
            {
                return _selectedMule;
            }
            set
            {
                _selectedMule = value;
            }
        }

        public MesoViewModel()
        {
            BossMeso bossMeso = new BossMeso();
            
            initloadFields(bossMeso.bossDict, bossMeso.charDict);

        }

        private void initloadFields(Dictionary<int,Boss> bossDict, Dictionary<string, Character> charDict)
        {

            foreach (Boss bossItem in bossDict.Values)
            {
                if (!difficultyList.Contains(bossItem.difficulty))
                {
                    difficultyList.Add(bossItem.difficulty);
                };

                if (!bossNameList.Contains(bossItem.name))
                {
                    bossNameList.Add(bossItem.name);
                };
            }
            foreach (Character charItem in charDict.Values)
            {
                if (!charNameList.Contains(charItem.className))
                {
                    charNameList.Add(charItem.className);
                };
            }
        }




    }

}
