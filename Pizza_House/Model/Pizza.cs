using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    class Pizza
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual List<Pizza_Ingredients> Ingredients { get; set; }
        public string Topping
        {
            get { return string.Join(", ", Ingredients.Select(x => x.Ingredient.Name)); }
            set { }
        }
        public virtual List<Pizza_Sizes> Sizes { get; set; }
        public int Price
        {
            get { return Ingredients.Sum(x => x.Ingredient.Price); }
            set {  }
        }

        public Pizza(int Nr, string name)
        {
            ID = Nr;
            Name = name;
        }
        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}", ID + 1, Name, Ingredients.FirstOrDefault().Ingredient.Name, Price);
        }
    }
}
