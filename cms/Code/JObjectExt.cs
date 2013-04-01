using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using cms.data.Dtos;

namespace cms.Code
{
	public static class JObjectExt
	{
		public static ResourceDtoLoc ToResource(this JToken resourcesJson)
		{
			var jToken = resourcesJson.First;
			var id = jToken.Value<int>("Id");

			var d = new ResourceDtoLoc
			       	{
						Value = jToken["Value"].Value<string>(),
						Id =  id
			       	};
			return d;
		}
		public static GridElementDto ToGridElementDto(this JToken s)
		{
			var d = new GridElementDto
			        	{
			        		Id = new Guid(s["Id"].Value<string>()), 
							Content = s["Content"].Value<string>(), 
							Position = s["Position"].Value<int>(), 
							Skin = s["Skin"].Value<string>(), 
							Type = s["Type"].Value<string>(), 
							Width = s["Width"].Value<int>(),
							Resources = s["Resources"].ToDictionary(k => ((JProperty)k).Name, v => v.ToResource())
						};
			return d;
		}
	}
}