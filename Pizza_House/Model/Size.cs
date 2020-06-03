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

        public Size(int id, string name, int mod)
        {
            ID = id;
            Name = name;
            PriceMod = mod;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
