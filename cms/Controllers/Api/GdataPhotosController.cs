using System.Collections.Generic;
using System.Web.Http;
using cms.Code.LinkAccounts;

namespace cms.Controllers.Api
{
	public class GdataPhotosController : WebApiControllerBase
	{
		private PicasaServiceFactory Picasa { get; set; }

		public GdataPhotosController()
		{
			//var gdataAuth = new GoogleDataOAuth2(OAuth2ParametersStorageFactory.Storage(ApplicationId));
			//Picasa = new PicasaServiceFactory(gdataAuth.GetRequestDataFactoryInstance("https://picasaweb.google.com/data"));
			var x = "asdlsakndkl ";
		}

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
