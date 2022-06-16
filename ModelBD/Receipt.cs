using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DixyProject.ModelBD
{
        public class Receipt
        {
            public int Id { get; set; }           // Идентификатор
            public string Date { get; set; }     // Дата
            public string Price { get; set; }     // Цена
        }
}
