using System.Data.Entity;
using System.Web.Mvc;
using cms.data.EF;
using System.Linq;

namespace cms.Controllers
{
    public class SetupController : Controller
    {
        public ActionResult Index()
        {
        	var a = new EfContext();
			Database.SetInitializer(new DropAndCreate());
        	a.ApplicationSettings.ToList();
			return View();
        }

    }
}
