using System.Web.Mvc;

namespace SAS.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Homepage of website
        /// </summary>
        /// <returns>view of homepage</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}