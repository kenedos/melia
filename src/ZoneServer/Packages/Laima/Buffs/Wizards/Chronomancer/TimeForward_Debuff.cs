using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Chronomancer
{
	/// <summary>
	/// Handler for TimeForward_Debuff.
	/// While active, any skill the target uses has its cooldown
	/// increased by a flat amount based on skill level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.TimeForward_Debuff)]
	public class TimeForward_DebuffOverride : BuffHandler
	{
		private const string ExtraCdVar = "Melia.TimeForward.ExtraCooldown";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = Math.Max(1, (int)buff.NumArg1);
			var increase = 2 + (skillLevel * 0.2);

			buff.Vars.SetFloat(ExtraCdVar, (float)increase);

			if (buff.Target.Components.TryGet<CooldownComponent>(out var cooldowns))
				cooldowns.ExtraCooldown += TimeSpan.FromSeconds(increase);
		}

		public override void OnEnd(Buff buff)
		{
			if (!buff.Vars.TryGetFloat(ExtraCdVar, out var increase))
				return;

			if (buff.Target.Components.TryGet<CooldownComponent>(out var cooldowns))
				cooldowns.ExtraCooldown -= TimeSpan.FromSeconds(increase);
		}
	}
}
