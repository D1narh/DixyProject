using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    /// Логика взаимодействия для Cabinet.xaml
    /// </summary>
    public partial class Cabinet : Window
    {
        int auth = 1;
        string Login;
        public Cabinet(string login,int a)
        {
            Login = login;
            InitializeComponent();


            Connect b = new Connect();
            SqlCommand myCommand = new SqlCommand($"select * from [dbo].[users] where Login='{login}'", b.connect());
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            StreamWriter name = new StreamWriter("fio.txt");
            using (b.connect())
                while (myReader.Read())
                {
                    FIO.Content += (myReader["Name"].ToString()) + " " + (myReader["First_name"].ToString()) + " " + (myReader["Last_name"].ToString());
                    Date.Content += (myReader["Registr_date"].ToString());
                    Bal.Content += " " + (myReader["Points"].ToString());
                    position.Content = (myReader["Position_Id"].ToString());
                }
            name.Close();
            b.connect().Close();

            if(position.Content.ToString() == "1")
            {
                auth = 2;

                New_product.Visibility = Visibility.Visible;
                New_sales.Visibility = Visibility.Visible;
            }
        }

        private void Exit_Click_1(object sender, RoutedEventArgs e)
        {
            auth = 0;
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Receipt_Click(object sender, RoutedEventArgs e)
        {
            Checks checks = new Checks();
            checks.Show();
            this.Close();
        }

        private void Favorites_Click(object sender, RoutedEventArgs e)
        {
            Favorites favorit = new Favorites();
            favorit.Show();
            this.Close();
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            //BackProduct back = new BackProduct();
            //back.Show();
            //this.Close();
        }

        private void New_product_Click(object sender, RoutedEventArgs e)
        {
            AddEditPtoduct ptoduct = new AddEditPtoduct(null,Login);
            ptoduct.Show();
            this.Close();
        }

        private void Catalog_Click(object sender, RoutedEventArgs e)
        {
            Catalog catalog = new Catalog(Login,auth);
            catalog.Show();
            this.Close();
        }
    }
}
