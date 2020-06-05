using Pizza_House.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pizza_House
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variable
        int Menu_Id = -1;
        Random rnd = new Random();
        List<Menu_Items> Menu_Items = new List<Menu_Items>();
        List<Menu_Item_Ingredients> Pizza_Ingredients = new List<Menu_Item_Ingredients>();
        List<Menu_Item_Sizes> Menu_Item_Sizes = new List<Menu_Item_Sizes>();
        List<Ingredient> Ingredients = new List<Ingredient>();
        List<Model.Size> Sizes = new List<Model.Size>();
        List<Cart_Item> Cart_Content = new List<Cart_Item>();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            SetUp();
        }

        public void SetUp()
        {
            Change_Panel("Home");
            GenerateMenu();
            PrepareCustom();
        }

        private void PrepareCustom()
        {
            Custom_Topping_Listbox.ItemsSource = Ingredients.Where(x => x.Type == Ingredient_Type.Topping).ToList();
            Custom_Dough_Combo.ItemsSource = Ingredients.Where(x => x.Type == Ingredient_Type.Dough).OrderBy(x => x.Price);
            Custom_Size_Combo.ItemsSource = Sizes.Where(x => x.Type == Size_Type.Pizza).OrderBy(x => x.PriceMod);
            Custom_Dough_Combo.SelectedIndex = 1;
            Custom_Size_Combo.SelectedIndex = 1;
            FindPriceCustom();
        }

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
            #endregion
            #region Sizes
            Sizes.Add(new Model.Size(1, "Normal", 0, Size_Type.Pizza));
            Sizes.Add(new Model.Size(2, "Large", 50, Size_Type.Pizza));
            Sizes.Add(new Model.Size(3, "ExtraLarge", 100, Size_Type.Pizza));
            Sizes.Add(new Model.Size(4, "Child", -50, Size_Type.Pizza));
            Sizes.Add(new Model.Size(5, "Small", -20, Size_Type.Drink));
            Sizes.Add(new Model.Size(6, "Medium", 0, Size_Type.Drink));
            Sizes.Add(new Model.Size(7, "Large", 20, Size_Type.Drink));
            #endregion
            #region Pizzas
            for (int i = 1; i < amount; i++)
            {
                Menu_Items _Pizza = new Menu_Items(Menu_Items.Count+1, "Pizza " + i);
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

                _Pizza.Ingredients = Pizza_Ingredients.Where(x => x.Menu_ItemID == i).ToList();
                _Pizza.Sizes = Menu_Item_Sizes.Where(x => x.Menu_ItemID == i).Distinct().ToList();
                Menu_Items.Add(_Pizza);
            }
            #endregion
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
            #endregion
            Menu_listbox.ItemsSource = Menu_Items;
        }

        /// <summary>
        /// Get controls of type
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

        private void Menu_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Menu_listbox.SelectedIndex == -1)
                return;
            Change_Panel("Details");
            Menu_Id = Menu_listbox.SelectedIndex;
            SetDetails();
            Menu_listbox.SelectedIndex = -1;
        }
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

        private void Menu_Img_MouseEnter(object sender, MouseEventArgs e)
        {
             Image img = (Image)sender;
            img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Assets\\Hover_" + img.Tag.ToString().ToLower() + ".png"));
            Cursor = Cursors.Hand;
        }

        private void Menu_Img_MouseLeave(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Assets\\" + img.Tag.ToString().ToLower() + ".png"));
            Cursor = Cursors.Arrow;
        }

        private void MouseEnter_Hover(object sender, MouseEventArgs e) => Cursor = Cursors.Hand;

        private void MouseLeave_Hover(object sender, MouseEventArgs e) => Cursor = Cursors.Arrow;

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


        private void Add_Btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            Change_Panel("Home");
            foreach (var topping in Ingredients.Where(x => x.Type == Ingredient_Type.Topping && x.Selected))
                topping.Selected = false;
            PrepareCustom();

        }

        private void Change_Panel(object sender, MouseButtonEventArgs e) => Change_Panel(((Image)sender).Tag.ToString());

        private void Custom_Dough_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Custom_Size_Combo.SelectedIndex == -1 || Custom_Dough_Combo.SelectedIndex == -1)
                return;
            FindPriceCustom();
        }

        private void Add_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Menu_Id == -1)
                return;
            Pizza_amount_Con_lbl.Content = string.Format("How many of \n{0} \ndo you want?", Menu_Items[Menu_Id].Name);
            Amount_txt.Text = "1";
            Change_Panel("Amount");
            Amount_txt.Focus();
        }

        private void Amount_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            Amount_txt.Text = Regex.Replace(Amount_txt.Text, "[^0-9]+", "");
            if (Amount_txt.Text.Length > 3)
                Amount_txt.Text = Amount_txt.Text.Substring(0, 3);
            Amount_txt.SelectionStart = Amount_txt.Text.Length;
        }

        private void Add_Amount_Btn_Click(object sender, RoutedEventArgs e)
        {
            Cart_Item NewCartItem = new Cart_Item();
            NewCartItem.Menu_Item = Menu_Items[Menu_Id];
            NewCartItem.RemovedIngredients = Menu_Items[Menu_Id].Ingredients.Where(x => !x.Selected).Select(x => x.Ingredient).ToList();
            NewCartItem.Size = (Model.Size)Sizes_combo.SelectedItem;
            NewCartItem.Amount = Convert.ToInt32(Amount_txt.Text);
            float PizzaDefPrice = Convert.ToInt32(NewCartItem.Menu_Item.Price.Split(' ')[0]);
            NewCartItem.Price = PizzaDefPrice + (PizzaDefPrice * (NewCartItem.Size.PriceMod / 100));
            Change_Panel("Menu");
        }
    }
}
