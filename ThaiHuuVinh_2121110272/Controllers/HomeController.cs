using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using ThaiHuuVinh_2121110272.Context;
using ThaiHuuVinh_2121110272.Models;

namespace ThaiHuuVinh_2121110272.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebEcommerceEntities objWebChinhEntities = new WebEcommerceEntities();

        // GET: Home
        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel
            {
                ListCategory = objWebChinhEntities.Categories.ToList(),
                ListProduct = objWebChinhEntities.Products.ToList()
            };
            return View(objHomeModel);
        }

        // GET: Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User objUser)
        {
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return the view with validation errors
                return View(objUser);
            }

            try
            {
                // Check if the email already exists
                var existingUser = objWebChinhEntities.Users.FirstOrDefault(s => s.Email == objUser.Email);
                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "Email already exists. Please choose another.";
                    return View(objUser);
                }

                // Hash the password
                objUser.Password = CreateMD5(objUser.Password);

                // Add user to the database
                objWebChinhEntities.Users.Add(objUser);

                // Save changes to the database
                objWebChinhEntities.SaveChanges();

                TempData["SuccessMessage"] = "Registration successful!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log the error (optionally)
                // Log.Error("Registration error: ", ex);

                TempData["ErrorMessage"] = "An error occurred while registering. Please try again.";
                return View(objUser);
            }
        }

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User objUser)
        {
            // Hash the password
            objUser.Password = CreateMD5(objUser.Password);

            // Query the user from the database
            var user = objWebChinhEntities.Users
                .FirstOrDefault(n => n.Email == objUser.Email && n.Password == objUser.Password);

            if (user != null)
            {
                // Get the user's first and last name
                var fullName = $"{user.FirstName} {user.LastName}";

                // Store the full name in the session
                Session["username"] = fullName;

                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid email or password!";
                return View();
            }
        }

        // GET: Logout
        public ActionResult Logout()
        {
            // Clear the session
            Session.Clear();

            // Optionally, you can also abandon the session
            Session.Abandon();

            TempData["SuccessMessage"] = "Logged out successfully.";
            return RedirectToAction("Index", "Home");
        }

        // Method to create an MD5 hash from a string
        public static string CreateMD5(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
