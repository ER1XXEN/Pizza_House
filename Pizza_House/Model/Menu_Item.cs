using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    class Menu_Items
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public virtual List<Pizza_Ingredients> Ingredients { get; set; } = new List<Pizza_Ingredients>();
        public string Topping
        {
            get { return string.Join(", ", Ingredients.OrderBy(x => x.Ingredient.Type).Select(x => x.Ingredient.Name)); }
            set { }
        }
        public virtual List<Pizza_Sizes> Sizes { get; set; } = new List<Pizza_Sizes>();
        public string Price
        {
            get { return Ingredients.Sum(x => x.Ingredient.Price).ToString() + " USD"; }
            set { }
        }
        public Menu_Item_Type Type{ get; set; }

        public Menu_Items(string ID, string Name, string Toppings, string Price, Menu_Item_Type Type)
        {
            this.ID = ID;
            this.Name = Name;
            this.Topping = Toppings;
            this.Price = Price;
            this.Type = Type;
        }

        public Menu_Items(int Nr, string name)
        {
            ID = Nr.ToString();
            Name = name;
        }
        //public override string ToString()
        //{
        //    return String.Format("{0} {1} {2} {3}", ID + 1, Name, Ingredients.FirstOrDefault().Ingredient.Name, Price);
        //}
    }
    public enum Menu_Item_Type
    {
        Pizza = 0,
        Drink = 1
    }
}
