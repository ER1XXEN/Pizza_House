using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    class Discount_Set
    {
        public string _Items
        {
            get { return GetItems(); }
            set { }
        }
        public string _Target
        {
            get { return GetTarget(); }
            set { }
        }
        public int ID { get; set; }
        public string Name { get; set; } = "Discount";
        public Discount_Type Type { get; set; }
        public virtual List<Menu_Item_Type> Items_Needed { get; set; }
        public Ingredient_Type? Item_Type { get; set; } = null;
        public int Percentage { get; set; }
        public bool Top { get; set; } = false;

        public float _Price { get; set; } = 0;

        public string GetTarget()
        {
            string Res = this.Type == Discount_Type.Percentage ? (this.Percentage.ToString() + "%") : ((this.Top ? "Most expensive " : "Cheapest ") + this.Item_Type.ToString());
            return Res;
        }
        public string GetItems()
        {
            int Pizzas = this.Items_Needed.Count(x => x == Menu_Item_Type.Pizza);
            int Drinks = this.Items_Needed.Count(x => x == Menu_Item_Type.Drink);
            string Res = (Pizzas != 0 ? Pizzas + "x Pizzas" : "") + ((Pizzas != 0 && Drinks != 0) ? " & " : "") + (Drinks != 0 ? Drinks + "x Drinks" : "");
            return Res;
        }

        public Discount_Set(int ID, Discount_Type Type, List<Menu_Item_Type> Items_Needed, Ingredient_Type? Item_Type, int Percentage = 0, bool Top = false)
        {
            this.ID = ID;
            this.Type = Type;
            this.Items_Needed = Items_Needed;
            this.Item_Type = Item_Type;
            this.Percentage = Percentage;
            this.Top = Top;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
    public enum Discount_Type
    {
        Percentage = 0,
        Item = 1
    }
}
