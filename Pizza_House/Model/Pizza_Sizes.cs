using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    class Pizza_Sizes
    {
        public virtual Pizza Pizza { get; set; }
        public int PizzaID { get; set; }
        public virtual Size Size { get; set; }
        public int SizeID { get; set; }

        public Pizza_Sizes(Pizza pizza, Size size)
        {
            Pizza = pizza;
            PizzaID = pizza.ID;
            Size = size;
            SizeID = size.ID;
        }
    }
}
