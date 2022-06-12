using Microsoft.Data.Sqlite;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class EquipmentSetEffectsTable : BaseDBTable, ITableUpload
    {
        private string[] SetAt = {"(" +
                "EquipSet string," +
                "ClassType string," +
                "SetAt int," +
                "STR int," +
                "DEX int," +
                "INT int," +
                "LUK int," +
                "AllStats int," +
                "MaxHP int," +
                "MaxMP int," +
                "PercMaxHP int," +
                "PercMaxMP int," +
                "DEF int," +
                "WATK int," +
                "MATK int," +
                "IED int," +
                "BD int," +
                "CDMG int," +
                "DMG int," +
                "AllSkills int," +
                "NDMG int," +
                "StatusResistance int," +
                "PRIMARY KEY (EquipSet, ClassType)" +
                ");"
        };

        private string[] CulSet = { "(" +
                "EquipSet int," +
                "ClassType int,Set At,STR,DEX,INT,LUK,All Stats,Max HP,Max MP,Perc Max HP,Perc Max MP,Defense,Weapon Attack,Magic Attack,Ignored Enemy Defense,Boss Damage,Critical Damage,Damage,All Skills,Damage Against Normal Monsters,Abnormal Status Resistance"

        };
        public EquipmentSetEffectsTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            switch (TableName)
            {
                case "SetEffectAt":
                    TablePara = SetAt[0];
                    break;
                case "CulSetEffect":
                    break;
            }
        }

        public void RetrieveData()
        {
            throw new NotImplementedException();
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
