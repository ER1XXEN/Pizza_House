using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    class Cart_Item
    {
        //public string _Price { get { return Price + " USD"; } set { } }
        public Menu_Items Menu_Item { get; set; }
        public List<Ingredient> RemovedIngredients { get; set; } = new List<Ingredient>();
        public List<Ingredient> _Ingredients { get { return this.Menu_Item.Ingredients.Select(x => x.Ingredient).Except(RemovedIngredients).ToList(); } set { } }
        public Size Size { get; set; }
        public string Name { get { return Menu_Item.Name; } set { } }
        public int Amount { get; set; } = 1;
        public float _Price
        {
            get
            {
                float DefPrice = Convert.ToInt32(Menu_Item.Price.Split(' ')[0]);
                float PriceMod = Size.PriceMod;
                return DefPrice + (DefPrice * (PriceMod / 100));
            }
            set { }
        }

    }
}
