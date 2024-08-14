using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThaiHuuVinh_2121110272.Context;

namespace ThaiHuuVinh_2121110272.Models
{
    public class CartModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
