using System;
using System.IO;

namespace ServerSideCharacter2
{
	public class ErrorLogger : IDisposable
	{
		private readonly StreamWriter _logWriter;

		public string FileName { get; }

		public ErrorLogger(string filename, bool clear)
		{
			FileName = filename;
			_logWriter = new StreamWriter(filename, !clear);
		}

		public void WriteToFile(string msg)
		{
			string dateTime = DateTime.Now.ToString();
			string text = "[" + dateTime + "] " + msg + "\n";
			_logWriter.WriteLine(text);
			_logWriter.Flush();
		}

		public void Dispose()
		{
			_logWriter.Dispose();
		}
	}
}
