namespace cms.Controllers.Api
{
	public interface IPhoto
	{
		int Width { get; set; }
		int Height { get; set; }
		string PhotoUri { get; set; }
	}
}