using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Wizards.Sorcerer
{
	[Package("laima")]
	[BuffHandler(BuffId.Summoning_Buff)]
	public class Summoning_BuffOverride : BuffHandler
	{
		private const int UpdateInterval = 1000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(UpdateInterval);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			var hasMainSummon = character.Summons.GetSummons(s =>
				!s.IsDead &&
				s.Vars.TryGetInt("SORCERER_SUMMONING", out var value) &&
				value == 1
			).Any();

			if (!hasMainSummon)
				character.StopBuff(BuffId.Summoning_Buff);
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
