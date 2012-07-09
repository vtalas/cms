using System;
using System.IO;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using cms.Code.Bootstraper;
using cms.Code.Bootstraper.DataRepository;
using cms.Code.Bootstraper.Less;
using dotless.Core.Input;

namespace cms.web.tests
{
	[TestFixture]
	public class BootstrapGeneratorTest
	{
		private bool deleteUserData = true;
		string Userid = "tetsuser";
		const string Basepath = "../../App_Data";
		JObject _defaultJsonData;
		BootstrapGenerator bg { get; set; }
		protected Lessaak Lesak { get; set; }

		[SetUp]
		public void BootstrapGeneratorTest_setup()
		{

			Userid = "xxx" + DateTime.Now.Millisecond;
			var defs = FileExts.GetContent(Path.Combine(Basepath, "default_bootstrap.json"));
			_defaultJsonData = JObject.Parse(defs);

			bg = new BootstrapGenerator(Basepath, Userid,new BootstrapDataRepositoryImpl(Basepath));
			Lesak = new Lessaak(new ConsoleLogger(), new RelativePathResolver());
		}


		bool IsMinified(string css)
		{
			return !css.Contains("\n");
		}

		void Deleteuserdata()
		{
			if (deleteUserData )
			{
				var files = Directory.GetFiles(Path.Combine(Basepath, "userdata"));
				foreach (var file in files)
				{
					File.Delete(file);
				}
			}
		}

		[Test]
		public void Check_defaults()
		{
			Assert.IsTrue(Directory.Exists(Basepath));
		}

		[Test]
		public void GetuserBootstrapTest_containsAnything()
		{
			var css = bg.GetUserBootstrapCss(Lesak, true);
			Console.Write(css);
			Assert.IsTrue( IsMinified(css));
			Deleteuserdata();
		}
		[Test]
		public void GetuserBootstrapTest_minification()
		{
			var css = bg.GetUserBootstrapCss(Lesak, false);
			Console.Write(css);
			Assert.IsFalse(IsMinified(css));
			Deleteuserdata();
		}


		[Test]
		public void SetUserVariablesLess_jsonData_Test()
		{
			var token = JToken.FromObject(new {type = "newtype", value = "newvalue"});
			var updated = _defaultJsonData;

			var userDataJson = bg.GetUserVariablesJson();
			Assert.AreEqual(_defaultJsonData, userDataJson);

			updated.Add("newvalue", token);
			bg.SetUserVariablesJson(updated);

			userDataJson = bg.GetUserVariablesJson();
			Assert.IsNotNullOrEmpty(userDataJson.ToString());
			Console.WriteLine(userDataJson.ToString());
			Assert.AreNotSame(_defaultJsonData,userDataJson);
			Deleteuserdata();
		}
	}
}

