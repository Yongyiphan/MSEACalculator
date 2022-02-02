﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.CalculationRes;

namespace MSEACalculator.CharacterRes
{
    public class ArcaneSymbol
    {

        //To be keyed in
        public string Name { get; set; }

        public int CurrentLevel { get; set; } = 1;
        public int CurrentExp { get; set; } = 1;
        


        //Generated when Compiled (Initialised in GVar.cs)
        public int CurrentLimit { get; set; } //CalForm.CalCurrentLimit(CurrentLevel);
        public int MaxExp { get; set; } = GVar.MaxSymbolExp;
        public string SubMap { get; set; }
        public bool unlockSubMap { get; set; } = false;
        public int BaseSymbolGain { get; set; } = 0;
        public int PartyQuestSymbols { get; set; } = 0;
        public int PQGainLimit { get; set; }



        //Result:
        public int DaysLeft { get; set; }
        public int NewCurrentLvl { get; set; }
        public int NewCurrentExp { get; set; }
        public int AccumulatedExp { get; set; } = 1;
        

        //Example
        //ChuChu
        //CurrentLevel : 1
        //CurrentExp/CurrentLimit: 885/12 

        //Results:
        //New Level = 13
        //CurrentExp/CurrentLimit : 103/180
        //AccumulatedExp / TotalExp : 885/2679
        //Days Left: ? Days
        //TotalCost: 826,280,000

        //Result XAML Display
        //Level: 13 103/180 855/2679
        //(N Days Left) | 1794 | 826,280,000

        //Arcane Catalyst

        public ArcaneSymbol()
        {
            CurrentLimit = CalForm.CalCurrentLimit(CurrentLevel);

        }

        
        

    }
}
