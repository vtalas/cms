using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using cms.data.EF;

namespace cms.data
{
	public interface IDataProvider : IDisposable
	{
		//EfContext Context { get; }
	}
}