using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Chronomancer
{
	[Package("laima")]
	[BuffHandler(BuffId.Slow_Debuff)]
	public class Slow_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = buff.NumArg1;

			var speedReduction = 3f + skillLevel * 1f;

			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -speedReduction);

			buff.Target.AddState(StateType.Held, buff.Duration);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);

			buff.Target.RemoveState(StateType.Held);
		}
	}
}
