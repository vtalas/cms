using System.Linq;
using Newtonsoft.Json.Linq;
using cms.data.EF;
using cms.data.Models;

namespace cms.data
{
	public interface IDataProvider
	{
		EfContext Contenxt { get; }
	}
}