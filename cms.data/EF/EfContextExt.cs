using System;
using System.Data.Entity;
using System.Linq;
using cms.data.Shared.Models;

namespace cms.data.EF
{
	public static class EfContextExt
	{
		public static bool Exist(this IDbSet<Resource> context, int id  )
		{
			return context.Any(x => x.Id == id);
		}

		public static GridElement Get(this IDbSet<GridElement> context, Guid guid, Guid applicationId)
		{
			var item = context.Single(x => x.Id == guid);
			
			if(item.Grid.Any(x=>x.ApplicationSettings.Id != applicationId))
				throw new InvalidOperationException();
			
			return item;
		}

		public static Resource Get(this IDbSet<Resource> context, int id)
		{
			return context.Single(x => x.Id == id);
		}

	}
}