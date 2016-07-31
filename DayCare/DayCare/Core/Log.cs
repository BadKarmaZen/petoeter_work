using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core
{
	public class Log : ILog
	{
		public string LogFileName { get; set; }
		public string ClassName { get; set; }
		
		public Log()
		{

		}

		public Log(Type type)
		{
			if (type.FullName.StartsWith("Caliburn"))
			{
				LogFileName = "Caliburn.Log";
				ClassName = string.Empty;
			}
			else
			{
				LogFileName = "DayCare.Log";
				ClassName = type.Name;
			}
		}

		public void Error(Exception exception)
		{
			WriteLine("ERROR : {0}" + exception);
			WriteLine("ERROR : {0}" + exception.Message);
			if (exception.InnerException != null)
			{
				WriteLine("ERROR.INNER : {0}" + exception.InnerException);
				WriteLine("ERROR.INNER : {0}" + exception.InnerException.Message);				
			}
		}

		public void Info(string format, params object[] args)
		{
			WriteLine(string.Format("INFO {0}: {1}", ClassName, format), args);
		}

		public void Warn(string format, params object[] args)
		{
			WriteLine(string.Format("WARNING {0}: {1}", ClassName, format), args);
		}

		protected void WriteLine(string format, params object[] args)
		{
			using (StreamWriter w = File.AppendText(LogFileName))
			{
				var time = DateTime.Now;
				w.Write(string.Format("{0}.{1:D3}: ", time, time.Millisecond));
				w.WriteLine(format, args);
			}
		}
	}
}
