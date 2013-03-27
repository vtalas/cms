﻿using System;
using Google.GData.Client;
using NUnit.Framework;
using cms.Code.LinkAccounts;

namespace cms.web.tests
{

	
	[TestFixture]
	public class GdataTests
	{
		private const string ClientId = "568637062174-4s9b1pohf5p0hkk8o4frvesnhj8na7ug.apps.googleusercontent.com";
		private const string ClientSecret = "YxM0MTKCTSBV5vGlU05Yj63h";
		private const string AccessToken = "ya29.AHES6ZRQrQVpXpt-dxJ9nneI07yjAF0ou_qqtJtkgblsKFR0";

		//[Test]
		//public void shouldThrowException_NoAccessToken()
		//{
		//	var a = new GoogleDataOAuth2(new GdataStorageMock());
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
		//	var a = new GoogleDataOAuth2(p);
			
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

		//	var a = new GoogleDataOAuth2(p);
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

		//	var a = new GoogleDataOAuth2(p);
			
		//	Console.WriteLine(a.GetAccessToken());
		//}
	}
}