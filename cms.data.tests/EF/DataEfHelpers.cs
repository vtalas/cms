using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using cms.data.Dtos;
using cms.data.Shared.Models;

namespace cms.data.tests.EF
{
	public static class DataEfHelpers{

		public static Resource getresource(this IEnumerable<Resource> source, string culture, string key )
		{
			return source.Single(x => x.Culture == culture && x.Key == key);
		}
	
		public static Guid guid = new Guid("c78ee05e-1115-480b-9ab7-a3ab3c0f6643");
	
		public static GridElement AddDefaultGridElement()
		{
			using (var db = SessionManager.CreateSessionWithSampleData)
			{
				var a = new GridElement
					        {
						        Content = "oldcontent",
						        Width = 12,
						        Position = 0,
						        Skin = "xxx",
						        Resources = new List<Resource>
							                    {
								                    new Resource{ Culture = "cs", Value = "cesky", Key = "text1"},
								                    new Resource{ Culture = "en", Value = "englicky", Key = "text1"},
							                    }
					        };
				var newitem = db.GridElement.AddToGrid(a, guid);
				Assert.True(!newitem.Id.IsEmpty());
				return newitem;
			}

		}

		public static string CurrentCulture{get { return shared.SharedLayer.Culture; }
		}
		public static string _defaultlink = "linkTestPage";

	}
}