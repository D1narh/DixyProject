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
    /// Логика взаимодействия для BackProduct.xaml
    /// </summary>
    public partial class BackProduct : Window
    {
        string a;

        public BackProduct(string a)
        {
            InitializeComponent();
            id.Text = a;

            Connect c = new Connect();
            DataTable buffer = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand("SELECT Id, Date_add, (SELECT Price FROM receipt_Line WHERE Receipt_Id=receipt.Id) AS Price FROM receipt WHERE [ID]=@id", c.connect());
            command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id.Text.ToString();
            adapter.SelectCommand = command;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        numOrder.Content = reader["Receipt_Id"].ToString();
                        dateOrder.Content = reader["Date_add"].ToString();
                        itogOrder.Content = reader["Price"].ToString();
                    }
                }
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Checks chks = new Checks();
            chks.Show();
            Close();
        }

        private void doIt_Click(object sender, RoutedEventArgs e)
        {
            //тут не знаю, что будем делать
        }
    }
}