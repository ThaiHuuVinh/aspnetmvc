using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThaiHuuVinh_2121110272.Context;
using static ThaiHuuVinh_2121110272.common;
using PagedList;

namespace ThaiHuuVinh_2121110272.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        WebEcommerceEntities objWebChinhEntities = new WebEcommerceEntities();
        // GET: Admin/Product
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
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            // Sort by product ID in descending order
            listProduct = listProduct.OrderByDescending(n => n.Id).ToList();

            // Return paginated list
            return View(listProduct.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {

            this.LoadData();


            return View();
        }
        [ValidateInput(false)]

        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            this.LoadData();
            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý tệp tin hình ảnh nếu có
                    if (Request.Files["ImageUpload"] != null && Request.Files["ImageUpload"].ContentLength > 0)
                    {
                        var file = Request.Files["ImageUpload"];
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;

                        // Đường dẫn đến thư mục lưu trữ hình ảnh
                        string path = Server.MapPath("~/Content/images/items/");

                        // Tạo thư mục nếu nó không tồn tại
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        // Đường dẫn đầy đủ để lưu tệp tin
                        string fullPath = Path.Combine(path, fileName);

                        // Lưu tệp tin vào thư mục
                        file.SaveAs(fullPath);
                        objProduct.Avatar = fileName; // Lưu tên tệp tin vào cơ sở dữ liệu
                    }

                    // Cập nhật thông tin sản phẩm và lưu vào cơ sở dữ liệu
                    objProduct.CreatedOnUtc = DateTime.Now;
                    objWebChinhEntities.Products.Add(objProduct);
                    objWebChinhEntities.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Ghi lỗi nếu có
                    System.Diagnostics.Debug.WriteLine("Error during image upload or database save: " + ex.Message);
                    ModelState.AddModelError("", "An error occurred while saving the product. Please try again.");
                }
            }

            // Nếu có lỗi hoặc không hợp lệ, trở về form tạo sản phẩm
            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objProduct = objWebChinhEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var objProduct = objWebChinhEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            if (objProduct != null)
            {
                objWebChinhEntities.Products.Remove(objProduct);
                objWebChinhEntities.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objProduct = objWebChinhEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Edit(int id, Product objProduct)
        {
            this.LoadData();
            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = objWebChinhEntities.Products.Find(id);

                    if (Request.Files["ImageUpload"] != null && Request.Files["ImageUpload"].ContentLength > 0)
                    {
                        var file = Request.Files["ImageUpload"];
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;

                        // Đường dẫn đến thư mục lưu trữ hình ảnh
                        string path = Server.MapPath("~/Content/images/items/");

                        // Tạo thư mục nếu nó không tồn tại
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        // Đường dẫn đầy đủ để lưu tệp tin
                        string fullPath = Path.Combine(path, fileName);

                        // Lưu tệp tin vào thư mục
                        file.SaveAs(fullPath);
                        existingProduct.Avatar = fileName; // Cập nhật tên tệp tin vào cơ sở dữ liệu
                    }

                    // Cập nhật các thuộc tính khác của sản phẩm
                    existingProduct.Name = objProduct.Name;
                    existingProduct.CategoryId = objProduct.CategoryId;
                    existingProduct.ShortDes = objProduct.ShortDes;
                    existingProduct.FullDescription = objProduct.FullDescription;
                    existingProduct.Price = objProduct.Price;
                    existingProduct.PriceDiscount = objProduct.PriceDiscount;
                    existingProduct.TypeId = objProduct.TypeId;
                    existingProduct.Slug = objProduct.Slug;
                    existingProduct.BrandId = objProduct.BrandId;
                    existingProduct.CreatedOnUtc = objProduct.CreatedOnUtc; // Nếu cần cập nhật ngày tạo

                    // Cập nhật thông tin sản phẩm trong cơ sở dữ liệu
                    objWebChinhEntities.Entry(existingProduct).State = EntityState.Modified;
                    objWebChinhEntities.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Ghi lỗi nếu có
                    System.Diagnostics.Debug.WriteLine("Error during image upload or database update: " + ex.Message);
                    ModelState.AddModelError("", "An error occurred while updating the product. Please try again.");
                }
            }

            // Nếu có lỗi hoặc không hợp lệ, trở về form chỉnh sửa sản phẩm
            return View(objProduct);
        }


        void LoadData()
        {
            common objcommon = new common();
            var lstCategory = objWebChinhEntities.Categories.ToList();
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstCategory);
            ViewBag.ListCatgory = objcommon.ToSelectList(dtCategory, "Id", "Name");

            var lstBrand = objWebChinhEntities.Brands.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            ViewBag.ListBrand = objcommon.ToSelectList(dtBrand, "Id", "Name");

            //loai san pham
            List<ProductType> lstProductType = new List<ProductType>();
            ProductType objProductType = new ProductType();
            objProductType.Id = 01;
            objProductType.Name = "Giảm giá sốc";
            lstProductType.Add(objProductType);

            objProductType = new ProductType();
            objProductType.Id = 02;
            objProductType.Name = "Đề xuất";
            lstProductType.Add(objProductType);

            DataTable dtProductType = converter.ToDataTable(lstProductType);
            ViewBag.ListProductType = objcommon.ToSelectList(dtProductType, "Id", "Name");
        }
    }
}