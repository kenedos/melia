using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Limacon buff, swaps main attack skill.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Limacon_Buff)]
	public class SchwarzerReiter_Limacon_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = buff.Caster;

			if (caster is Character character)
			{
				SchwarzerReiterAttackHelper.UpdateMainAttack(character);
			}
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Caster is Character character)
			{
				SchwarzerReiterAttackHelper.UpdateMainAttack(character);
			}
		}
	}
}
