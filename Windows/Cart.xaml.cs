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
    /// Логика взаимодействия для Cart.xaml
    /// </summary>
    public partial class Cart : Window
    {
        string a;
        public Cart(string a)
        {
            InitializeComponent();
            id.Text = a;

            
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Connect c = new Connect();
            //    string query = "INSERT INTO [dbo].[receipt] VALUES (@client,@date)";
            //    using (c.connect())
            //    {
            //        using (SqlCommand conn = new SqlCommand(query, c.connect()))
            //        {
            //            conn.Parameters.Add("@client", SqlDbType.VarChar).Value = id.Text;
            //            conn.Parameters.Add("@date", SqlDbType.VarChar).Value = DateTime.Now;

            //            int ss = conn.ExecuteNonQuery();
            //            if (ss > 0)
            //            {
            //                MessageBox.Show("Спасибо за покупку!\nЧек за покупку будет доступен в соответствующей вкладке!");
            //            }
            //            else
            //            {
            //                MessageBox.Show("Произошла ошибка при покупке и выдаче чека!\nПопробуйте еще раз!", "Внимание!");
            //            }
            //            c.connect().Close();
            //        }
            //    }
            //    //ФИКСАНУТЬ ВСТАВКУ
            //    string query1 = "INSERT INTO [dbo].[receipt_Line] VALUES (SELECT TOP 1 Id FROM receipt ORDER BY ID DESC,@Prod,@amount,@price)";
            //    using (c.connect())
            //    {
            //        using (SqlCommand conn = new SqlCommand(query1, c.connect()))
            //        {
            //            conn.Parameters.Add("@Prod", SqlDbType.VarChar).Value = title.Text;
            //            conn.Parameters.Add("@amount", SqlDbType.VarChar).Value = Desc.Text;
            //            conn.Parameters.Add("@price", SqlDbType.VarChar).Value = prce.Text;

            //            int ss1 = conn.ExecuteNonQuery();
            //            if (ss1 > 0)
            //            {
            //                MessageBox.Show("Вы приобрели продукты на сумму " + prce.Text + " рублей!");
            //            }
            //            else
            //            {
            //                MessageBox.Show("Произошла ошибка!\nПопробуйте еще раз!", "Внимание!");
            //            }
            //            c.connect().Close();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
    }
}