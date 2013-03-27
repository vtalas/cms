using System.Collections.Generic;
using System.Web.Http;
using System.Web.Routing;

namespace cms.Controllers.Api
{
	public class GdataPhotosController : WebApiControllerBase
	{
		// GET api/gdataphotos
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/gdataphotos/5
		public string Get(int id)
		{
			return "value";
		}

		// POST api/gdataphotos
		public void Post([FromBody]string value)
		{
		}

		// PUT api/gdataphotos/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/gdataphotos/5
		public void Delete(int id)
		{
		}
	}
}
