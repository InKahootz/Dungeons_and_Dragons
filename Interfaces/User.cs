using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Interfaces
{
    [DataContract]
    public class User
    {
        public void Initialize(Dictionary<String, String> sql, DateTime time) {
            LogInTime = time;

            Id = int.Parse(sql["id"]);
            Name = sql["login"];

            Race = sql["race"];
            Gender = sql["gender"];

            Class1 = sql["class1"];
            Class1Lvl = int.Parse(sql["class1_lvl"]);
            Experience = int.Parse(sql["experience"]);

            Attributes = new Dictionary<String, Int32>();
            Attributes.Add("Str", int.Parse(sql["strength"]));
            Attributes.Add("Dex", int.Parse(sql["dexterity"]));
            Attributes.Add("Build", int.Parse(sql["build"]));
            Attributes.Add("Int", int.Parse(sql["intellect"]));
            Attributes.Add("Prud", int.Parse(sql["prudence"]));
            Attributes.Add("Char", int.Parse(sql["charisma"]));

            Attributes.Add("StrMod", int.Parse(sql["strengthModifier"]));
            Attributes.Add("DexMod", int.Parse(sql["dexterityModifier"]));
            Attributes.Add("BuildMod", int.Parse(sql["buildModifier"]));
            Attributes.Add("IntMod", int.Parse(sql["intellectModifier"]));
            Attributes.Add("PrudMod", int.Parse(sql["prudenceModifier"]));
            Attributes.Add("CharMod", int.Parse(sql["charismaModifier"]));

            Perseverance = int.Parse(sql["perseverance"]);
            Reflex = int.Parse(sql["reflex"]);
            Will = int.Parse(sql["will"]);

            DefenseThrows = new Dictionary<String, Int32>();
            DefenseThrows.Add("Per", (Perseverance + Attributes["BuildMod"]));
            DefenseThrows.Add("Ref", (Reflex + Attributes["DexMod"]));
            DefenseThrows.Add("Wil", (Will + Attributes["PrudMod"]));

            Notes = sql["notes"];
        }

        [DataMember]
        public DateTime LogInTime { get; set; }

        [DataMember]
        public Int32 Id { get; set; }

        [DataMember]
        public String Name { get; set; }


        [DataMember]
        public String Race { get; set; }

        [DataMember]
        public String Gender { get; set; }


        [DataMember]
        public String Class1 { get; set; }

        [DataMember]
        public Int32 Class1Lvl { get; set; }

        [DataMember]
        public String Class2 { get; set; }

        [DataMember]
        public Int32 Class2Lvl { get; set; }

        [DataMember]
        public Int32 Experience { get; set; }


        [DataMember]
        public Int32 Perseverance { get; set; }
        [DataMember]
        public Int32 Reflex { get; set; }
        [DataMember]
        public Int32 Will { get; set; }


        [DataMember]
        public Dictionary<String, Int32> Attributes { get; set; }
        [DataMember]
        public Dictionary<String, Int32> DefenseThrows { get; set; }


        [DataMember]
        public String Notes { get; set; }
    }
}
