using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    class Size
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int PriceMod { get; set; }
        public Size_Type Type { get; set; }

        public Size(int id, string name, int mod, Size_Type type)
        {
            ID = id;
            Name = name;
            PriceMod = mod;
            Type = type;
        }
        public override string ToString()
        {
            return Name + "\t\r" + PriceMod+"% To final Price";
        }
    }
    public enum Size_Type
    {
        Pizza = 0,
        Drink = 1
    }
}
