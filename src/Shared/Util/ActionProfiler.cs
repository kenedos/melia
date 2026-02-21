using System;
using System.Diagnostics;
using Yggdrasil.Logging;

namespace Melia.Shared.Util
{
	/// <summary>
	/// A small helper class to profile a block of code using a 'using' statement.
	/// It automatically starts a stopwatch on creation and logs the elapsed time on disposal.
	/// </summary>
	public sealed class ActionProfiler : IDisposable
	{
		private readonly string _actionName;
		private readonly long _warningThresholdMs;
		private readonly long _debugThresholdMs;
		private readonly Stopwatch _stopwatch;

		public ActionProfiler(string actionName, long warningThresholdMs = 200, long debugThresholdMs = 100)
		{
			_actionName = actionName;
			_warningThresholdMs = warningThresholdMs;
			_debugThresholdMs = debugThresholdMs;
			_stopwatch = Stopwatch.StartNew();
		}

		public void Dispose()
		{
			_stopwatch.Stop();
		}
	}

	// A dummy disposable to use when profiling is disabled, avoiding null checks.
	public sealed class NullProfiler : IDisposable
	{
		public static readonly NullProfiler Instance = new NullProfiler();
		private NullProfiler() { }
		public void Dispose() { }
	}
}
