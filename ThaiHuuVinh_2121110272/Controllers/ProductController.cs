using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThaiHuuVinh_2121110272.Context;

namespace ThaiHuuVinh_2121110272.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        WebEcommerceEntities objWebChinhEntities = new WebEcommerceEntities();
        public ActionResult Detail(int Id)
        {
            var objProduct = objWebChinhEntities.Products.Where(n=>n.Id == Id).FirstOrDefault();   
            return View(objProduct);
        }

        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var listProduct = new List<Product>();

            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                // Retrieve products based on the search string
                listProduct = objWebChinhEntities.Products
                    .Where(n => n.Name.Contains(SearchString))
                    .ToList();
            }
            else
            {
                // Retrieve all products if search string is empty
                listProduct = objWebChinhEntities.Products.ToList();
            }

            ViewBag.CurrentFilter = SearchString;

            // Number of items per page
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            // Sort by product ID in descending order
            listProduct = listProduct.OrderByDescending(n => n.Id).ToList();

            // Return paginated list
            return View(listProduct.ToPagedList(pageNumber, pageSize));
        }
    }
}