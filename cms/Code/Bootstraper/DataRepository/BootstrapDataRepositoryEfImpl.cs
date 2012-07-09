using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cms.data.EF;
using System.Linq;
using cms.data.EF.Bootstrap;
using cms.data.EF.Initializers;

namespace cms.Code.Bootstraper.DataRepository
{
	public class EfBootstrapDataRepositoryImpl : BootstrapDataRepository
	{
		public EfBootstrapDataRepositoryImpl()
		{
		}

		public override JObject Default()
		{

			using (var a = new SessionProvider().Context)
			{
				var item  = a.Bootstrapgenerators.First(x=>x.StatusData  == (byte)Boostrapperstatus.Default);

				return JObject.FromObject(item);
			}
		}

		public override JObject Get(string userid)
		{
			throw new System.NotImplementedException();
		}

		public override void Save(string userid, string data)
		{
			throw new System.NotImplementedException();
		}

		public override void Save(string userId, JObject data)
		{
			throw new System.NotImplementedException();
		}

	}
}