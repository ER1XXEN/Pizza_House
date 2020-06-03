using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    class Pizza_Ingredients
    {
        public int PizzaID { get; set; }
        public virtual Pizza Pizza { get; set; }
        public int IngredientID { get; set; }
        public virtual Ingredient Ingredient { get; set; }

        public Pizza_Ingredients(int pizza, int ingredient)
        {
            PizzaID = pizza;
            IngredientID = ingredient;
        }
    }
}
