namespace cms.shared
{
	public static class CategoryEnum
	{
		public static string Page = "Page";
		public static string Menu = "Menu";
	}
	public class SharedLayer
	{
		public static string Culture { get;  set; }
	
		public static void Init()
		{
			Culture = "cs";
		}

	}
}
