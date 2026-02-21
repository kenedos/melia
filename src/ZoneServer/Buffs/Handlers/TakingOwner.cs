using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Give ride to master, None..
	/// </summary>
	[BuffHandler(BuffId.TakingOwner)]
	public class TakingOwner : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = (Character)buff.Caster;
			var target = (Companion)buff.Target;

			target.StopMove();
			target.Position = caster.Position;
			if (target.Components.TryGet<AiComponent>(out var aiComponent))
				aiComponent.Script.Suspended = true;
			target.IsRiding = true;
			Send.ZC_NORMAL.RidePet(caster, target);
		}

		public override void OnEnd(Buff buff)
		{
			var caster = (Character)buff.Caster;
			var target = (Companion)buff.Target;

			if (target.Components.TryGet<AiComponent>(out var aiComponent))
				aiComponent.Script.Suspended = false;
			target.IsRiding = false;
			target.Position = caster.Position;
			Send.ZC_NORMAL.RidePet(caster, target);
		}
	}
}
