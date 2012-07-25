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

			var a = new List<Resource>
			        	{
							new Resource{Id = 1,Value= "EEasbdjkbasjkdjkasdasd",Culture = "cs"},
							new Resource{Id = 2,Value= "AAkjasbdjkbasjkdjkasdasd",Culture = "cs"},
							new Resource{Id = 3,Value= "BBkjasbdjkbasjkdjkasdasd",Culture = "cs"},
							new Resource{Id = 4,Value= "CCkjasbdjkbasjkdjkasdasd",Culture = "cs"},
			        	};

			var json = JSONNetResult.ToJson(dd);
			var jo = JObject.FromObject(dd);

			Console.WriteLine(json);

			Console.WriteLine(jo["001"]);


		}
	}
}