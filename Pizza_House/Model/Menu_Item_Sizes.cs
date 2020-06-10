using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_House.Model
{
    #region Class

    internal class Menu_Item_Sizes
    {
        #region Properties

        public virtual Menu_Items Menu_Items { get; set; }
        public int Menu_ItemID { get; set; }
        public virtual Size Size { get; set; }
        public int SizeID { get; set; }

        #endregion Properties

        #region Constructors

        public Menu_Item_Sizes(Menu_Items Menu_Items, Size size)
        {
            this.Menu_Items = Menu_Items;
            Menu_ItemID = Convert.ToInt32(Menu_Items.ID);
            Size = size;
            SizeID = size.ID;
        }

        #endregion Constructors
    }

    #endregion Class
}