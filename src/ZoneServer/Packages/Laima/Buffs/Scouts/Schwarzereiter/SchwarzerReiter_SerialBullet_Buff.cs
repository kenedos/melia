using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	[Package("laima")]
	[BuffHandler(BuffId.DoubleBullet_Toggle_Buff)]
	public class SchwarzerReiter_DoubleBullet_Toggle_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Caster is Character character)
				SchwarzerReiterAttackHelper.UpdateMainAttack(character);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Caster is Character character)
				SchwarzerReiterAttackHelper.UpdateMainAttack(character);
		}
	}
}
