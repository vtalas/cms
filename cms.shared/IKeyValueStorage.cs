namespace cms.shared
{
	public interface IKeyValueStorage
	{
		string SettingsStorage(string key);
		string SettingsStorage(string key, string value);
	}
}