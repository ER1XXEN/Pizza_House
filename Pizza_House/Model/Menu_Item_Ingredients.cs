using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    class Menu_Item_Ingredients
    {
        public int Menu_ItemID { get; set; }
        public virtual Menu_Items Menu_Item { get; set; }
        public int IngredientID { get; set; }
        public virtual Ingredient Ingredient { get; set; }

        public bool Selected { get; set; } = true;

        public Menu_Item_Ingredients(int Menu_ItemID, int ingredient)
        {
            this.Menu_ItemID = Menu_ItemID;
            IngredientID = ingredient;
        }
    }
}
