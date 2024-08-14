using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThaiHuuVinh_2121110272.Context;

namespace ThaiHuuVinh_2121110272.Models
{
    public class HomeModel
    {
        public List<Product> ListProduct { get; set; }
        public List<Category> ListCategory { get; set; }
    }
}