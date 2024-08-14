using System.Web;
using System.Web.Mvc;

namespace ThaiHuuVinh_2121110272
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
