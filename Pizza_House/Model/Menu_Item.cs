using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    #region Class

    internal class Menu_Items
    {
        #region Properties

        private int _Price;
        public string ID { get; set; }
        public string Name { get; set; }

        public string Topping
        {
            get { return string.Join(", ", Ingredients.OrderBy(x => x.Ingredient.Type).Select(x => x.Ingredient.Name)); }
            set { }
        }

        public string Price
        {
            get
            {
                if (Ingredients.Count != 0)
                    return Ingredients.Sum(x => x.Ingredient.Price).ToString() + " USD";
                return _Price + " USD";
            }
            set
            {
                _Price = Convert.ToInt32(value);
            }
        }

        public Menu_Item_Type Type { get; set; }

        #endregion Properties

        #region Virtual Properties

        public virtual List<Menu_Item_Ingredients> Ingredients { get; set; } = new List<Menu_Item_Ingredients>();
        public virtual List<Menu_Item_Sizes> Sizes { get; set; } = new List<Menu_Item_Sizes>();

        #endregion Virtual Properties

        #region Constructors

        public Menu_Items(string ID, string Name, string Toppings, string Price, Menu_Item_Type Type)
        {
            this.ID = ID;
            this.Name = Name;
            this.Topping = Toppings;
            this.Price = Price;
            this.Type = Type;
        }

        public Menu_Items(int ID, string Name, Menu_Item_Type Type)
        {
            this.ID = ID.ToString();
            this.Name = Name;
            this.Type = Type;
        }

        public Menu_Items(int Nr, string name)
        {
            ID = Nr.ToString();
            Name = name;
        }

        public Menu_Items(int ID, List<Menu_Item_Ingredients> ingredients, string Name)
        {
            this.ID = ID.ToString();
            Ingredients = ingredients;
            this.Name = Name;
        }

        #endregion Constructors
    }

    #endregion Class

    #region Enum

    public enum Menu_Item_Type
    {
        Pizza = 0,
        Drink = 1,
    }

    #endregion Enum
}