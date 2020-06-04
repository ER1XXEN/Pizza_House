using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    class Cart_Item
    {
        public Menu_Items Menu_Item { get; set; }
        public virtual List<Ingredient> RemovedIngredients { get; set; }
        public Size Size { get; set; }
        public float Price
        {
            get
            {
                float DefPrice = Menu_Item.Ingredients.Sum(x => x.Ingredient.Price);
                float PriceMod = Size.PriceMod;
                return DefPrice + (DefPrice*(PriceMod/100));
            }
            set { }
        }
        public int Amount { get; set; }
    }
}
