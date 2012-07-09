using System;
using System.Web;
using dotless.Core.Loggers;

namespace cms.Code.Bootstraper.Less
{
	public class ConsoleLogger : ILogger
	{
		public void Log(LogLevel level, string message)
		{
			throw new NotImplementedException();
		}

		public void Info(string message)
		{
			throw new NotImplementedException();
		}

		public void Debug(string message)
		{
			throw new NotImplementedException();
		}

		public void Warn(string message)
		{
			throw new NotImplementedException();
		}

		public void Error(string message)
		{
			Console.WriteLine(message);
		}
	}
	public class ResponseLogger : ILogger
	{
		private HttpResponseBase r { get; set; }
		public ResponseLogger(HttpResponseBase response)
		{
			r = response;
		}

		public void Log(LogLevel level, string message)
		{
			throw new NotImplementedException();
		}

		public void Info(string message)
		{
			throw new NotImplementedException();
		}

		public void Debug(string message)
		{
			throw new NotImplementedException();
		}

		public void Warn(string message)
		{
			throw new NotImplementedException();
		}

		public void Error(string message)
		{
			r.Write(message);
		}
	}
}