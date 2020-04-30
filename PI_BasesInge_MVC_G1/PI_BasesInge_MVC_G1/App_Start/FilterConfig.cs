using System.Web;
using System.Web.Mvc;

namespace PI_BasesInge_MVC_G1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
