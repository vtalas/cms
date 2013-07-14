using cms.data.EF;
using Newtonsoft.Json;

namespace cms.Code.LinkAccounts
{
	public class SettingsLoader<T> where T: class
	{
		private readonly string _key;
		public SessionProvider StorageProvider { get; set; }
		
		public SettingsLoader(SessionProvider storageProvider, string key)
		{
			_key = key;
			StorageProvider = storageProvider;
		}

		public string ToJson(T  data)
		{
			return JsonConvert.SerializeObject(data);
		}

		public void Set(T data )
		{
			using (var db = StorageProvider.CreateSession())
			{
				db.Session.Settings.SettingsStorage(_key, ToJson(data));
			}
		}

		public T Get()
		{
			using (var db = StorageProvider.CreateSession())
			{
				var dataString = db.Session.Settings.SettingsStorage(_key);
				return JsonConvert.DeserializeObject<T>(dataString);
			}
		}

	}
}