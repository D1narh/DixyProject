using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DixyProject.ModelBD
{
    public class Product
    {
        public int Id { get; set; }           // Идентификатор
        public string Title { get; set; }     // Наименование
        public string Price { get; set; }     // Цена
        public string Description { get; set; }     //Описание
        public string ImagePath { get; set; } // Путь к изображению
        public string Discount { get; set; }  // Скидка
        public bool DiscountStatus { get; set; }   // Есть ли скидка
        public string PriceDis { get; set; }  // Цена со скидкой
    }
}
