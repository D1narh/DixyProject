using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DixyProject.ModelBD
{
    public class Basket
    {
        public  int Id { get; set; }           // Идентификатор
        public  string Name { get; set; }     // название
        public  string Price { get; set; }     // Цена
        public  string Ammount { get; set; }     // Колличество
        public  string ImagePath { get; set; }     // Изображения
    }
}
