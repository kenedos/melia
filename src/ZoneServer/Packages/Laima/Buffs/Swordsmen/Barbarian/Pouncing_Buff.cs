using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Pouncing, Use sword while moving forward.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Pouncing_Buff)]
	public class Pouncing_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Caster is not Character character)
				return;

			character.Properties.SetFloat(PropertyName.DashRun, 0);
			character.Properties.Modify(PropertyName.Jumpable, -1);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Caster is not Character character)
				return;

			// Legacy behavior: Pouncing drains stamina continuously while active.
			//character.ModifyStamina(-200);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Caster is not Character character)
				return;

			character.Properties.Modify(PropertyName.Jumpable, 1);
		}
	}
}
