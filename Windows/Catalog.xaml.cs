using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

using DixyProject.ModelBD;

namespace DixyProject.Windows
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        public ObservableCollection<Product> Phones = new ObservableCollection<Product>();
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        SqlDataReader reader;

        string delete;

        string image, title, cost, discount = "";
        bool discountStatus = false;
        int maxcol = 0;
        int coll = 0;
        int id = 0;


        string price = "";

        int auth = 0;
        string Login;
        int IDD;

        public Catalog(int id,string login,int a)
        {
            auth = a;
            Login = login;
            IDD = id;

            InitializeComponent();

            Start();
        }

        private void ContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {

                TestFirstName();

                InfoProduct info = new InfoProduct(delete,Login,auth);
                info.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Вы не выбрали товар. Ошибка :" + ex.Message);
            }
        }

        private void cabinet_Click(object sender, RoutedEventArgs e)
        {
            if (auth == 0)
            {
                Auth auth = new Auth();
                auth.Show();
                this.Close();
            }
            else
            {
                Cabinet cabinet = new Cabinet(Login, auth);
                cabinet.Show();
                this.Close();
            }
        }

        private void cart_Click(object sender, RoutedEventArgs e)
        {
            Cart cart = new Cart(IDD, Login, auth);
            cart.Show();
            this.Close();
        }

        public void Start()
        {
            Connect b = new Connect();

            cmd = new SqlCommand("select p.Id,p.Name,Price=cast(cast(Price as int) as varchar),p.Image,p.Description,Sales from product p inner join category c on p.Categori_Id = c.Id", b.connect());
            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            adapter.Fill(ds, "Service");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int id = Convert.ToInt32(dr[0].ToString());
                string title = dr[1].ToString();
                string cost = dr[2].ToString();
                string image = dr[3].ToString();
                string description = dr[4].ToString();
                string sales = dr[5].ToString();
                string price = "";
                bool discount2;


                if (sales != "0")
                {
                    discount2 = true;

                    int price1 = int.Parse(cost);
                    int discountasa = int.Parse(sales);

                    int costprice = price1 - (int)Math.Round((double)(price1 * discountasa / 100));
                    price = Convert.ToString(costprice) + "  рублей";
                    sales = "Cкидка " + sales + "%";
                }
                else
                {
                    cost += " рублей";
                    sales = "";
                    discount2 = false;
                }

                string ImagePath = DixyProject.App.AppDir + image + "";

                Phones.Add(new Product
                {
                    Id = id,
                    ImagePath = DixyProject.App.AppDir + image + "",
                    Title = "" + title + "",
                    Price = cost,
                    Description = description,
                    Discount = sales,
                    DiscountStatus = discount2,
                    PriceDis = price
                });
            }
            list.ItemsSource = Phones;
        }
        public void TestFirstName()
        {
            if (list.SelectedItem == null)
                return;
            var _Container = list.ItemContainerGenerator
                .ContainerFromItem(list.SelectedItem);

            foreach (var item in list.Items)
            {

                var _Children = AllChildren(_Container);

                var _FirstName = _Children
                    // only interested in TextBoxes
                    .OfType<Label>()
                    // only interested in FirstName
                    .First(x => x.Name.Equals("ID"));

                // test & set color
                delete = _FirstName.Content.ToString();
            }
        }

        public List<Control> AllChildren(DependencyObject parent)
        {
            var _List = new List<Control>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is Control)
                    _List.Add(_Child as Control);
                _List.AddRange(AllChildren(_Child));
            }
            return _List;
        }
    }
}
