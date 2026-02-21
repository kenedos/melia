using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melia.Shared.Versioning
{
	/// <summary>
	/// Global game related variables.
	/// </summary>
	public static class Versions
	{
		/// <summary>
		/// Gets or sets the game version the server optimizes for.
		/// This is used primarily for client compatibility settings.
		/// </summary>
		public static int Client { get; set; }

		/// <summary>
		/// Gets or sets the protocol version to use, which defines how
		/// packets are structured overall.
		/// </summary>
		public static int Protocol { get; set; } = 1000;
	}

	/// <summary>
	/// Contains specific, common version numbers used for version checks.
	/// </summary>
	public static class KnownVersions
	{
		public const int Unknown = -1;

		/// <summary>
		/// January 2015
		/// </summary>
		public const int KoreanClosedBeta1 = 2760;
		/// <summary>
		/// August 2015
		/// </summary>
		public const int ClosedBeta1 = 10593;
		/// <summary>
		/// October 27th 2015 ~ December 10th 2017
		/// </summary>
		public const int ClosedBeta2 = 10939;
		/// <summary>
		/// March 18, 2016
		/// </summary>
		public const int OpenBeta = 11025;

		/// <summary>
		/// Start of ~2017
		/// </summary>
		public const int ClientVer148158 = 148158;
		/// <summary>
		/// // End of ~2017
		/// </summary>
		public const int ClientVer185517 = 185517;

		/// <summary>
		/// January 8th, 2019
		/// </summary>
		public const int PreReBuild = 234929;
		/// <summary>
		/// Represents the latest version
		/// Currently: May 5th, 2025
		/// </summary>
		public const int Latest = 395869;
	}
}
