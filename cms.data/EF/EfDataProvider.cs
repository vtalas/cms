using System.Data.Entity;
using System.Linq;
using cms.data.Models;

namespace cms.data.EF
{
	public class EfDataProvider : IDataProvider
	{
		private EfContext db { get; set; }
		public EfDataProvider()
		{
			db = new EfContext();
			
		}
		public EfDataProvider(IDatabaseInitializer<EfContext> initializer)
		{
			db = new EfContext();
			Database.SetInitializer(initializer);
		}

		public EfContext Contenxt
		{
			get { throw new System.NotImplementedException(); }
		}
	}
}