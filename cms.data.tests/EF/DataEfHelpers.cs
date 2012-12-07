using System;
using System.Collections.Generic;
using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.tests.EF
{
	public static class DataEfHelpers{

		public static Resource getresource(this IEnumerable<Resource> source, string culture, string key )
		{
			return source.Single(x => x.Culture == culture && x.Key == key);
		}
	
		public static string _defaultlink = "testPage_link";
	}
}