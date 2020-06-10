using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    #region Class
    class Cart
    {
        #region Properties
        public float _Price { get; set; } = 0;
        public float Price
        {
            get
            {
                float DefPrice = _Price;
                if (DefPrice == 0)
                    DefPrice = this.Cart_Items.Sum(x => x._Price * x.Amount) + this.Discounts.Where(x => x.Type == Discount_Type.Item).Sum(x => x._Price);
                return DefPrice;
            }
            set
            {
                _Price = value;
            }
        }
        #endregion
        #region Virtual Properties
        public virtual List<Cart_Item> Cart_Items { get; set; } = new List<Cart_Item>();
        public virtual List<Discount_Set> Discounts { get; set; } = new List<Discount_Set>();
        #endregion
        #region List Properties
        public List<Ingredient> _Ingredients { get => Cart_Items.SelectMany(x => x._Ingredients).OrderBy(x => x.Price).ToList(); set { } }
        #endregion
    }
    #endregion
}
