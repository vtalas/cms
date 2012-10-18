using System.Web.Optimization;

namespace cms.App_Start
{
	public static class Class1
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-1.7.2.js"));
			// Code removed for clarity.
		}
	}
}