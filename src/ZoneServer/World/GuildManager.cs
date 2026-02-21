using System.Collections.Generic;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World
{
	/// <summary>
	/// Stub: Guild system was removed during Laima merge.
	/// Provides no-op methods to satisfy callers.
	/// </summary>
	public class GuildManager
	{
		public object GetGuild(long guildId) => null;

		public bool TryGetGuild(long guildId, out object guild)
		{
			guild = null;
			return false;
		}

		public bool Exists(long guildId) => false;

		public List<object> GetAllGuilds() => new();
	}
}
