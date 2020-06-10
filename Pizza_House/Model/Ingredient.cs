namespace Pizza_House.Model
{
    #region Class

    internal class Ingredient
    {
        #region Properties

        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool Selected { get; set; } = false;
        public Ingredient_Type Type { get; set; }

        #endregion Properties

        #region Constructor

        public Ingredient(int id, string name, int type, int price)
        {
            ID = id;
            Name = name;
            Price = price;
            Type = (Ingredient_Type)type;
        }

        #endregion Constructor

        #region Overrides

        public override string ToString()
        {
            return Name + "\t\t" + Price + " USD";
        }

        #endregion Overrides
    }

    #endregion Class

    #region Enum

    public enum Ingredient_Type
    {
        Dough = 0,
        Topping = 1
    }

    #endregion Enum
}