using System.Collections.Generic;
using MvcSiteMapProvider.Extensibility;

namespace cms.Code
{
	public class Prdel : DynamicNodeProviderBase
	{
		public override IEnumerable<DynamicNode> GetDynamicNodeCollection()
		{
			var returnValue = new List<DynamicNode>();

			var node = new DynamicNode();
			node.Title = "lkandlkasnd";
//			node.ParentKey = "Genre_" + album.Genre.Name;
			returnValue.Add(node);

			//yield return node;
			return returnValue;
		}
	}

}