using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    class Cart
    {
        public virtual List<Cart_Item> Cart_Items { get; set; } = new List<Cart_Item>();
        public virtual List<Discount_Set> Discounts { get; set; } = new List<Discount_Set>();
        public float Price
        {
            get
            {
                float DefPrice = Cart_Items.Sum(x => x.Price * x.Amount);
                return DefPrice;
            }
            set
            { }
        }
    }
}
