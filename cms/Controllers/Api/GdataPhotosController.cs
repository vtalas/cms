using System;
using System.Collections.Generic;
using System.Web.Http;
using Google.GData.Photos;
using Microsoft.Ajax.Utilities;
using WebMatrix.WebData;
using cms.data.EF;
using cms.data.EF.DataProviderImplementation;
using cms.data.EF.Initializers;

namespace cms.Controllers.Api
{
	public class GdataPhotosController : WebApiPicasaControllerBase
	{
		// GET api/gdataphotos
		public IEnumerable<string> Get()
		{
			var service = Picasa.GetService();
			var query = new AlbumQuery(PicasaQuery.CreatePicasaUri("default"));

			var feed = service.Query(query);

			var x = new List<string>();
			
			foreach (PicasaEntry entry in feed.Entries)
			{
				x.Add( entry.Title.Text);
				var ac = new AlbumAccessor(entry);
				//				Console.WriteLine(ac.NumPhotos);
			}
			return x;
		}

		// GET api/gdataphotos/5
		public string Get(int id)
		{
			return "kjasdhkasjd";
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
