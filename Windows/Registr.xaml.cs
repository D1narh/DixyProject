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
    /// Логика взаимодействия для Registr.xaml
    /// </summary>
    public partial class Registr : Window
    {

        private SqlCommand cmd = new SqlCommand();
        private SqlDataReader reader;

        int Auth = 0;

        public Registr()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Close();
        }

        private void regist_Click(object sender, RoutedEventArgs e)
        {
            Connect b = new Connect();
            Console.WriteLine(Login.Text + " and " + Password.Text);

            cmd.CommandText = "SELECT * FROM [dbo].[users] WHERE Login = '" + Login.Text + "'";

            Console.WriteLine(cmd.CommandText);
            cmd.Connection = b.connect();
            reader = cmd.ExecuteReader();

            int i = 0;

            while (reader.Read())
            {
                i++;
            }
            if (i >= 1)
            {
                MessageBox.Show("Аккаунт с таких логином уже существует", "Ошибка");
            }
            else
            {
                string query = "INSERT INTO [dbo].[users](Login,Password,Registr_date) VALUES(@Login,@Password,@Registr_date)";
                using (b.connect())
                {

                    using (SqlCommand conn = new SqlCommand(query, b.connect()))
                    {
                        conn.Parameters.Add("@Login", SqlDbType.VarChar).Value = Login.Text;
                        conn.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password.Text;
                        conn.Parameters.Add("@Registr_date", SqlDbType.VarChar).Value = System.DateTime.Now;


                        int ss = conn.ExecuteNonQuery();
                        if (ss > 0)
                        {
                            Cabinet personal = new Cabinet(Login.Text,Auth);
                            personal.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Noke");
                        }
                        b.connect().Close();
                    }
                }
            }
        }

        private void auth_Click(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Close();
        }
    }
}
