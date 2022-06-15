using System;
using System.Collections.Generic;
using System.Data;
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
using DixyProject.ModelBD;

namespace DixyProject.Windows
{
    /// <summary>
    /// Логика взаимодействия для InfoProduct.xaml
    /// </summary>
    public partial class InfoProduct : Window
    {
        string a;
        //public SqlConnection connection = new SqlConnection(@"Data Source=192.168.221.12;Initial Catalog=Dixy;User ID=user01;Password=01;Max Pool Size=30000;Pooling=true");

        int auth = 0;
        string Logins;

        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        SqlDataReader reader;

        public InfoProduct(string a, string login, int au)
        {
            auth = au;
            Logins = login;
            InitializeComponent();
            id.Content = a;

            Connect c = new Connect();
            DataTable buffer = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand("SELECT Name, (SELECT Name FROM category WHERE Id=product.Categori_Id) AS Category, Description, Price, Image FROM Product WHERE [ID]=" + a + "", c.connect());
            adapter.SelectCommand = command;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        titleLabel.Content = reader["Name"].ToString();
                        labCategory.Content = reader["Category"].ToString();
                        labDesc.Content = reader["Description"].ToString();
                        labPrice.Content = reader["Price"].ToString();
                        string aa = reader["Image"].ToString();
                        if (aa != "")
                        {
                            picProd.Source = new BitmapImage(new Uri(DixyProject.App.AppDir + reader["Image"].ToString() + ""));
                        }
                        else
                        {
                            picProd.Source = null;
                        }
                    }
                }
            }



            SqlCommand myCommand = new SqlCommand($"select Id from [dbo].[users] where Login='{Logins}'", c.connect());
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            StreamWriter name = new StreamWriter("fio.txt");
            using (c.connect())
                while (myReader.Read())
                {
                    ID.Content = (myReader["Id"].ToString());
                }
            name.Close();
            c.connect().Close();

            if (auth == 2)
            {
                edit.Visibility = Visibility.Visible;
            }
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            AddEditPtoduct ae = new AddEditPtoduct(id.Content.ToString(), Logins);
            ae.Show();
            Close();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Catalog cat = new Catalog(Logins, auth);
            cat.Show();
            Close();
        }

        //Вылетает ошибка в cmd.CommandText
        private void favorites_Click(object sender, RoutedEventArgs e)
        {
            if (auth == 0)
            {
                MessageBox.Show("Перед тем ка добавить товар в избранное, вам нужно авторизироваться", "Внимание");
            }
            else
            {
                Connect c = new Connect();
                DataTable buffer = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommand command = new SqlCommand("Select * from favorites where Client_Id = " + ID.Content + " and Product_Id = " + id.Content + "", c.connect());
                adapter.SelectCommand = command;
                int i = 0;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        i++;
                    }
                }
                if (i >= 1)
                {
                    MessageBox.Show("Вы уже добавляли данный товар в избранное", "Ошибка");
                }
                else
                {
                    string query = "INSERT INTO [dbo].[favorites] VALUES(@Client_Id,@Date_add,@Product_Id)";
                    using (c.connect())
                    {

                        using (SqlCommand conn = new SqlCommand(query, c.connect()))
                        {
                            conn.Parameters.Add("@Client_Id", SqlDbType.VarChar).Value = ID.Content.ToString();
                            conn.Parameters.Add("@Date_add", SqlDbType.VarChar).Value = System.DateTime.Now;
                            conn.Parameters.Add("@Product_Id", SqlDbType.VarChar).Value = id.Content.ToString();


                            int ss = conn.ExecuteNonQuery();
                            if (ss > 0)
                            {
                                MessageBox.Show("Товар был успешно добавлен в избранное");
                            }
                            else
                            {
                                MessageBox.Show("Произошла ошибка");
                            }
                            c.connect().Close();
                        }
                    }
                }
            }
        }

        private void basket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (auth == 0)
                {
                    MessageBox.Show("Перед тем ка добавить товар в избранное, вам нужно авторизироваться", "Внимание");
                }
                else
                {
                    Connect c = new Connect();
                    DataTable buffer = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand command = new SqlCommand("Select * from basket where Client_Id = " + ID.Content + " and Product_Id = " + id.Content + "", c.connect());
                    adapter.SelectCommand = command;
                    int i = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            i++;
                        }
                    }
                    if (i >= 1)
                    {
                        string query = "UPDATE [dbo].[basket] SET Amount = Amount + 1 where Client_Id = "+ID.Content+ " and Product_Id = "+id.Content+"";
                        using (c.connect())
                        {

                            using (SqlCommand conn = new SqlCommand(query, c.connect()))
                            {
                                int ss = conn.ExecuteNonQuery();
                                if (ss > 0)
                                {
                                    MessageBox.Show("В вашу корзину добавлен еще один товар, который уже был в корзине");
                                }
                                else
                                {
                                    MessageBox.Show("Произошла ошибка");
                                }
                                c.connect().Close();
                            }
                        }
                    }
                    else
                    {
                        string query = "INSERT INTO [dbo].[basket] VALUES(@Client_Id,@Product_Id,@Amount)";
                        using (c.connect())
                        {

                            using (SqlCommand conn = new SqlCommand(query, c.connect()))
                            {
                                conn.Parameters.Add("@Client_Id", SqlDbType.VarChar).Value = ID.Content.ToString();
                                conn.Parameters.Add("@Product_Id", SqlDbType.VarChar).Value = id.Content.ToString();
                                conn.Parameters.Add("@Amount", SqlDbType.VarChar).Value = 1;


                                int ss = conn.ExecuteNonQuery();
                                if (ss > 0)
                                {
                                    MessageBox.Show("Товар был успешно добавлен в корзину");
                                }
                                else
                                {
                                    MessageBox.Show("Произошла ошибка");
                                }
                                c.connect().Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка = " + ex, "Ошибка");
            }
        }
    }
}