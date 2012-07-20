using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using cms.Code;
using System.Linq;
using cms.data.Migrations;
using cms.data.Shared.Models;

namespace cms.data.tests
{

	
	[TestFixture]
	public class LocalizationsTests
	{

		[TestFixtureSetUp]
		public void Setup()
		{
		}

		[Test]
		public void Sertest()
		{

	
			var dd = new Dictionary<string, string>
			         	{
			         		{"001", "kjabsdjkasbdkasd"},
			         		{"002", "aa asdasd "},
			         		{"003", "bbb"}
			         	};

			var a = new List<LocalizedContent>
			        	{
							new LocalizedContent{Id = 1,Content = "kjasbdjkbasjkdjkasdasd",CultureCode = "cs"},
							new LocalizedContent{Id = 1,Content = "kjasbdjkbasjkdjkasdasd",CultureCode = "cs"},
							new LocalizedContent{Id = 1,Content = "kjasbdjkbasjkdjkasdasd",CultureCode = "cs"},
							new LocalizedContent{Id = 1,Content = "kjasbdjkbasjkdjkasdasd",CultureCode = "cs"},
			        	};

			var json = JSONNetResult.ToJson(dd);
			var jo = JObject.FromObject(dd);

			Console.WriteLine(json);

			Console.WriteLine(jo["001"]);


		}
	}
}