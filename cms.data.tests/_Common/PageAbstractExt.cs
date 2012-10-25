using System;
using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.Extensions
{
	public static class PageAbstractExt
	{
		public static bool Exist(this IQueryable<Resource> context, int id)
		{
			return context.Any(x => x.Id == id);
		}

		public static Grid Get(this IQueryable<Grid> context, Guid guid)
		{
			var item = context.Single(x => x.Id == guid);
			return item;
		}

		public static Resource Get(this IQueryable<Resource> context, int id)
		{
			return context.Single(x => x.Id == id);
		}
	}
}