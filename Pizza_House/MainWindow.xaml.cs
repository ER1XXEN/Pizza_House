using Pizza_House.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        List<Pizza> Pizzas = new List<Pizza>();
        List<Pizza_Ingredients> Pizza_Ingredients = new List<Pizza_Ingredients>();
        List<Pizza_Sizes> Pizza_Sizes = new List<Pizza_Sizes>();
        List<Ingredient> Ingredients = new List<Ingredient>();
        List<Model.Size> Sizes = new List<Model.Size>();
        List<int> Cart_Items = new List<int>();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            SetUp();
        }

        public void SetUp()
        {
            foreach (Canvas item in FindWindowChildren<Canvas>(Body))
            {
                if (item.Name.Split('_')[0].Contains("Menu"))
                    item.Visibility = Visibility.Visible;
                else
                    item.Visibility = Visibility.Hidden;
            }
            GenerateMenu();
        }

        private void GenerateMenu()
        {
            int amount = 20;
            //Ingredients
            for (int i = 0; i < amount; i++)
            {
                Ingredients.Add(new Ingredient(i, "Ing " + i, rnd.Next(10,100)));
            }
            //Sizes
            Sizes.Add(new Model.Size(1, "Normal", 0));
            Sizes.Add(new Model.Size(2, "Large", 50));
            Sizes.Add(new Model.Size(3, "ExtraLarge", 100));
            Sizes.Add(new Model.Size(4, "Child", -50));
            //Pizzas
            for (int i = 1; i < amount; i++)
            {
                Pizza _Pizza = new Pizza(i, "Pizza " + i);
                for (int x = 0; x < rnd.Next(4, 6); x++)
                {
                    Pizza_Ingredients _Ingredients = new Pizza_Ingredients(i, rnd.Next(0, Ingredients.Count));
                    _Ingredients.Pizza = _Pizza;
                    _Ingredients.Ingredient = Ingredients[_Ingredients.IngredientID];
                    Pizza_Ingredients.Add(_Ingredients);
                }
                Pizza_Sizes.Add(new Model.Pizza_Sizes(_Pizza, Sizes[0]));
                for (int x = 0; x < rnd.Next(0, 5); x++)
                {
                    Pizza_Sizes _Size = new Pizza_Sizes(_Pizza, Sizes[rnd.Next(0, Sizes.Count)]);
                    Pizza_Sizes.Add(_Size);
                }
                _Pizza.Ingredients = Pizza_Ingredients.Where(x => x.PizzaID == i).ToList();
                _Pizza.Sizes = Pizza_Sizes.Where(x => x.PizzaID == i).Distinct().ToList();
                Pizzas.Add(_Pizza);
            }
            Menu_listbox.ItemsSource = Pizzas;
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
        private void Add_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Change_Panel(object sender, MouseButtonEventArgs e)
        {

            foreach (Canvas item in FindWindowChildren<Canvas>(Body))
            {
                if (item.Name.Split('_')[0].Contains(((Image)sender).Tag.ToString()))
                    item.Visibility = Visibility.Visible;
                else
                    item.Visibility = Visibility.Hidden;
            }
        }

        private void Menu_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Canvas item in FindWindowChildren<Canvas>(Body))
            {
                if (item.Name.Split('_')[0].Contains("Details"))
                    item.Visibility = Visibility.Visible;
                else
                    item.Visibility = Visibility.Hidden;
            }
            Menu_Id = Menu_listbox.SelectedIndex;
            SetDetails();
        }
        private void SetDetails()
        {
            Desc_Img.Source = new BitmapImage(new Uri("http://placekitten.com/" + rnd.Next(200, 900) + "/" + rnd.Next(200, 900)));
            Sizes_combo.ItemsSource = Pizzas[Menu_Id].Sizes.Select(x => x.Size);
            Sizes_combo.SelectedIndex = 0;
            Details_Name_lbl.Content = Pizzas[Menu_Id].Name;
            Desc_Price_lbl.Content = "Price : " + Pizzas[Menu_Id].Price + " USD";
            Ingredient_List.ItemsSource = Pizzas[Menu_Id].Ingredients.Select(x => x.Ingredient.Name);
        }

        private void Ingredient_List_SelectionChanged(object sender, SelectionChangedEventArgs e)=>Ingredient_List.SelectedIndex = -1;

        private void Sizes_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int DefPrize = Pizzas[Menu_Id].Price;
            int PrizeMod = Pizzas[Menu_Id].Sizes.ElementAt(Sizes_combo.SelectedIndex).Size.PriceMod;
            float tesP = DefPrize + (DefPrize * PrizeMod / 100);
            //Desc_Price_lbl.Content = "Price : " + (DefPrize + (DefPrize*(PrizeMod/100)))+ " USD";
            Desc_Price_lbl.Content = "Price : " + tesP.ToString("N2")+ " USD";
        }
    }
}
