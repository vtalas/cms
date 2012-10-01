using System.Collections.Generic;

namespace cms.Code.UserResources
{
	public class ApplicationResources
	{
		Dictionary<string, ResourcesObject> JavascriptsClient { get; set; }
		Dictionary<string, HtmlTemplateResource> HtmlTemplateAdmin { get; set; }
		Dictionary<string, ResourcesObject> JavascriptsAdmin { get; set; }
		Dictionary<string, HtmlTemplateResource> HtmlTemplateClient { get; set; }

		public IApplicationResource Javascript(string type, ViewModeType mode = ViewModeType.Client)
		{
			return new ResourcesObject();
		}

		public IApplicationResource HtmlTemplate(string type, string skin, ViewModeType mode = ViewModeType.Client)
		{
			return new ResourcesObject();
		}
		public IApplicationResource HtmlTemplate(string type, ViewModeType mode = ViewModeType.Client)
		{
			return HtmlTemplate(type, "", mode);
		}

		public List<IApplicationResource> GetAll()
		{
			return new List<IApplicationResource>();
		}
	}
}