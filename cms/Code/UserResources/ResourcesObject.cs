
namespace cms.Code.UserResources
{
	public interface IApplicationResource
	{
		ViewModeType ModeType { get; set; }
		string Path { get; set; }
	}

	
	public class ResourcesObject : IApplicationResource
	{
		public ViewModeType ModeType { get; set; }
		public string Path { get; set; }
	}
}