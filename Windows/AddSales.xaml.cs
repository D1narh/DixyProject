using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddSales.xaml
    /// </summary>
    public partial class AddSales : Window
    {
        int auth = 2;
        string Login;

        public AddSales(string log,int a)
        {
            Login = log;
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                int idCat = Category.SelectedIndex + 1;
                Connect c = new Connect();
                string query = "UPDATE [dbo].[category] SET [Sales] = @sales WHERE [Id] = @Id";
                using (c.connect())
                {
                    using (SqlCommand conn = new SqlCommand(query, c.connect()))
                    {
                        conn.Parameters.Add("@sales", SqlDbType.VarChar).Value = Sales.Text;
                        conn.Parameters.Add("@Id", SqlDbType.VarChar).Value = idCat;

                        int ss = conn.ExecuteNonQuery();
                        if (ss > 0)
                        {
                            MessageBox.Show("Вы добавили скидку на категорию: " + Category.SelectedItem);
                            Cabinet cabinet = new Cabinet(Login, auth);
                            cabinet.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Данные о продукте не были обновлены!", "Внимание!");
                        }
                        c.connect().Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Cabinet cabinet = new Cabinet(Login, auth);
            cabinet.Show();
            this.Close();
        }
    }
}
