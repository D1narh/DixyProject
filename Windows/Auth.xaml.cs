using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        private SqlCommand cmd = new SqlCommand();
        private SqlDataReader reader;

        int auth = 0;
        string Logins = "";

        public Auth()
        {
            InitializeComponent();
        }

        private void Authori_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Connect b = new Connect();
                Console.WriteLine(Login.Text + " and " + Password.Text);

                cmd.CommandText = "SELECT * FROM users WHERE Login = '" + Login.Text + "' AND Password = '" + Password.Text + "'";

                Console.WriteLine(cmd.CommandText);
                cmd.Connection = b.connect();
                reader = cmd.ExecuteReader();

                int i = 0;

                while (reader.Read())
                {
                    i++;
                }
                if (i == 1)
                {
                    Cabinet cabinet = new Cabinet(Login.Text,auth);
                    cabinet.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Вы не ввели логин или пароль", "Ошибка аунтификации");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка = " + ex);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Catalog catalog = new Catalog(Logins, auth);
            catalog.Show();
            this.Close();
        }
    }
}

