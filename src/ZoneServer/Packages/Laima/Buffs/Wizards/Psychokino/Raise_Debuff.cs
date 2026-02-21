using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Psychokino
{
	/// <summary>
	/// Handle for the Raise, Restrained in the air..
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Raise_Debuff)]
	public class Raise_DebuffOverride : BuffHandler
	{
		public override void OnStart(Buff buff)
		{
			var target = buff.Target;

			target.AddState(StateType.Raised, buff.Duration);
			target.Vibrate(100, 1.5f, 10, 0.1f);
			target.FlyMath(70, 0.5f, 10);
			target.SetTempVar("RAISE_MOVETYPE", target.MoveType.ToString());
			target.MoveType = MoveType.Fly;
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			target.RemoveState(StateType.Raised);
			target.Vibrate(0.1f, 0, 0, 0);
			target.FlyMath(0, 0.1f, 0.5f);
			if (Enum.TryParse<MoveType>(target.GetTempVarStr("RAISE_MOVETYPE"), out var moveType))
				target.MoveType = moveType;
		}
	}
}
