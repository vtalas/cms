
namespace cms.data.Shared.Models
{
	public class ApplicationSettingStorage
	{
		public int Id { get; set; }
		public string Key { get; set; }
		public string Value { get; set; }
		public virtual ApplicationSetting AppliceSetting { get; set; }
	}
}