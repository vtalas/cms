using System.Reflection;

namespace cms.Code.Bootstraper
{
	public class Class1
	{
		 
	//Hi, I see in the forum that there were other person that had my problem with the restart of the web application after 
	//the deletion of one directory under the site. I had the same problem with a File Manager that I created with the RadControl, 
	//after many search I found a solution code. I send the code that I found 

	public void DisableHttp()
        {
            PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            object o = p.GetValue(null, null);
            var f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
            object monitor = f.GetValue(o);
            var m = monitor.GetType().GetMethod("StopMonitoring", 
            BindingFlags.Instance | BindingFlags.NonPublic);
            m.Invoke(monitor, new object[] { });
        }

//it work only on C#, I try to translate in Vb.net but the object doesn't has the same property.
//This code disable the monitoring for the root of the website except bin and themes. The code is send in 1 post in the Microsoft newsgroup and very difficult to find. I tried and it works fine. 
//To use in my project I create a dll in c# and include in the project. Maybe it can be very usefull also for the person that use the editor that I saw in the forum other person has the problem with the editor control.
	}
}