using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThaiHuuVinh_2121110272.Context;

namespace ThaiHuuVinh_2121110272.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        WebEcommerceEntities objWebChinhEntities = new WebEcommerceEntities();
        public ActionResult Index()
        {
            var lstCategory = objWebChinhEntities.Categories.ToList();
            return View(lstCategory);
        }
        public ActionResult ProductCategory(int Id)
        {


            var lstProduct = objWebChinhEntities.Products.Where(n=>n.CategoryId == Id).ToList();

            return View(lstProduct);
        }

    }

}