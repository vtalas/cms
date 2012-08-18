namespace cms.shared
{
	public class SharedLayer
	{
		public static string Culture { get;  set; }
	
		public static void Init()
		{
			Culture = "cs";
		}

	}
}
