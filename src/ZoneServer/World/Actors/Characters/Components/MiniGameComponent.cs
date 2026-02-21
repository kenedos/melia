using System;
using System.Threading.Tasks;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Actors.Characters.Components
{
	/// <summary>
	/// Stub: MiniGames system was removed during Laima merge.
	/// Provides no-op methods to satisfy callers.
	/// </summary>
	public class MiniGameComponent : CharacterComponent, IUpdateable
	{
		public MiniGameComponent(Character character) : base(character)
		{
		}

		public Task<bool> Start(string miniGameId, TimeSpan startDelay)
		{
			return Task.FromResult(false);
		}

		public void End()
		{
			// No-op: MiniGames not available
		}

		public void Cancel()
		{
			// No-op: MiniGames not available
		}

		public void Update(TimeSpan elapsed)
		{
			// No-op: MiniGames not available
		}
	}
}
