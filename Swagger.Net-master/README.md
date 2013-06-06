Swagger.Net
===========

Library to document the ASP.NET Web API using the Swagger specification

Latest version: Pre-release 0.5.5

Introduction
------------

Swagger.Net will expose any apis the inherit from the ApiController in the new [ASP.NET Web API](http://www.asp.net/web-api).

Swagger UI is included in the solution.  However, when using the NuGet package you will need to roll your own somewhere and just point your instance of [Swagger UI](https://github.com/wordnik/swagger-ui) at http://YOUR-URL:PORT/api/swagger to expose all of the APIs that you have built.  

Optionally, you may get the [Swagger UI for .NET NuGet package](https://nuget.org/packages/Swagger.Net.UI).

Swagger.Net uses a combination of the Web API [ApiExplorer](http://msdn.microsoft.com/en-us/library/system.web.http.description.apiexplorer.aspx) class and XML Documentation you write in your /// blocks.

One article that helped me tremendously: [Generating a Web API help page using ApiExplorer] (http://blogs.msdn.com/b/yaohuang1/archive/2012/05/21/asp-net-web-api-generating-a-web-api-help-page-using-apiexplorer.aspx). Thank you!

Swagger.NET conforms to the [Swagger specification](http://swagger.wordnik.com/spec) to support all swagger components including client code gen.

Requirements
------------

+ [ASP.NET MVC 4.0](http://www.asp.net/mvc/mvc4)
+ .NET 4.0

Upgrading from v0.5.1 & v0.5.2
------------------------------

0.5.1 and 0.5.2 were both built on the RC, as such, no upgrade path is supported.

Setup
-----

Install the [Swagger.Net NuGet package](https://nuget.org/packages/Swagger.Net) or the [Swagger UI for .NET NuGet package](https://nuget.org/packages/Swagger.Net.UI). Search for "Swagger" in the package manager or Install-Package Swagger.Net & Install-Package Swagger.Net.UI

Configuration
-------------
1. Enable "XML documentation file" and accept the default value, or specify a custom value (i.e. App_Data\Docs.XML), in the Web API's properties | Build menu (Alt+Enter). If you specify a custom value, you will need to edit the App_Start\SwaggerNet.cs file.

2. Point your browswer at /api/swagger to see the api listing for the Swagger spec or point your instance of [Swagger UI](https://github.com/wordnik/swagger-ui) (not included, see step 1) at http://YOUR-URL:PORT/api/swagger to expose all of the APIs that you have built.  If you have Swagger UI for .NET, point at /swagger.

Known Issues
------------

I'm hoping you will help me find and/or fix these.

+ The version of Swagger UI I have built in the /docs folder DOES NOT WORK in Internet Explorer.  :-(
+ Models according to the swagger spec are not yet supported


Other Thoughts
--------------

I dropped the Swagger UI directly on the root in a folder called docs, then redirected the index action to /docs.  I also edited the index.html file to point it directly at /api/swagger so it can traverse the documentation.  The Swagger UI that I installed I got from the [Swagger UI GitHub Repo](https://github.com/wordnik/swagger-ui/downloads)

I have built this library with the lastest versions of everything .NET.  (VS 2012, .NET 4.5, etc.) Note: Minimum of .NET 4.0 is required.

Improvements
------------

Create a fork of [Swagger.Net](https://github.com/miketrionfo/Swagger.Net/fork).  If you pull down the code, note that the swagger docs are included.

Did you change it? [Submit a pull request](https://github.com/miketrionfo/Swagger.Net/pull/new/master).

License
-------

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at [apache.org/licenses/LICENSE-2.0](http://apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

Change Log
----------

0.5.5 Require only .NET 4.0. Fix for duplicate controllers in action filter

0.5.4 Forced the swagger controller to return JSON and removed optional global asax step.

0.5.3 Updated to support the RTM of WebAPI.

0.5.2 Significantly simplified the configuration by using [WebActivator](https://github.com/davidebbo/WebActivator) in the NuGet package

0.5.1 Created a NuGet package