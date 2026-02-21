using System;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// Thrown when a script references a map that doesn't exist in the
	/// current version's map database. This allows scripts targeting
	/// newer client versions to be silently skipped without masking
	/// real errors.
	/// </summary>
	public class MapNotLoadedException : Exception
	{
		public MapNotLoadedException(string mapClassName)
			: base($"Map '{mapClassName}' not found in map database for this version.")
		{
		}
	}
}
