using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    class Ingredient
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Ingredient_Type Type { get; set; }
        public bool Selected { get; set; } = false;
        public Ingredient(int id, string name, int type, int price)
        {
            ID = id;
            Name = name;
            Price = price;
            Type = (Ingredient_Type)type;
        }
        public override string ToString()
        {
            return Name + "\t\t" + Price + " USD";
        }
    }
    public enum Ingredient_Type
    {
        Dough = 0,
        Topping = 1
    }
}
