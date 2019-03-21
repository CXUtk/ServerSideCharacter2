using ServerSideCharacter2.Utils;
using System;
using System.IO;

namespace ServerSideCharacter2
{
	public class ErrorLogger : IDisposable
	{
		private StreamWriter _logWriter;

		public string FileName { get; }

		public bool IsAppend { get; set; }

		public ErrorLogger(string filename, bool clear)
		{
			FileName = filename;
			IsAppend = !clear;
		}

		public void WriteToFile(string msg)
		{
			_logWriter = new StreamWriter(FileName, IsAppend);
			string dateTime = DateTime.Now.ToString();
			string text = "[" + dateTime + "] " + msg + "\n";
			_logWriter.WriteLine(text);
			_logWriter.Flush();
			_logWriter.Close();

		}

		public void Dispose()
		{
			_logWriter.Dispose();
		}
	}
}
