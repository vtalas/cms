using System.Data.Entity;
using System.Web.Mvc;
using cms.data.EF;
using System.Linq;
using cms.data.EF.Initializers;

namespace cms.Controllers
{
    public class SetupController : Controller
    {
        public ActionResult Index()
        {
        	using (var a = new EfContext())
        	{

				Database.SetInitializer(new DropAndCreateTables());
				a.ApplicationSettings.ToList();
        	}
			return View();
        }

    }
}
