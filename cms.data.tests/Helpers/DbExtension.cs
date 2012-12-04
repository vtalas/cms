using cms.data.EF;
using cms.data.Shared.Models;

namespace cms.data.tests.Helpers
{
	public static class DbExtension
	{
		public static Grid AddToDb(this Grid item)
		{
			using (var db = new EfContext())
			{
				var x = db.Grids.Add(item);
				db.SaveChanges();
				return x;
			}
		}

	}
}