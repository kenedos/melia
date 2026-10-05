using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Retreat Shot buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.RetreatShot)]
	public class SchwarzerReiter_RetreatShot_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Caster is Character character)
			{
				character.SetAttackState(true);

				// Mantém o ataque correto caso Limacon/Serial Bullet estejam ativos.
				SchwarzerReiterAttackHelper.UpdateMainAttack(character);
			}
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Caster is Character character)
			{
				character.SetAttackState(false);

				// Reaplica o ataque correto ao terminar o buff.
				SchwarzerReiterAttackHelper.UpdateMainAttack(character);
			}
		}
	}
}
