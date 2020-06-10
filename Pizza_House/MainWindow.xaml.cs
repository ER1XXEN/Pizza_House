using Pizza_House.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pizza_House
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variable

        #region ints

        private int Menu_Id = -1;

        #endregion ints

        #region Classes

        private Random rnd = new Random();
        private Cart Cart = new Cart();

        #endregion Classes

        #region Lists

        private List<Menu_Items> Menu_Items = new List<Menu_Items>();
        private List<Menu_Item_Ingredients> Pizza_Ingredients = new List<Menu_Item_Ingredients>();
        private List<Menu_Item_Sizes> Menu_Item_Sizes = new List<Menu_Item_Sizes>();
        private List<Ingredient> Ingredients = new List<Ingredient>();
        private List<Model.Size> Sizes = new List<Model.Size>();
        private List<Discount_Set> Discounts = new List<Discount_Set>();

        #endregion Lists

        #endregion Variable

        #region Pre-made_Methods

        /// <summary>
        /// Initializes controls
        /// Begins setup();
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            SetUp();
        }

        #endregion Pre-made_Methods

        #region Control_Methods

        /// <summary>
        /// Change panels to Details_Panel
        /// Sets Detail information according to selected Menu_Item
        /// Resets selectedIndex of Menu_listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Menu_listbox.SelectedIndex == -1)
                return;
            Change_Panel("Details");
            Menu_Id = Menu_listbox.SelectedIndex;
            SetDetails();
            Menu_listbox.SelectedIndex = -1;
        }

        /// <summary>
        /// Toggles whether or not user wish to have that topping on pizza
        /// Can't toggle dough
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ingredient_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Ingredient_List.SelectedIndex == -1)
                return;
            Menu_Item_Ingredients item = (Menu_Item_Ingredients)Ingredient_List.SelectedItem;
            if (item.Ingredient.Type != Ingredient_Type.Dough)
                item.Selected = !item.Selected;
            Ingredient_List.Items.Refresh();
            Ingredient_List.SelectedIndex = -1;
        }

        /// <summary>
        /// Updates price of Menu_Item based on Size.modifier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sizes_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Sizes_combo.SelectedIndex == -1)
                return;
            float DefPrize = Convert.ToInt32(Menu_Items[Menu_Id].Price.Split(' ')[0]);
            float PrizeMod = Menu_Items[Menu_Id].Sizes.OrderBy(x => x.Size.PriceMod).ElementAt(Sizes_combo.SelectedIndex).Size.PriceMod;
            float tesP = DefPrize + (DefPrize * (PrizeMod / 100));
            //Desc_Price_lbl.Content = "Price : " + (DefPrize + (DefPrize*(PrizeMod/100)))+ " USD";
            Desc_Price_lbl.Content = "Price : " + tesP.ToString("N2") + " USD";
        }

        /// <summary>
        /// Change image to hover type
        /// And change cursor to hand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Img_MouseEnter(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Assets\\Hover_" + img.Tag.ToString().ToLower() + ".png"));
            Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Change image to non-hover type
        /// And change cursor to cursor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Img_MouseLeave(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Assets\\" + img.Tag.ToString().ToLower() + ".png"));
            Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Change cursor to 'Hand'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseEnter_Hover(object sender, MouseEventArgs e) => Cursor = Cursors.Hand;

        /// <summary>
        /// Change cursor to 'Arrow'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeave_Hover(object sender, MouseEventArgs e) => Cursor = Cursors.Arrow;

        /// <summary>
        /// Toggle if selected topping is a part of custom Pizza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Custom_Topping_Listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Custom_Topping_Listbox.SelectedIndex == -1)
                return;
            Ingredient item = (Ingredient)Custom_Topping_Listbox.SelectedItem;
            item.Selected = !item.Selected;
            Custom_Topping_Listbox.Items.Refresh();
            Custom_Topping_Listbox.SelectedIndex = -1;
            FindPriceCustom();
        }

        /// <summary>
        /// Add custom made pizza to Cart
        /// Then resets custom panel data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            Change_Panel("Home");
            Cart_Item Cart_Item = new Cart_Item();
            Cart_Item.Size = (Model.Size)Custom_Size_Combo.SelectedItem;
            Cart_Item.Menu_Item = new Menu_Items(Menu_Items.Count() + 1, "Custom Pizza");

            List<Ingredient> ingredients = Ingredients.Where(x => x.Type == Ingredient_Type.Topping && x.Selected).ToList();
            ingredients.Add((Ingredient)Custom_Dough_Combo.SelectedItem);
            List<Menu_Item_Ingredients> menu_Item_Ingredients = new List<Menu_Item_Ingredients>();
            foreach (Ingredient item in ingredients)
            {
                Menu_Item_Ingredients newIngredient = new Menu_Item_Ingredients(Menu_Items.Count() + 1, item.ID);
                newIngredient.Menu_Item = Cart_Item.Menu_Item;
                newIngredient.Ingredient = item;
                menu_Item_Ingredients.Add(newIngredient);
            }
            Cart_Item.Menu_Item.Ingredients = menu_Item_Ingredients;

            Cart.Cart_Items.Add(Cart_Item);
            foreach (var topping in Ingredients.Where(x => x.Type == Ingredient_Type.Topping && x.Selected))
                topping.Selected = false;

            PrepareCustom();
            CheckCart();
        }

        /// <summary>
        /// Change panel depending on tag of 'triggered' Control element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_Panel(object sender, MouseButtonEventArgs e) => Change_Panel(((Image)sender).Tag.ToString());

        /// <summary>
        /// Updates price of custom pizza if Custom_Size_Combo and Custom_Dough_Combo have  an item selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Custom_Dough_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Custom_Size_Combo.SelectedIndex == -1 || Custom_Dough_Combo.SelectedIndex == -1)
                return;
            FindPriceCustom();
        }

        /// <summary>
        /// prepare Amount_Panel with required data
        /// change panel to Amount_Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Menu_Id == -1)
                return;
            Pizza_amount_Con_lbl.Content = string.Format("How many of \n{0} \ndo you want?", Menu_Items[Menu_Id].Name);
            Amount_txt.Text = "1";
            Change_Panel("Amount");
            Amount_txt.Focus();
        }

        /// <summary>
        /// Prevents typing of letters and symbols, as well as preventing content.length to surpass 3 digits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Amount_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            Amount_txt.Text = Regex.Replace(Amount_txt.Text, "[^0-9]+", "");
            if (Amount_txt.Text.Length > 3)
                Amount_txt.Text = Amount_txt.Text.Substring(0, 3);
            Amount_txt.SelectionStart = Amount_txt.Text.Length;
        }

        /// <summary>
        /// Add specified amount of selected product based on amount on Amount_txt with specified data
        /// then reset all changed data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Amount_Btn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Convert.ToInt32(Amount_txt.Text); i++)
            {
                Cart_Item NewCartItem = new Cart_Item();
                NewCartItem.Menu_Item = Menu_Items[Menu_Id];
                NewCartItem.RemovedIngredients = Menu_Items[Menu_Id].Ingredients.Where(x => !x.Selected).Select(x => x.Ingredient).ToList();
                NewCartItem.Size = (Model.Size)Sizes_combo.SelectedItem;
                float PizzaDefPrice = Convert.ToInt32(NewCartItem.Menu_Item.Price.Split(' ')[0]);
                NewCartItem._Price = PizzaDefPrice + (PizzaDefPrice * (NewCartItem.Size.PriceMod / 100));
                Cart.Cart_Items.Add(NewCartItem);
            }
            Change_Panel("Menu");
            CheckCart();
        }

        /// <summary>
        /// Reset SelectedIndex in Discount_listbox to -1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Discount_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e) => Discount_listbox.SelectedIndex = -1;

        #endregion Control_Methods

        #region Custom_Methods

        /// <summary>
        /// Start default procedures
        /// </summary>
        public void SetUp()
        {
            Change_Panel("Home");
            GenerateMenu();
            PrepareCustom();
            PrepareDiscount();
        }

        /// <summary>
        /// Prepare data for Custom_Panel
        /// </summary>
        private void PrepareCustom()
        {
            Custom_Topping_Listbox.ItemsSource = Ingredients.Where(x => x.Type == Ingredient_Type.Topping).ToList();
            Custom_Dough_Combo.ItemsSource = Ingredients.Where(x => x.Type == Ingredient_Type.Dough).OrderBy(x => x.Price);
            Custom_Size_Combo.ItemsSource = Sizes.Where(x => x.Type == Size_Type.Pizza).OrderBy(x => x.PriceMod);
            Custom_Dough_Combo.SelectedIndex = 1;
            Custom_Size_Combo.SelectedIndex = 1;
            FindPriceCustom();
        }

        /// <summary>
        /// Prepare and set data for Discount_Panel
        /// </summary>
        private void PrepareDiscount()
        {
            #region Discounts

            Discounts.Add(new Discount_Set(Discounts.Count() + 1, Discount_Type.Percentage, new List<Menu_Item_Type>() { Menu_Item_Type.Pizza, Menu_Item_Type.Drink, Menu_Item_Type.Drink }, null, 20));
            Discounts.Add(new Discount_Set(Discounts.Count() + 1, Discount_Type.Percentage, new List<Menu_Item_Type>() { Menu_Item_Type.Pizza, Menu_Item_Type.Drink }, null, 10));
            Discounts.Add(new Discount_Set(Discounts.Count() + 1, Discount_Type.Item, new List<Menu_Item_Type>() { Menu_Item_Type.Pizza, Menu_Item_Type.Pizza, Menu_Item_Type.Drink, Menu_Item_Type.Drink }, Ingredient_Type.Dough, 0, true));
            Discounts.Add(new Discount_Set(Discounts.Count() + 1, Discount_Type.Item, new List<Menu_Item_Type>() { Menu_Item_Type.Pizza, Menu_Item_Type.Pizza, Menu_Item_Type.Drink }, Ingredient_Type.Topping, 0, true));

            #endregion Discounts

            Discount_listbox.ItemsSource = Discounts;
        }

        /// <summary>
        /// Generate required dummy data for Menu_Items and sets it
        /// </summary>
        private void GenerateMenu()
        {
            int amount = 10;

            #region Ingredients

            for (int i = 0; i < amount; i++)
            {
                Ingredient _Ingredient = new Ingredient(i, "Ing " + i, rnd.Next(0, 2), rnd.Next(10, 50));
                if (_Ingredient.Type == Ingredient_Type.Dough)
                    _Ingredient.Name = "Dou " + i;
                Ingredients.Add(_Ingredient);
            }

            #endregion Ingredients

            #region Sizes

            Sizes.Add(new Model.Size(1, "Normal", 0, Size_Type.Pizza));
            Sizes.Add(new Model.Size(2, "Large", 50, Size_Type.Pizza));
            Sizes.Add(new Model.Size(3, "ExtraLarge", 100, Size_Type.Pizza));
            Sizes.Add(new Model.Size(4, "Child", -50, Size_Type.Pizza));
            Sizes.Add(new Model.Size(5, "Small", -20, Size_Type.Drink));
            Sizes.Add(new Model.Size(6, "Medium", 0, Size_Type.Drink));
            Sizes.Add(new Model.Size(7, "Large", 20, Size_Type.Drink));

            #endregion Sizes

            #region Pizzas

            for (int i = 1; i < amount; i++)
            {
                Menu_Items _Pizza = new Menu_Items(Menu_Items.Count + 1, "Pizza " + i);
                _Pizza.Type = Menu_Item_Type.Pizza;
                for (int x = 0; x < rnd.Next(7, 10); x++)
                {
                    Menu_Item_Ingredients _Ingredients = new Menu_Item_Ingredients(i, rnd.Next(0, Ingredients.Count));
                    _Ingredients.Menu_Item = _Pizza;
                    _Ingredients.Ingredient = Ingredients[_Ingredients.IngredientID];
                    if (Pizza_Ingredients.Where(y => y.Menu_Item == _Pizza && y.Ingredient.Type == Ingredient_Type.Dough).Count() == 1 && _Ingredients.Ingredient.Type == Ingredient_Type.Dough)
                        x--;
                    else
                        Pizza_Ingredients.Add(_Ingredients);
                }
                foreach (Model.Size Size in Sizes.Where(x => x.Type == Size_Type.Pizza).ToList())
                    Menu_Item_Sizes.Add(new Model.Menu_Item_Sizes(_Pizza, Size));

                _Pizza.Sizes = Menu_Item_Sizes.Where(x => x.Menu_ItemID == i).Distinct().ToList();
                _Pizza.Ingredients = Pizza_Ingredients.Where(x => x.Menu_ItemID == i).ToList();

                Menu_Items.Add(_Pizza);
            }

            #endregion Pizzas

            #region Drinks

            for (int i = 1; i < amount; i++)
            {
                Menu_Items _Drink = new Menu_Items(Menu_Items.Count + 1, "Drink " + i, Menu_Item_Type.Drink);
                foreach (Model.Size Size in Sizes.Where(x => x.Type == Size_Type.Drink).ToList())
                    Menu_Item_Sizes.Add(new Model.Menu_Item_Sizes(_Drink, Size));

                _Drink.Price = string.Format("{0}", rnd.Next(20, 50));
                _Drink.Sizes = Menu_Item_Sizes.Where(x => x.Menu_ItemID == Menu_Items.Count + 1).Distinct().ToList();
                Menu_Items.Add(_Drink);
            }

            #endregion Drinks

            Menu_listbox.ItemsSource = Menu_Items;
        }

        /// <summary>
        /// Get price for Custom Pizza depending on selected data
        /// </summary>
        private void FindPriceCustom()
        {
            int ToppingPrice = Custom_Topping_Listbox.Items.Cast<Ingredient>().Where(x => x.Selected).ToList().Sum(x => x.Price);
            Ingredient Dough = (Ingredient)(Custom_Dough_Combo.SelectedItem);
            float DefPrize = ToppingPrice + Dough.Price;
            Model.Size Size = (Model.Size)(Custom_Size_Combo.SelectedItem);
            float PrizeMod = Size.PriceMod;
            float FinalPrice = DefPrize + (DefPrize * PrizeMod / 100);
            Custom_Price_lbl.Content = FinalPrice + " USD";
        }

        /// <summary>
        /// Updates information required for Cart
        /// </summary>
        private void CheckCart()
        {
            Cart.Discounts = new List<Discount_Set>();
            int PizzaAmount = Cart.Cart_Items.Where(x => x.Menu_Item.Type == Menu_Item_Type.Pizza).Sum(x => x.Amount);
            int DrinkAmount = Cart.Cart_Items.Where(x => x.Menu_Item.Type == Menu_Item_Type.Drink).Sum(x => x.Amount);
            List<Discount_Set> Discount = Discounts.OrderByDescending(x => x.Items_Needed.Count).ToList();
            while (Discount.Any(x => (x.Items_Needed.Count(y => y == Menu_Item_Type.Pizza) <= PizzaAmount) && (x.Items_Needed.Count(y => y == Menu_Item_Type.Drink) <= DrinkAmount)))
            {
                var d = Discount.OrderByDescending(x => x.Type).FirstOrDefault(x => (x.Items_Needed.Count(y => y == Menu_Item_Type.Pizza) <= PizzaAmount) && (x.Items_Needed.Count(y => y == Menu_Item_Type.Drink) <= DrinkAmount));
                PizzaAmount = PizzaAmount - d.Items_Needed.Where(x => x == Menu_Item_Type.Pizza).Count();
                DrinkAmount = DrinkAmount - d.Items_Needed.Where(x => x == Menu_Item_Type.Drink).Count();
                d._Price = 0;
                Cart.Discounts.Add(d);
            }
            foreach (Discount_Set item in Cart.Discounts.OrderByDescending(x => x.Type).ThenByDescending(x => x.Percentage))
            {
                if (item.Type == Discount_Type.Item)
                {
                    Ingredient_Type t = (Ingredient_Type)item.Item_Type;
                    List<Ingredient> Ingredients = Cart._Ingredients.Where(x => x.Type == t).ToList();
                    int da = Cart.Discounts.Where(x => x.Item_Type == t && x._Price != 0 && x.Top == item.Top).ToList().Count();
                    if (item.Top)
                        Ingredients = Ingredients.OrderByDescending(x => x.Price).Skip(da).ToList();
                    else
                        Ingredients = Ingredients.OrderBy(x => x.Price).Skip(da).ToList();
                    item._Price = -Ingredients.FirstOrDefault(x => x.Type == t).Price;
                }
                else
                {
                    float p = Cart.Price * ((float)item.Percentage / 100);
                    item._Price = -p;
                    Cart.Price = Cart.Price - p;
                }
            }

            List<object> OrderItems = Cart.Cart_Items.ToList<object>();
            foreach (object item in Cart.Discounts)
                OrderItems.Add(item);
            Cart_listbox.ItemsSource = OrderItems;
            Cart_listbox.Items.Refresh();
            Order_Price_lbl.Content = Cart.Price + " USD";
        }

        /// <summary>
        /// Change panel to Targeted panel
        /// </summary>
        /// <param name="Panel">Panel to switch to</param>
        private void Change_Panel(string Panel)
        {
            foreach (Canvas item in FindWindowChildren<Canvas>(Body))
            {
                if (item.Name.Split('_')[0].Contains(Panel))
                    item.Visibility = Visibility.Visible;
                else
                    item.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Set Details for selected Product in Menu
        /// </summary>
        private void SetDetails()
        {
            foreach (var topping in Menu_Items[Menu_Id].Ingredients.Where(x => !x.Selected))
                topping.Selected = true;
            Desc_Img.Source = new BitmapImage(new Uri("http://placekitten.com/" + rnd.Next(200, 900) + "/" + rnd.Next(200, 900)));
            Sizes_combo.ItemsSource = Menu_Items[Menu_Id].Sizes.Select(x => x.Size).OrderBy(x => x.PriceMod);
            Sizes_combo.SelectedIndex = Menu_Items[Menu_Id].Sizes.Select(x => x.Size).OrderBy(x => x.PriceMod).ToList().FindIndex(x => x.PriceMod == 0);
            Details_Name_lbl.Content = Menu_Items[Menu_Id].Name;
            Desc_Price_lbl.Content = "Price : " + Convert.ToInt32(Menu_Items[Menu_Id].Price.Split(' ')[0]).ToString("N2") + " USD";
            Ingredient_List.ItemsSource = Menu_Items[Menu_Id].Ingredients.OrderBy(x => x.Ingredient.Type);
        }

        #endregion Custom_Methods

        #region Helper_Methods

        /// <summary>
        /// Get controls of type in children of control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindWindowChildren<T>(DependencyObject dObj) where T : DependencyObject
        {
            if (dObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dObj); i++)
                {
                    DependencyObject ch = VisualTreeHelper.GetChild(dObj, i);
                    if (ch != null && ch is T)
                    {
                        yield return (T)ch;
                    }

                    foreach (T nestedChild in FindWindowChildren<T>(ch))
                    {
                        yield return nestedChild;
                    }
                }
            }
        }

        #endregion Helper_Methods
    }
}