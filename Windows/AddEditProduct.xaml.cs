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
using Microsoft.Win32;

namespace DixyProject.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditPtoduct.xaml
    /// </summary>
    public partial class AddEditPtoduct : Window
    {
        public SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-PU825S2;Initial Catalog=Dixi;Integrated Security=True;Max Pool Size=30000;Pooling=true");
        string a;
        SqlCommand cmd;

        string image = "";
        string put = "";

        int auth = 2;
        string Logins;

        int hh;

        public AddEditPtoduct(string a,string login)
        {
            Logins = login;
            InitializeComponent();
            
            if(a == null)
            {
                save.Visibility = Visibility.Hidden;
                hh = 1;
            }
            else
            {
                add.Visibility = Visibility.Hidden;

                Connect b = new Connect();

                cmd = new SqlCommand("Select Name,Description,Price=cast(Price as int),Image,Categori_Id from product where Id =" + a+"", b.connect());

                SqlDataReader rea = cmd.ExecuteReader();
                while (rea.Read())
                {
                    id.Text = a;
                    Title.Text = rea[0].ToString();
                    Desc.Text = rea[1].ToString();
                    Price.Text = rea[2].ToString();
                    string aa = rea[3].ToString();
                    image = rea[3].ToString();
                    Category.SelectedIndex = int.Parse(rea[4].ToString()) - 1;
                    if (aa != "")
                    {
                        Imag.ImageSource = new BitmapImage(new Uri(DixyProject.App.AppDir + rea[3].ToString() + ""));
                    }
                    else
                    {
                        Imag.ImageSource = null;
                    }
                }
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            string fileName = System.IO.Path.GetFileName(txtEditor.Text);
            try
            {
                if (Title.Text == "" || Desc.Text == "" || Price.Text == "" || Category.SelectedIndex == null)
                {
                    MessageBox.Show("Вы не ввели некоторые данные", "Внимание");
                }
                else
                {
                    if (put == "1")
                    {
                        image = @"Image\Logo.ico";
                    }
                    int idCat = Category.SelectedIndex + 1;
                    int prce = Convert.ToInt32(Price.Text);
                    Connect c = new Connect();
                    string query = "UPDATE [dbo].[Product] SET [Name] = @title,[Categori_Id] = @categ,[Description]=@desc,[Price] = @price,[Image] = @img WHERE [Id] = @Id";
                    using (c.connect())
                    {
                        using (SqlCommand conn = new SqlCommand(query, c.connect()))
                        {
                            conn.Parameters.Add("@title", SqlDbType.VarChar).Value = Title.Text;
                            conn.Parameters.Add("@categ", SqlDbType.VarChar).Value = idCat;
                            conn.Parameters.Add("@desc", SqlDbType.VarChar).Value = Desc.Text;
                            conn.Parameters.Add("@price", SqlDbType.VarChar).Value = prce;
                            conn.Parameters.Add("@img", SqlDbType.VarChar).Value = image;
                            conn.Parameters.Add("@Id", SqlDbType.VarChar).Value = id.Text;

                            int ss = conn.ExecuteNonQuery();
                            if (ss > 0 && prce > 0)
                            {
                                MessageBox.Show("Данные о продукте были обновлены!");
                            }
                            else
                            {
                                MessageBox.Show("Данные о продукте не были обновлены!", "Внимание!");
                            }
                            c.connect().Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(Title.Text == "" || Desc.Text == "" || Price.Text == "" || Category.SelectedIndex == null)
                {
                    MessageBox.Show("Вы не ввели некоторые данные", "Внимание");
                }
                else
                {
                    if (int.Parse(Price.Text) <= 0)
                    {
                        MessageBox.Show("Цена не может быть отрицательной", "Внимание");
                    }
                    else
                    {
                        if (a == null)
                        {
                            place.Content = "Logo.ico";
                            Add();
                        }
                        else
                        {
                            Add();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Add()
        {
            string fileName = System.IO.Path.GetFileName(txtEditor.Text);

            int idCat = Category.SelectedIndex + 1;
            int prce1 = Convert.ToInt32(Price.Text);
            Connect c = new Connect();
            string query = "INSERT INTO Product(Name,Categori_Id,Description,Price,Image) VALUES (@title,@categ,@desc,@price,@img)";
            using (c.connect())
            {
                using (SqlCommand conn = new SqlCommand(query, c.connect()))
                {
                    conn.Parameters.Add("@title", SqlDbType.VarChar).Value = Title.Text;
                    conn.Parameters.Add("@categ", SqlDbType.VarChar).Value = idCat;
                    conn.Parameters.Add("@desc", SqlDbType.VarChar).Value = Desc.Text;
                    conn.Parameters.Add("@price", SqlDbType.VarChar).Value = Price.Text;
                    conn.Parameters.Add("@img", SqlDbType.VarChar).Value =  fileName;
                    conn.Parameters.Add("@Id", SqlDbType.VarChar).Value = id.Text;

                    cmd = new SqlCommand(@"Select TOP(1) product.Image from product where Image = 'Image\" + fileName + "'", c.connect());
                    string path = txtEditor.Text;
                    if (File.Exists(path))
                    {
                        if (File.Exists(path))
                        {
                            if (File.Exists($@"{DixyProject.App.AppDir}\Image\" + fileName + ""))
                            {
                                //Nothing
                            }
                            else
                            {
                                File.Copy(path, $@"{DixyProject.App.AppDir}\Image\" + fileName + "", true);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Файл был удалён в момент добавления");
                        }
                    }
                    else
                    {
                        //Nothing
                    }

                    int ss = conn.ExecuteNonQuery();
                    if (ss > 0 && prce1 > 0)
                    {
                        MessageBox.Show("Продукт был добавлен в базу данных!");

                        Catalog catalog = new Catalog(Logins,auth);
                        catalog.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Продукт не был добавлен в базу данных!\nВозможно, Вы ввели неверные значения или поля пустые!", "Внимание!");
                    }
                    c.connect().Close();
                }
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if(hh == 1)
            {
                Cabinet cabinet = new Cabinet(Logins,auth);
                cabinet.Show();
                this.Close();
            }
            else
            {
                InfoProduct info = new InfoProduct(id.Text,Logins,auth);
                info.Show();
                this.Close();
            }
        }

        private void view_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                image = "";
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                    txtEditor.Text = openFileDialog.FileName;


                Uri Image = new Uri(txtEditor.Text);

                Imag.ImageSource = new BitmapImage(Image);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: Вы не выбрали изображение при обзоре");
            }
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            string image = @"Image\Logo.ico";
            try
            {
                put = "1";
                Imag.ImageSource = Imag.ImageSource = new BitmapImage(new Uri(DixyProject.App.AppDir + image + ""));
                place.Content = "Расположение изображения продукта";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Category_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "SELECT Name FROM category";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    DataTable table = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(table);
                    Category.DisplayMemberPath = "Name";
                    Category.ItemsSource = table.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}