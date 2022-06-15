using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DixyProject.ModelBD
{
    class Connect
    {
        public SqlConnection connect()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-PU825S2;Initial Catalog=Dixi;Integrated Security=True;Max Pool Size=30000;Pooling=true");
            connection.Open();
            return connection;

            //SqlConnection connection = new SqlConnection("Data Source=192.168.221.12;Initial Catalog=Dixy;User ID=user01;Password=01;Max Pool Size=30000;Pooling=true");
            //connection.Open();
            //return connection;
        }
    }
}
