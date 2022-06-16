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
    /// Логика взаимодействия для Checks.xaml
    /// </summary>
    public partial class Checks : Window
    {

        public ObservableCollection<Receipt> Receip = new ObservableCollection<Receipt>();
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        SqlDataReader reader;

        string delete;

        int auth = 0;
        string Logins = "";
        int IDD;

        public Checks(int id,string log,int a)
        {
            InitializeComponent();
            IDD = id;
            Logins = log;
            auth = a;

            Start();
        }

        public void Start()
        {
            Connect b = new Connect();

            cmd = new SqlCommand("Select r.Id,Sum(rl.Price),r.Date_add from receipt r inner join receipt_Line rl on r.Id = rl.Receipt_Id where r.Client_Id = " + IDD + " group by r.Id,r.Date_add", b.connect());
            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            adapter.Fill(ds, "Service");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int id = Convert.ToInt32(dr[0].ToString());
                string date = dr[2].ToString();
                string price = dr[1].ToString();

                Receip.Add(new Receipt
                {
                    Id = id,
                    Price = price,
                    Date = date
                });
            }
            list.ItemsSource = Receip;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Cabinet cabinet = new Cabinet(Logins, auth);
            cabinet.Show();
            this.Close();
        }
    }
}
