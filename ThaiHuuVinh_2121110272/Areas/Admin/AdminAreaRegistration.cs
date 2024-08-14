using System.Web.Mvc;

namespace ThaiHuuVinh_2121110272.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }, 
                new[] { "ThaiHuuVinh_2121110272.Areas.Admin.Controllers" }

            );
        }
    }
}