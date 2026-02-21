using System;
using System.Threading.Tasks;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone
{
	internal class Program
	{
		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				Log.Error("Unhandled exception: {0}", e.ExceptionObject);
			};

			TaskScheduler.UnobservedTaskException += (sender, e) =>
			{
				Log.Error("Unobserved task exception: {0}", e.Exception);
				e.SetObserved();
			};

			try
			{
				ZoneServer.Instance.Run(args);
			}
			catch (Exception ex)
			{
				Log.Error("While starting server: {0}", ex);
				ConsoleUtil.Exit(1);
			}
		}
	}
}
