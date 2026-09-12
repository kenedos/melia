using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Scouts.Squire
{
	/// <summary>
	/// Handler for the Arrest debuff, which binds the target in place.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Arrest)]
	public class Squire_Arrest_BuffOverride : BuffHandler
	{
		private const int SlowDurationMs = 4000;
		private const int SlowDurationPerAbilityLevel = 400;

		public override void OnExtend(Buff buff)
		{
			buff.Target.AddState(StateType.Held, buff.Duration);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Held);

			if (buff.Caster is not ICombatEntity caster)
				return;

			if (!caster.TryGetActiveAbilityLevel(AbilityId.Squire1, out var abilityLevel))
				return;

			var duration = TimeSpan.FromMilliseconds(SlowDurationMs + SlowDurationPerAbilityLevel * abilityLevel);
			buff.Target.StartBuff(BuffId.UC_slowdown, 1, abilityLevel, duration, caster, SkillId.Squire_Arrest);
		}
	}
}
