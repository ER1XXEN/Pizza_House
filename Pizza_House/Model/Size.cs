using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    #region Class

    internal class Size
    {
        #region Properties

        public int ID { get; set; }
        public string Name { get; set; }
        public int PriceMod { get; set; }
        public Size_Type Type { get; set; }

        #endregion Properties

        #region Constructors

        public Size(int id, string name, int mod, Size_Type type)
        {
            ID = id;
            Name = name;
            PriceMod = mod;
            Type = type;
        }

        #endregion Constructors

        #region Overrides

        public override string ToString()
        {
            return Name + "\t\r" + PriceMod + "% To final Price";
        }

        #endregion Overrides
    }

    #endregion Class

    #region Enum

    public enum Size_Type
    {
        Pizza = 0,
        Drink = 1
    }

    #endregion Enum
}