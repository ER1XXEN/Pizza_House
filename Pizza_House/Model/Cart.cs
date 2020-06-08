using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    class Cart
    {
        public virtual List<Cart_Item> Cart_Items { get; set; }
        public virtual List<Discount_Set> Discounts { get; set; }
        public float Price { get; set; }
    }
}
