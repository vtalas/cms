using Newtonsoft.Json.Linq;

namespace cms.Code.Bootstraper.DataRepository
{
	public abstract class BootstrapDataRepository
	{
		public abstract JObject Default();
		public abstract JObject Get(string userid);
		public abstract void Save(string userid,string data);
		public abstract void Save(string userId, JObject data);
	}
}