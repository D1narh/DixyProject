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
    /// Логика взаимодействия для Cart.xaml
    /// </summary>
    public partial class Cart : Window
    {
        //Глобальные переменные
        public ObservableCollection<Basket> Baskets = new ObservableCollection<Basket>();
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;

        //Переменная для того чтобы понять какой id у продукта
        string delete;

        //Переменные для передачи на другое окно
        int auth;
        string Logins;
        int IDD;

        //Итоговая цена корзины
        int pprice = 0;

        //стэки Для извлечения товаров
        Stack<int> Price = new Stack<int>();
        Stack<int> Id = new Stack<int>();
        Stack<int> Amount = new Stack<int>();
        public Cart(int id, string log, int a)
        {
            Logins = log;
            auth = a;
            InitializeComponent();
            IDD = id;

            Start();

        }

        public void Start()
        {
            //Очищаем стэк чтобы небыло канфузов
            Price.Clear();
            Id.Clear();
            Amount.Clear();

            Pprice.Content = "";
            pprice = 0;
            Baskets.Clear();
            Connect b = new Connect();

            cmd = new SqlCommand("Select p.Id,p.Name, b.Amount, Price = cast(p.Price * b.Amount as int),p.Image from basket b inner join product p on p.Id = b.Product_Id where b.Client_Id = " + IDD + "", b.connect());
            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            adapter.Fill(ds, "Service");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int id = Convert.ToInt32(dr[0].ToString());
                string name = dr[1].ToString();
                string price = dr[3].ToString();
                string ammount = dr[2].ToString();
                string image = dr[4].ToString();

                //Заполняем стек для того чтобы в случаи покупки, можно было понять какие товары были
                Price.Push(int.Parse(price));
                Amount.Push(int.Parse(ammount));
                Id.Push(id);

                if (ammount == "0")
                {
                    Delete_from_basket();
                }
                else
                {
                    pprice += int.Parse(price);

                    Baskets.Add(new Basket
                    {
                        Id = id,
                        Price = price + " рублей",
                        Name = name,
                        Ammount = ammount,
                        ImagePath = DixyProject.App.AppDir + image + ""
                    });
                }
                list.ItemsSource = Baskets;

            }
            Pprice.Content = pprice.ToString() + " рублей";
        }

        //Удаление товара из корзины если его количество стало = 0
        public void Delete_from_basket()
        {
            Connect b = new Connect();
            string query = "DELETE FROM [dbo].[basket] WHERE [Product_Id] = '" + delete + "'";
            using (b.connect())
            {
                using (SqlCommand conn = new SqlCommand(query, b.connect()))
                {
                    int ss = conn.ExecuteNonQuery();
                    if (ss > 0)
                    {
                        Start();
                    }
                }
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Catalog catalog = new Catalog(IDD, Logins, auth);
            catalog.Show();
            this.Close();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            Basket basket = new Basket();
            try
            {

                Connect c = new Connect();
                string query = "INSERT INTO [dbo].[receipt] VALUES (@client,@date)";
                using (c.connect())
                {
                    using (SqlCommand conn = new SqlCommand(query, c.connect()))
                    {
                        conn.Parameters.Add("@client", SqlDbType.VarChar).Value = IDD;
                        conn.Parameters.Add("@date", SqlDbType.VarChar).Value = DateTime.Now;

                        int ss = conn.ExecuteNonQuery();
                        if (ss > 0)
                        {
                            MessageBox.Show("Спасибо за покупку!\nЧек за покупку будет доступен в соответствующей вкладке!");
                        }
                        else
                        {
                            MessageBox.Show("Произошла ошибка при покупке и выдаче чека!\nПопробуйте еще раз!", "Внимание!");
                        }
                        c.connect().Close();
                    }
                }

                //Переменные куда вставвляются значения из стека
                int Idd;
                string namee;
                int pricee;
                int ammountt;

                for (int i = 0; i < Baskets.Count; i++)
                {
                    Idd = Id.Pop();
                    pricee = Price.Pop();
                    ammountt = Amount.Pop();

                    string query1 = "INSERT INTO [dbo].[receipt_Line] VALUES ((Select TOP(1) Id from receipt ORDER BY ID DESC),@Prod,@amount,@price)";
                    using (c.connect())
                    {
                        using (SqlCommand conn = new SqlCommand(query1, c.connect()))
                        {
                            conn.Parameters.Add("@Prod", SqlDbType.VarChar).Value = Idd;
                            conn.Parameters.Add("@amount", SqlDbType.VarChar).Value = ammountt;
                            conn.Parameters.Add("@price", SqlDbType.VarChar).Value = pricee;

                            int ss1 = conn.ExecuteNonQuery();
                            if (ss1 > 0)
                            {
                                //Nothing
                            }
                            else
                            {
                                MessageBox.Show("Произошла ошибка!\nПопробуйте еще раз!", "Внимание!");
                            }
                            c.connect().Close();
                        }
                    }
                }

                //После того как все было выполненно, все что было в корзине удаляется
                string queryy = "DELETE FROM [dbo].[basket] WHERE [Client_Id] = '" + IDD + "'";
                using (c.connect())
                {
                    using (SqlCommand conn = new SqlCommand(queryy, c.connect()))
                    {
                        int ss = conn.ExecuteNonQuery();
                        if (ss > 0)
                        {
                            MessageBox.Show("Вы приобрели продукты на сумму " + Pprice.Content + " рублей!");
                            Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Product_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TestFirstName();

                InfoProduct info = new InfoProduct(delete, Logins, auth);
                info.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Вы не выбрали товар. Ошибка :" + ex.Message);
            }
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

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            TestFirstName();

            Baskets.Clear();

            Connect b = new Connect();
            string query = "UPDATE [dbo].[basket] SET [Amount] = Amount + 1 WHERE [Product_Id] = '" + delete + "'";
            using (b.connect())
            {
                using (SqlCommand conn = new SqlCommand(query, b.connect()))
                {
                    int ss = conn.ExecuteNonQuery();
                    if (ss > 0)
                    {
                        Start();
                    }
                }
            }
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            TestFirstName();

            Baskets.Clear();

            Connect b = new Connect();
            string query = "UPDATE [dbo].[basket] SET [Amount] = Amount - 1 WHERE [Product_Id] = '" + delete + "'";
            using (b.connect())
            {
                using (SqlCommand conn = new SqlCommand(query, b.connect()))
                {
                    int ss = conn.ExecuteNonQuery();
                    if (ss > 0)
                    {
                        Start();
                    }
                }
            }
        }
    }
}