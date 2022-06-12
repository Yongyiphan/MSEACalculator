using MSEACalculator.CalculationRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class EquipStatsCLS
    {
        //BASE STATS
        //public int MS { get; set; } = 0;
        //public int SS { get; set; } = 0;

        public int STR { get; set; } = 0;
        public int DEX { get; set; } = 0;
        public int INT { get; set; } = 0;
        public int LUK { get; set; } = 0;       
        public int DEF { get; set; } = 0;
        public int PercDEF { get; set; } = 0;
        public int MaxHP { get; set; } = 0;
        public string HP { get; set; } = "";
        public int MaxMP { get; set; } = 0;
        public string MP { get; set; } = "";
        public int SPD { get; set; } = 0;
        public int JUMP { get; set; } = 0;
        public int ATK { get; set; } = 0;
        public int MATK { get; set; } = 0;

        public int PercATK { get; set; } = 0;
        public int PercMATK { get; set; } = 0;

        public int NoUpgrades { get; set; } = 0;

        public int MaxDF { get; set; } = 0;
        
        //For Heart
        public int Rank { get; set; } = 0;
        //ADDITIONAL BASE STAT FOR WEAPONS
        public int IED { get; set; } = 0;
        public int BD { get; set; } = 0;
        public int ATKSPD { get; set; } = 0;
        public int DMG { get; set; } = 0;

        //FLAME STATS
        public int AllStat { get; set; } = 0;

        public EquipStatsCLS()
        {

        }

        public override bool Equals(object obj)
        {
            List<string> test = new List<string>();
            if (obj == null)
            {
                return false;
            }
            if(obj is EquipStatsCLS)
            {
                EquipStatsCLS cObj = (EquipStatsCLS)obj;
                //test.Add(STR ==  cObj.STR ? "true" : "false" );
                //test.Add(DEX ==  cObj.DEX ? "true" : "false" );
                //test.Add(INT ==  cObj.INT ? "true" : "false" );
                //test.Add(LUK ==  cObj.LUK ? "true" : "false" );
                //test.Add(ATK ==  cObj.ATK ? "true" : "false" );
                //test.Add(MATK ==  cObj.MATK ? "true" : "false");
                //test.Add(DEF ==  cObj.DEF ? "true" : "false" );

                //test.Add(PercATK ==  cObj.PercATK ? "true" : "false" );
                //test.Add(PercMATK ==  cObj.PercMATK ? "true" : "false" );
                //test.Add(PercDEF ==  cObj.PercDEF ? "true" : "false" );
                //test.Add(MaxHP ==  cObj.MaxHP ? "true" : "false" );
                //test.Add(MaxMP ==  cObj.MaxMP ? "true" : "false" );
                //test.Add(SPD ==  cObj.SPD ? "true" : "false" );
                //test.Add(JUMP ==  cObj.JUMP ? "true" : "false" );
                //test.Add(AllStat ==  cObj.AllStat ? "true" : "false" );

                foreach (PropertyInfo prop in GetType().GetProperties())
                {
                    
                    if(prop.PropertyType == typeof(int))
                    {

                        int GT = Convert.ToInt32(cObj.GetType().GetProperty(prop.Name).GetValue(cObj));
                        int current = Convert.ToInt32(prop.GetValue(this));

                        test.Add(GT == current ? "true" : "false");
                    }
                    if(prop.PropertyType == typeof(string))
                    {
                        string GT = cObj.GetType().GetProperty(prop.Name).GetValue(cObj).ToString();
                        string current = prop.GetValue(this).ToString();

                        test.Add(GT == current ? "true" : "false");
                    }
                    
                }
                
            }
            return test.Contains("false") ? false : true;
        }

        public EquipStatsCLS ShallowCopy()
        {
            return (EquipStatsCLS)this.MemberwiseClone();
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        
        public void AppendJobStat(string ClassType, int s1, int s2, string MainStat = "")
        {
            switch (ClassType)
            {
                case "Warrior":
                    STR += s1;
                    DEX += s2;
                    break;
                case "Bowman":
                    DEX += s1;
                    STR += s2;
                    break;
                case "Magician":
                    INT += s1;
                    LUK += s2;
                    break;
                case "Thief":
                    LUK += s1;
                    DEX += s2;
                    break;
                case "Pirate":
                    if(MainStat == "DEX")
                    {
                        DEX += s1;
                        STR += s2;
                    }
                    else
                    {
                        STR += s1;
                        DEX += s2;
                    }
                    break;
                case "HP":
                    break;
                case "Any":
                    STR += s1;
                    DEX += s1;
                    INT += s1;
                    LUK += s1;
                    break;
            }
        }

        public void ModifyEquipStat(EquipStatsCLS target, string mode)
        {
            var properties = GetType().GetProperties();
            foreach(PropertyInfo prop in properties)
            {
                try
                {
                    int current = Convert.ToInt32(prop.GetValue(this));
                    int next = Convert.ToInt32(target.GetType().GetProperty(prop.Name).GetValue(target));

                    switch (mode)
                    {
                        case "Add":
                            prop.SetValue(this, current += next);
                            break;
                        case "Subtract":
                            prop.SetValue(this, current -= next);
                            break;
                    }

                }
                catch(Exception) 
                {

                    string c = prop.GetValue(this).ToString();
                    int current = string.IsNullOrEmpty(c) ? 0 : Convert.ToInt32(c.Trim('%'));

                    string n = target.GetType().GetProperty(prop.Name).GetValue(target).ToString();
                    int next = string.IsNullOrEmpty(n) ? 0 : Convert.ToInt32(n.Trim('%'));


                    switch (mode)
                    {
                        case "Add":
                            prop.SetValue(this, String.Format("{0}%",current += next));
                            break;
                        case "Subtract":
                            prop.SetValue(this, String.Format("{0}%",current -= next));
                            break;
                    }

                    
                }
                finally
                {
                    if (prop.PropertyType == typeof(int))
                    {
                        if (Convert.ToInt32(prop.GetValue(this)) < 0)
                        {
                            prop.SetValue(this, 0);
                        }
                    }
                }
               
            }


        }

        private List<string> HiddenFields = new List<string>()
        {
             "ATKSPD"
        };
        public Dictionary<string, int> ToRecord()
        {

            Dictionary<string, int> result = new Dictionary<string, int>();

            foreach(PropertyInfo prop in GetType().GetProperties())
            {

                if (HiddenFields.Contains(prop.Name))
                {
                    continue;
                }
                if(prop.PropertyType == typeof(string))
                {
                    string sValue = prop.GetValue(this).ToString();
                    int iValue = sValue == string.Empty ? 0 : Convert.ToInt32(sValue.TrimEnd('%'));
                    result[prop.Name] = iValue;
                }
                else
                {
                    result[prop.Name] = Convert.ToInt32(prop.GetValue(this));
                }
            }

            return result;
        }
        
        public void DictToProperty(Dictionary<string, int> Record)
        {
            foreach (string key in Record.Keys)
            {
                if (GetType().GetProperty(key).PropertyType == typeof(string))
                {
                    if (Record[key] == 0)
                    {
                        GetType().GetProperty(key).SetValue(this, string.Empty);
                    }
                    else
                    {
                        GetType().GetProperty(key).SetValue(this, string.Format("{0}%", Record[key]));
                    }
                }
                else
                {
                    GetType().GetProperty(key).SetValue(this, Record[key], null);
                }
            }

        }
        
    }
}
