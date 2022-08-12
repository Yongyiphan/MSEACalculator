using MSEACalculator.CalculationRes.ViewModels;
using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CalculationRes
{
    public class CalForm
    {
        

        public static int calSFStatBoost(int BaseStat, int ProportionPerc)
        {
            int proportion = ProportionPerc/100;
            int result = (BaseStat / proportion) + 1;

            return result;


        }



        public static int CalMaxExp(int MaxLvl)
        {
            int MaxExp = 0;
            for (int i = 1; i < MaxLvl; i++)
            {
                MaxExp += CalCurrentLimit(i);
            }

            return MaxExp;
        }

        public static int CalCurrentLimit(int level)
        {
            return (level * level) + 11;
        }

        public static int CalAccEXp(int cLvl, int cExp)
        {
            int currentLimit = CalCurrentLimit(cLvl);
            int totalExp = 0;
            for (int i = 1; i < cLvl + 1; i++)
            {
                totalExp += CalCurrentLimit(i);
            }
            totalExp += cExp - currentLimit;

            return totalExp;
        }

 
        public static int CalDaysLeft(int accExp, decimal symGain, int ceiling)
        {

            int remainingExp = ceiling - accExp;
            decimal DaysLeft = 0;
            try
            {
                DaysLeft = Math.Ceiling(remainingExp / symGain);

            }
            catch (DivideByZeroException)
            {
                DaysLeft = -1;
            }

            return (int)DaysLeft;
        }

        public static int CalCostSymbol(int CLvl,int MLvl, int CostlvlMod, int CostMod)
        {
            int Cost = 0;
            for (int i = CLvl; i < MLvl+1; i++)
            {
                Cost += (i * CostlvlMod) + CostMod;
            }

            return Cost;
        }

        public static Dictionary<string, int> CalNewLvlExp(int accExp)
        {
            

            Dictionary<string, int> dictStore = new Dictionary<string, int>();

            //int currentLimit = CalCurrentLimit(cLvl);
            //int remainingExp, totalExp = 0;
            //for(int i = 1; i < cLvl + 1; i++)
            //{
            //    totalExp += CalCurrentLimit(i);
            //}

            //totalExp += cExp - CalCurrentLimit(cLvl);
            int currentLimit = CalCurrentLimit(1);
            int cLvl = 1;


            dictStore["CurrentTotalExp"] = accExp;

            
            while(accExp > currentLimit)
            {
                cLvl++;
                accExp -= currentLimit;
                currentLimit = CalCurrentLimit(cLvl);
            }

            dictStore["NewLevel"] = cLvl;
            dictStore["NewLimit"] = currentLimit;
            dictStore["RemainingExp"] = accExp;




            return dictStore;
        }


        public static Dictionary<string,int> CalArcaneStatsForce(int symbolLvl, string mode = "General")
        {
            Dictionary<string, int> dictStore = new Dictionary<string, int>();
            int cForce = 0;
            int cStat = 0;
            int baseForce = 30;
            int addForce = 10;

            int addStat = 0;
            int baseStat = 0;

            
            switch (mode)
            {
                case "Demon Avenger":
                    baseStat = 5250;
                    addStat = 1750;
                    break;

                case "Xenon":
                    baseStat = 117;
                    addStat = 39;
                    break;

                case "General":
                    baseStat = 300;
                    addStat = 100;
                    break;
            }
            
            if (symbolLvl > 1)
            {
                for(int i = 1; i<symbolLvl; i++)
                {
                    cForce += addForce;
                    cStat += addStat;
                }
            }

            dictStore["ArcaneForce"] = cForce + baseForce;
            dictStore["Stat"] = cStat + baseStat;


            return dictStore;
        }


        public static Dictionary<string, decimal> CalMesoConversion(decimal mesoRate, decimal moneyIn, string cMode)
        {
            Dictionary<string, decimal> dictStore = new Dictionary<string, decimal>();
            decimal billionMod = 1000000000;
            switch (cMode)
            {
                case "SGD":
                    //MoneyIn = SGD
                    dictStore["SGD"] = moneyIn;
                    dictStore["Meso"] = decimal.Divide(moneyIn, mesoRate) * billionMod;
                    break;

                case "B":
                    //MoneyIn = B
                    dictStore["SGD"] = decimal.Multiply(moneyIn, mesoRate);
                    dictStore["Meso"] = moneyIn * billionMod;
                    

                    break;
                case "Meso":
                    //MoneyIn = Meso
                    dictStore["SGD"] = decimal.Multiply(decimal.Divide(moneyIn, billionMod), mesoRate);
                    dictStore["Meso"] = moneyIn;
                    break;



                default:
                    break;
            }

            return dictStore;

        }

        public static (List<StatInputValue>, string) CalScrolling(EquipCLS Equip, EquipStatsCLS ScrollStat, List<string> StatObserved, string MainStat)
        {
            int SlotCount = Equip.SlotCount;
            int STTier = ComFunc.SpellTraceTier(Equip);
            string SlotType = ComFunc.ReturnScrollCat(Equip.EquipSlot);

            List<StatInputValue> StatPerSlot = new List<StatInputValue>();

            //    int recordedValue = Convert.ToInt32(ScrollStat.GetType().GetProperty(stat.Stat).GetValue(ScrollStat));
            Dictionary<string, int> ScrollRecord = ScrollStat.ToRecord();
            foreach (string stat in StatObserved)
            {
                
                if (ScrollRecord[stat] > 0)
                {
                    StatPerSlot.Add(new StatInputValue() { Stat = stat, Value = ((double)ScrollRecord[stat] / SlotCount).ToString() });
                    continue;
                }
                StatPerSlot.Add(new StatInputValue() { Stat = stat, Value = "0" });
            }

            //Find from ComFunc.SpellTraceDict the proper perc
            if (Equip.IsSpellTraced)
            {
                Dictionary<int, ScrollingModelCLS> CDict = ComFunc.SpellTraceDict[SlotType][STTier];
                Dictionary<string, string> TempDict = StatPerSlot.ToDictionary(x => x.Stat, x => x.Value);
                int PercFound = 0;
                string SearchProperty = "";

                if (GVar.ArmorEquips.Contains(Equip.EquipSlot) && Equip.EquipSlot != "Gloves" && Equip.SlotCount > 3)
                {
                    string atk = MainStat == "INT" ? "MATK" : "ATK";
                    TempDict[atk] = (Convert.ToDouble(TempDict[atk]) - 1d/SlotCount).ToString();
                }
                foreach (KeyValuePair<int, ScrollingModelCLS> kv in CDict)
                {
                CheckPerc:
                    if (PercFound != 0)
                    {
                        break;
                    }
                        foreach (KeyValuePair<string, string> kvt in TempDict)
                        {
                            SearchProperty = kvt.Key;
                            if (GVar.MainStats.Contains(kvt.Key))
                            {
                                SearchProperty = "MainStat";
                            }
                            if (kvt.Key == "ATK" || kvt.Key == "MATK")
                            {
                                SearchProperty = "ATK";
                            }

                            double recordedValue;
                            try
                            {
                                recordedValue = Convert.ToDouble(kvt.Value);
                            }
                            catch
                            {

                                recordedValue = Convert.ToDouble(Convert.ToInt32(kvt.Value));
                            }
                            double spelltraceValue = Convert.ToDouble(kv.Value.GetType().GetProperty(SearchProperty).GetValue(kv.Value));
                            if (recordedValue > 0 && recordedValue == spelltraceValue)
                            {
                                PercFound = kv.Key;
                                goto CheckPerc;
                            }
                            
                        }

                }
                foreach (StatInputValue statvalue in StatPerSlot)
                {
                    statvalue.Value = TempDict[statvalue.Stat];
                }



                Debug.WriteLine("Is Spell Traced");
                return (StatPerSlot, String.Format("{0}% Spell Trace Used", PercFound));
                
            }

            Debug.WriteLine("Not Spell Traced");

            return (StatPerSlot, "Not Spell Traced, Stat Per Slot");

        }

        public static EquipStatsCLS NewCalSpellTrace(EquipCLS SEquip, string SlotType, string MainStat, string ClassType)
        {
            EquipStatsCLS Result = new EquipStatsCLS();

            int STTier = ComFunc.SpellTraceTier(SEquip);
            int Perc = SEquip.SpellTracePerc;
            int SlotCount = SEquip.SlotCount;

            if (SlotCount > 0)
            {
                Result.MaxHP = ComFunc.SpellTraceDict[SlotType][STTier][Perc].MaxHP * SlotCount;
                Result.DEF = ComFunc.SpellTraceDict[SlotType][STTier][Perc].DEF * SlotCount;

                if (GVar.ArmorEquips.Contains(SEquip.EquipSlot) && SEquip.EquipSlot != "Gloves" && SlotCount > 3)
                {
                    if (ClassType == "Magician")
                    {
                        Result.MATK += 1;
                    }
                    else
                    {
                        Result.ATK += 1;
                    }
                }
                if (SlotType == "Weapon" || SlotType == "Heart" || SlotType == "Gloves")
                {
                    if (ClassType == "Magician")
                    {
                        Result.MATK = ComFunc.SpellTraceDict[SlotType][STTier][Perc].ATK * SlotCount;
                    }
                    else
                    {
                        Result.ATK  = ComFunc.SpellTraceDict[SlotType][STTier][Perc].ATK * SlotCount;
                    }
                }

                if (MainStat == "HP")
                {
                    Result.MaxHP += ComFunc.SpellTraceDict[SlotType][STTier][Perc].MainStat * SlotCount * 50;
                }
                else if (MainStat == "All Stat")
                {
                    Result.AppendJobStat("Any", STTier * SlotCount, 0);
                }
                else
                {
                    Result.GetType().GetProperty(MainStat).SetValue(Result, ComFunc.SpellTraceDict[SlotType][STTier][Perc].MainStat * SlotCount, null);
                }


            }

            return Result;
        }


        public static EquipStatsCLS CalSpellTrace(EquipCLS selectedEquip,CharacterCLS Character, string slotType, string ScrollType = "") 
        {
            //ONLY AFFECTS MAIN STAT, HP, DEF, ATK/MATK

            EquipStatsCLS result = new EquipStatsCLS();

            string MainStat = Character.ClassName == "Xenon" ? ScrollType : Character.MainStat;
            string ClassType = Character.ClassType;
            int STTier = ComFunc.SpellTraceTier(selectedEquip);
            int perc = selectedEquip.SpellTracePerc;
            result.MaxHP = ComFunc.SpellTraceDict[slotType][STTier][perc].MaxHP * selectedEquip.SlotCount;
            result.DEF = ComFunc.SpellTraceDict[slotType][STTier][perc].DEF * selectedEquip.SlotCount;
            if (GVar.ArmorEquips.Contains(selectedEquip.EquipSlot) && selectedEquip.EquipSlot != "Gloves" && selectedEquip.SlotCount > 3)
            {
                if (ClassType == "Magician")
                {
                    result.MATK += 1;
                }
                else
                {
                    result.ATK += 1;
                }
            }

            if (slotType == "Weapon" || slotType == "Heart" || slotType == "Gloves")
            {
                if (ClassType == "Magician")
                {
                    result.MATK = ComFunc.SpellTraceDict[slotType][STTier][perc].ATK * selectedEquip.SlotCount;
                }
                else
                {
                    result.ATK  = ComFunc.SpellTraceDict[slotType][STTier][perc].ATK * selectedEquip.SlotCount;
                }
            }

            if (MainStat == "HP")
            {
                result.MaxHP += ComFunc.SpellTraceDict[slotType][STTier][perc].MainStat * selectedEquip.SlotCount * 50;
            }
            else
            {
                result.GetType().GetProperty(MainStat).SetValue(result, ComFunc.SpellTraceDict[slotType][STTier][perc].MainStat * selectedEquip.SlotCount, null);
            }

            return result;
        }

        public static EquipStatsCLS ReverseStarforceStats(EquipCLS CEquip, EquipStatsCLS CurrentTotal, ReadOnlyCollection<StarforceCLS> SFList, string ClassType, string MainStat, string EquipTitle)
        {
            //CurrentTotal = Base + BlueStat
            //Reverse StaforceStats => CurrentTotal = Base + Scroll

            switch (EquipTitle)
            {
                case "Normal_Equips":
                    for (int i = CEquip.StarForce; i--> 0;)
                    {
                        StarforceCLS current = SFList.ElementAt(i);
                        if (i<15)
                        {
                            CurrentTotal.SubtractJobStat(ClassType, current.JobStat, current.JobStat, MainStat);
                            if (GVar.CategoryAEquips.Contains(CEquip.EquipSlot))
                            {
                                CurrentTotal.MaxHP -=  current.CatAMaxHP;
                            }
                            if (CEquip.EquipSlot != "Weapon")
                            {

                                CurrentTotal.DEF = StatBoost(current.NonWeapVDef, CurrentTotal.DEF, "Divide");

                                switch (CEquip.EquipSlot)
                                {
                                    case "Gloves":
                                        if (ClassType == "Magician")
                                        {
                                            CurrentTotal.MATK -= current.GloveVMATK;
                                        }
                                        else
                                        {
                                            CurrentTotal.ATK -= current.GloveVATK;
                                        }
                                        break;
                                    case "Shoes":
                                        CurrentTotal.SPD -=  current.SSpeed;
                                        CurrentTotal.JUMP -= current.SJump;
                                        break;
                                    case "Overall":
                                        CurrentTotal.DEF = StatBoost(current.NonWeapVDef, CurrentTotal.DEF, "Divide");
                                        break;

                                }
                            }
                            else
                            {
                                if (CurrentTotal.ATK > 0)
                                {
                                    CurrentTotal.ATK = StatBoost(current.WeapVATK, CurrentTotal.ATK, "Divide");
                                }
                                if (CurrentTotal.MATK > 0)
                                {

                                    CurrentTotal.MATK = StatBoost(current.WeapVMATK, CurrentTotal.MATK, "Divide");
                                }
                                CurrentTotal.MaxMP -= current.WeapMaxMP;

                            }

                        }
                        else
                        {
                            (int, int) SFKey = ComFunc.ReturnSFRange(current.VStatL.Keys.ToList(), CEquip);
                            foreach (string stat in GVar.MainStats)
                            {
                                int cValue = Convert.ToInt32(CurrentTotal.GetType().GetProperty(stat).GetValue(CurrentTotal));
                                if (cValue > 0)
                                {
                                    CurrentTotal.GetType().GetProperty(stat).SetValue(CurrentTotal, cValue -= current.VStatL[SFKey]);
                                }
                            }
                            if (CEquip.EquipSlot !="Weapon")
                            {
                                CurrentTotal.DEF = StatBoost(current.NonWeapVDef, CurrentTotal.DEF, "Divide");
                                if (CEquip.EquipSlot == "Overall")
                                {
                                    CurrentTotal.DEF = StatBoost(current.NonWeapVDef, CurrentTotal.DEF, "Divide");
                                }
                                CurrentTotal.ATK -= current.NonWeapVATKL[SFKey];
                                CurrentTotal.MATK -= current.NonWeapVMATKL[SFKey];
                            }
                            else
                            {
                                if (CurrentTotal.ATK > 0)
                                {
                                    CurrentTotal.ATK -= current.WeapVATKL[SFKey];
                                }
                                if (CurrentTotal.MATK > 0)
                                {
                                    CurrentTotal.MATK -= current.WeapVMATKL[SFKey];
                                }
                            }


                        }
                    }
                    break;
                case "Superior_Items":
                    for (int i = CEquip.StarForce; i--> 0;)
                    {
                        StarforceCLS current = SFList.ElementAt(i);

                        (int, int) SFKey = ComFunc.ReturnSFRange(current.VStatL.Keys.ToList(), CEquip);
                        foreach (string ms in GVar.MainStats)
                        {
                            int cValue = Convert.ToInt32(CurrentTotal.GetType().GetProperty(ms).GetValue(CurrentTotal));
                            if (cValue > 0)
                            {
                                CurrentTotal.GetType().GetProperty(ms).SetValue(CurrentTotal, cValue - current.VStatL[SFKey]);
                            }
                        }
                        if (CurrentTotal.MATK > 0)
                        {
                            CurrentTotal.MATK -= current.WeapVATKL[SFKey];
                        }
                        if (CurrentTotal.ATK > 0)
                        {
                            CurrentTotal.ATK -= current.WeapVATKL[SFKey];
                        }
                        if (CurrentTotal.DEF> 0)
                        {
                            CurrentTotal.DEF -= current.VDef;
                        }
                    }

                    break;
            }

            //CurrentTotal = Base Stat + ScrollStats
            return CurrentTotal;
        }

        public static EquipStatsCLS NewCalStarforceStats(EquipCLS CEquip, ReadOnlyCollection<StarforceCLS> SFList, string ClassType, string MainStat, string EquipTitle = "Normal")
        {
            EquipStatsCLS ScaledStats = CEquip.BaseStats.ShallowCopy();
            //Base + Scrolled
            ScaledStats.ModifyEquipStat(CEquip.ScrollStats, "Add");

            EquipStatsCLS Result = ScaledStats.ShallowCopy();

            if (CEquip.StarForce > 0)
            {
                switch (EquipTitle)
                {
                    case "Normal_Equips":
                        for (int i = 0; i<CEquip.StarForce; i++)
                        {
                            StarforceCLS current = SFList.ElementAt(i);

                            if (i<15)
                            {
                                Result.AppendJobStat(ClassType, current.JobStat, current.JobStat, MainStat);
                                if (GVar.CategoryAEquips.Contains(CEquip.EquipSlot))
                                {
                                    Result.MaxHP +=  current.CatAMaxHP;
                                }
                                if (CEquip.EquipSlot != "Weapon")
                                {

                                    Result.DEF += StatBoost(current.NonWeapVDef, Result.DEF);

                                    switch (CEquip.EquipSlot)
                                    {
                                        case "Gloves":
                                            if (ClassType == "Magician")
                                            {
                                                Result.MATK += current.GloveVMATK;
                                            }
                                            else
                                            {
                                                Result.ATK += current.GloveVATK;
                                            }
                                            break;
                                        case "Shoes":
                                            Result.SPD +=  current.SSpeed;
                                            Result.JUMP += current.SJump;
                                            break;
                                        case "Overall":
                                            Result.DEF += StatBoost(current.NonWeapVDef, Result.DEF);
                                            break;

                                    }
                                }
                                else
                                {
                                    if (Result.ATK > 0)
                                    {
                                        Result.ATK += StatBoost(current.WeapVATK, Result.ATK);
                                    }
                                    if (Result.MATK > 0)
                                    {

                                        Result.MATK += StatBoost(current.WeapVMATK, Result.MATK);
                                    }
                                    Result.MaxMP += current.WeapMaxMP;

                                }

                            }
                            else
                            {
                                (int, int) SFKey = ComFunc.ReturnSFRange(current.VStatL.Keys.ToList(), CEquip);
                                foreach (string stat in GVar.MainStats)
                                {
                                    int value = Convert.ToInt32(ScaledStats.GetType().GetProperty(stat).GetValue(ScaledStats));
                                    int cValue = Convert.ToInt32(Result.GetType().GetProperty(stat).GetValue(Result));
                                    if (value > 0)
                                    {
                                        Result.GetType().GetProperty(stat).SetValue(Result, cValue += current.VStatL[SFKey]);
                                    }
                                }
                                if (CEquip.EquipSlot !="Weapon")
                                {
                                    Result.DEF += StatBoost(current.NonWeapVDef, Result.DEF);
                                    if (CEquip.EquipSlot == "Overall")
                                    {
                                        Result.DEF += StatBoost(current.NonWeapVDef, Result.DEF);
                                    }
                                    Result.ATK += current.NonWeapVATKL[SFKey];
                                    Result.MATK += current.NonWeapVMATKL[SFKey];
                                }
                                else
                                {
                                    if (Result.ATK > 0)
                                    {
                                        Result.ATK += current.WeapVATKL[SFKey];
                                    }
                                    if (Result.MATK > 0)
                                    {
                                        Result.MATK += current.WeapVMATKL[SFKey];
                                    }
                                }


                            }


                        }
                        break;

                    case "Superior_Items":
                        for (int i = 0; i< CEquip.StarForce; i++)
                        {
                            StarforceCLS current = SFList.ElementAt(i);

                            (int, int) SFKey = ComFunc.ReturnSFRange(current.VStatL.Keys.ToList(), CEquip);
                            foreach (string ms in GVar.MainStats)
                            {
                                int cValue = Convert.ToInt32(Result.GetType().GetProperty(ms).GetValue(Result));
                                if (cValue > 0)
                                {
                                    Result.GetType().GetProperty(ms).SetValue(Result, cValue + current.VStatL[SFKey]);
                                }
                            }
                            if (Result.MATK > 0)
                            {
                                Result.MATK += current.WeapVATKL[SFKey];
                            }
                            if (Result.ATK > 0)
                            {
                                Result.ATK += current.WeapVATKL[SFKey];
                            }
                            if (Result.DEF> 0)
                            {
                                Result.DEF += current.VDef;
                            }
                        }

                        break;
                }
            }

            Result.ModifyEquipStat(ScaledStats, "Subtract");

            return Result;
        }

        public static EquipStatsCLS CalStarforceStats(CharacterCLS Character, EquipCLS CEquip, ReadOnlyCollection<StarforceCLS> SFList, string EquipSFType = "Basic")
        {
            EquipStatsCLS ScaledStats = CEquip.BaseStats.ShallowCopy();

            //BASE + SCROLLED STATS
            ScaledStats.ModifyEquipStat(CEquip.ScrollStats, "Add");



            EquipStatsCLS Result = ScaledStats.ShallowCopy();

            if (GVar.EnhanceRestriction["Starforce"].Contains(CEquip.EquipSlot) == false && CEquip.StarForce >  0)
            {
                switch (EquipSFType)
                {
                    case "Basic":
                        for (int i = 0; i<CEquip.StarForce; i++)
                        {

                            StarforceCLS current = SFList.ElementAt(i);

                            if (i<15)
                            {

                                
                                Result.AppendJobStat(CEquip.ClassType == "Any" || Character.ClassName == "Xenon" ? CEquip.ClassType : Character.ClassType, current.JobStat, current.JobStat, Character.MainStat);
                                if (GVar.CategoryAEquips.Contains(CEquip.EquipSlot))
                                {
                                    Result.MaxHP +=  current.CatAMaxHP;
                                }
                                if (CEquip.EquipSlot != "Weapon")
                                {

                                    Result.DEF += StatBoost(current.NonWeapVDef, Result.DEF);
                                    //TempStat.PercDEF += current.NonWeapVDef;

                                    switch (CEquip.EquipSlot)
                                    {
                                        case "Gloves":
                                            if (Character.ClassType == "Magician")
                                            {
                                                Result.MATK += current.GloveVMATK;
                                            }
                                            else
                                            {

                                                Result.ATK += current.GloveVATK;
                                            }
                                            break;
                                        case "Shoes":
                                            Result.SPD +=  current.SSpeed;
                                            Result.JUMP += current.SJump;
                                            break;
                                        case "Overall":
                                            Result.DEF += StatBoost(current.NonWeapVDef, Result.DEF);
                                            break;

                                    }
                                }
                                else
                                {
                                    if (Result.ATK > 0)
                                    {
                                        Result.ATK += StatBoost(current.WeapVATK, Result.ATK);
                                    }
                                    if (Result.MATK > 0)
                                    {

                                        Result.MATK += StatBoost(current.WeapVMATK, Result.MATK);
                                    }
                                    Result.MaxMP += current.WeapMaxMP;

                                }

                            }
                            else
                            {
                                foreach (string stat in GVar.MainStats)
                                {
                                    int value = Convert.ToInt32(ScaledStats.GetType().GetProperty(stat).GetValue(ScaledStats));
                                    int cValue = Convert.ToInt32(Result.GetType().GetProperty(stat).GetValue(Result));
                                    if (value > 0)
                                    {
                                        //Result.GetType().GetProperty(stat).SetValue(Result, cValue += current.VStatL[lvlRank]);
                                    }
                                }

                                if (CEquip.EquipSlot !="Weapon")
                                {
                                    Result.DEF += StatBoost(current.NonWeapVDef, Result.DEF);
                                    if (CEquip.EquipSlot == "Overall")
                                    {
                                        Result.DEF += StatBoost(current.NonWeapVDef, Result.DEF);
                                    }
                                    //Result.ATK += current.NonWeapVATKL[lvlRank];
                                    //Result.MATK += current.NonWeapVMATKL[lvlRank];
                                }
                                else
                                {
                                    //    if (Result.ATK > 0)
                                    //    {
                                    //        Result.ATK += current.WeapVATKL[lvlRank];
                                    //    }
                                    //    if (Result.MATK > 0)
                                    //    {
                                    //        Result.MATK += current.WeapVMATKL[lvlRank];
                                    //    }
                                }


                            }
                        }


                        break;
                    case "Superior":

                        for (int i = 0; i< CEquip.StarForce; i++)
                        {
                            StarforceCLS current = SFList.ElementAt(i);
                            foreach (string ms in GVar.MainStats)
                            {
                                int cValue = Convert.ToInt32(Result.GetType().GetProperty(ms).GetValue(Result));
                                if (cValue > 0) 
                                {
                                    //Result.GetType().GetProperty(ms).SetValue(Result, cValue + current.VStatL[lvlRank]);
                                }
                            }
                            if(Result.MATK > 0)
                            {
                                    //Result.MATK += current.WeapVATKL[lvlRank];
                            }
                            if (Result.ATK > 0)
                            {
                                //Result.ATK += current.WeapVATKL[lvlRank];
                            }
                            if(Result.DEF> 0)
                            {
                                Result.DEF += current.VDef;
                            }
                        }

                        break;
                }
            }

            Result.ModifyEquipStat(ScaledStats, "Subtract");
            //TempStat.FlatDEF -= ScaledStats.FlatDEF;
            //TempStat.FlatATK -= ScaledStats.FlatATK;
            //TempStat.FlatMATK -= ScaledStats.FlatMATK;
            
            //foreach(string stat in GVar.MainStats)
            //{
            //    int value = Convert.ToInt32(ScaledStats.GetType().GetProperty(stat).GetValue(ScaledStats));
            //    //Update Respective JobStat
            //    result.GetType().GetProperty(stat).SetValue(result, TempStat.GetType().GetProperty(stat).GetValue(TempStat));
            //    if(CEquip.StarForce > 15)
            //    {
            //        int current = Convert.ToInt32(result.GetType().GetProperty(stat).GetValue(result));
            //        if (value > 0)
            //        {
            //            current +=  TempStat.MS;                        
            //            result.GetType().GetProperty(stat).SetValue(result, current, null);
            //        }

            //    }

            //}

            //List<string> vList = new List<string>
            //{
            //    "DEF", "ATK","MATK"
            //};

            //foreach(string flat in vList)
            //{
                
            //    int value = Convert.ToInt32(ScaledStats.GetType().GetProperty(String.Format("Flat{0}", flat)).GetValue(ScaledStats));
            //    int addon = StatBoost(Convert.ToInt32(TempStat.GetType().GetProperty(String.Format("Perc{0}",flat)).GetValue(TempStat)), value);
            //    if (value > 0)
            //    {
            //        result.GetType().GetProperty(String.Format("Flat{0}", flat)).SetValue(result, value += addon);
            //    }
            //}

            //result.FlatATK += TempStat.FlatATK;



            return Result;

            }

        public static int StatBoost(int perc, int itemstat, string type = "Multiply")
        {
            decimal result;
            int output = 0;
            switch (type)
            {
                case "Multiply":
                    result = Math.Floor(Convert.ToDecimal(itemstat) * perc/100m);
                    output = 1 + Convert.ToInt32(result);
                    break;
                case "Divide":
                    decimal t = perc/100m;
                    result = Math.Floor(Convert.ToDecimal(itemstat) / (1+perc/100m));
                    output = Convert.ToInt32(result);
                    break;
            }


            return output;
        }

    }
}
