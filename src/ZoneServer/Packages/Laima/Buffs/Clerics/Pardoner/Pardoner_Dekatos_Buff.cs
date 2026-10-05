using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handle for the Dekatos_Buff, which has a chance to instantly kill
	/// a non-boss monster when the buff expires.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Dekatos_Buff)]
	public class Pardoner_Dekatos_BuffOverride : BuffHandler
	{
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (buff.Caster is not ICombatEntity caster) return;

			// The effect does not apply to player characters.
			if (target is Character) return;

			// The execute chance calculation.
			var executeChance = 10f;

			var casterMsp = caster.Properties.GetFloatSafe(PropertyName.MSP); // MNA
			var targetLevel = target.Properties.GetFloatSafe(PropertyName.Lv);

			var bonusChance = (casterMsp / (targetLevel * 2f)) * 10f;

			// The bonus chance is capped between 0 and 10.
			bonusChance = Math.Clamp(bonusChance, 0, 10);

			executeChance += bonusChance;

			// Roll for the execute.
			if (RandomProvider.Get().Next(1, 101) <= executeChance
				&& target is Mob monster && monster.Rank != MonsterRank.Boss)
			{
				// If the check succeeds on a non-boss monster, kill it.
				monster.Kill(caster);
			}
		}
	}
}
