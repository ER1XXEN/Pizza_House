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
        public Ingredient(int id, string name, int price)
        {
            ID = id;
            Name = name;
            Price = price;
        }
    }
}
