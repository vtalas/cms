using System;
using System.Linq;
using Google.GData.Client;
using Microsoft.Ajax.Utilities;
using NUnit.Framework;
using cms.Code.LinkAccounts;
using cms.Code.LinkAccounts.Picasa;

namespace cms.web.tests
{
	[TestFixture]
	public class GdataTests
	{
		private const string ClientId = "568637062174-4s9b1pohf5p0hkk8o4frvesnhj8na7ug.apps.googleusercontent.com";
		private const string ClientSecret = "YxM0MTKCTSBV5vGlU05Yj63h";
		private const string AccessToken = "ya29.AHES6ZRQrQVpXpt-dxJ9nneI07yjAF0ou_qqtJtkgblsKFR0";

		[Test]
		public void sdkhskd()
		{
			var x = "https://lh5.googleusercontent.com/-i9UhPVg1blo/UXOVpMmRp4I/AAAAAAAAAZo/UsLzCQeW9O0/7.jpg";
			var a = new Uri(x);
			var aa = new GdataPhotosSettings();
			aa.LargeHeight = 100;
			aa.LargeWidth = 100;
			
			aa.SmallHeight = 100;
			aa.SmallWidth = 100;
			
			aa.MediumWidth = 100;
			aa.MediumHeight = 100;

			//var json =  JsonConvert.SerializeObject(aa);
			//Console.WriteLine(json);

		}

		[Test]
		public void sdkhskdxx()
		{
			var x = "https://lh5.googleusercontent.com/-i9UhPVg1blo/UXOVpMmRp4I/AAAAAAAAAZo/UsLzCQeW9O0/7.jpg";
			var a = new Uri(x);

			var b = new UriBuilder(a);
			var parts = b.Path.Split('/');

			parts[parts.Length - 2] = "kjasbkdjabsdkj";
			b.Path = String.Join("/", parts);
			
			Console.WriteLine(b);


			//var json =  JsonConvert.SerializeObject(aa);
			//Console.WriteLine(json);

		}

		//[Test]
		//public void shouldThrowException_NoAccessToken()
		//{
		//	var a = new GoogleDataOAuth2Service(new GdataStorageMock());
		//	var query = new AlbumQuery(PicasaQuery.CreatePicasaUri("default"));
		//	var feed = a.Picasa.Query(query);
		//}

		//[Test]
		//public void shouldThrowException_InvalidAccessToken()
		//{
		//	var p = new GdataStorageMock
		//		{
		//			ClientId = ClientId,
		//			ClientSecret = ClientSecret,
		//			AccessToken = AccessToken
		//		};
		//	var a = new GoogleDataOAuth2Service(p);

		//	var query = new AlbumQuery(PicasaQuery.CreatePicasaUri("default"));

		//	var feed = a.Picasa.Query(query);

		//	foreach (PicasaEntry entry in feed.Entries)
		//	{
		//		Console.WriteLine(entry.Title.Text);
		//		var ac = new AlbumAccessor(entry);
		//		Console.WriteLine(ac.NumPhotos);
		//	}
		//}

		//[Test]
		//public void oauthUtil()
		//{
		//	var p = new GdataStorageMock {AccessToken = AccessToken};

		//	var a = new GoogleDataOAuth2Service(p);
		//	Console.WriteLine(a.OAuth2AuthorizationUrl());
		//}

		//[Test]
		//public void GetAccessToken()
		//{
		//	var p = new GdataStorageMock
		//		{
		//			ClientId = ClientId,
		//			ClientSecret = ClientSecret,
		//			AccessCode = "4/4hG97iKEh2f2A_h-aSe8qJAXBzzK.0oMHv0ixm5EdOl05ti8ZT3aRqGZBewI"
		//		};

		//	var a = new GoogleDataOAuth2Service(p);

		//	Console.WriteLine(a.GetAccessToken());
		//}
	}
}