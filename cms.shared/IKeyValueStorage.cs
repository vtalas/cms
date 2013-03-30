using System;

namespace cms.shared
{
	public interface IKeyValueStorage: IDisposable
	{
		string SettingsStorage(string key);
		string SettingsStorage(string key, string value);
	}
}