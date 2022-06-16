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
using Microsoft.Win32;

namespace DixyProject.Windows
{
    /// <summary>
    /// Логика взаимодействия для Edit_personal_area.xaml
    /// </summary>
    public partial class Edit_personal_area : Window
    {
        int auth;

        string image = "";
        string put = "";

        public Edit_personal_area(string log,int a)
        {
            auth = a;
            put = log;
            InitializeComponent();



            Connect b = new Connect();
            SqlCommand myCommand = new SqlCommand($"select * from [dbo].[users] where Login='{log}'", b.connect());
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            StreamWriter name = new StreamWriter("fio.txt");
            using (b.connect())
                while (myReader.Read())
                {
                    Name.Text =(myReader["Name"].ToString());
                    First_name.Text =(myReader["First_name"].ToString());
                    Last_name.Text =(myReader["Last_name"].ToString());
                    Imag.ImageSource = new BitmapImage(new Uri(DixyProject.App.AppDir + myReader["Image_path"].ToString() + ""));
                    image = myReader["Image_path"].ToString();
                }
            name.Close();
            b.connect().Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Connect b = new Connect();

            string fileName = System.IO.Path.GetFileName(txtEditor.Text);

            if (image != "")
            {
                fileName = image;
            }
            else
            {
                fileName = @"Image\" + System.IO.Path.GetFileName(txtEditor.Text);

            }
            string query = "UPDATE [dbo].[users] SET [First_name] = '" + First_name.Text + "',[Name] = '" + Name.Text + "',[Last_name] = '" + Last_name.Text + "',[Image_path] = '" + fileName + "' WHERE [Login] = '" + put + "'";
            using (b.connect())
            {

                using (SqlCommand conn = new SqlCommand(query, b.connect()))
                {
                    int ss = conn.ExecuteNonQuery();
                    if (ss > 0)
                    {
                        Cabinet cabinet = new Cabinet(put, auth);
                        cabinet.Show();
                        this.Close();
                    }
                }
            }
        }
        
        private void del_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
