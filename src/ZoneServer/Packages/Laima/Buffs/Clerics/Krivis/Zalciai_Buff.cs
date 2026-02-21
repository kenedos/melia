using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using System;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Shared.Data.Database;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Zalciai, heals HP in exchange for SP.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Zalciai_Buff)]
	public class Zalciai_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = buff.Caster;
			var target = buff.Target;
			var skillLevel = buff.NumArg1;
			var maxHeal = buff.NumArg2;
		}

		public override void WhileActive(Buff buff)
		{
			var healTick = buff.NumArg2;
			var skillLevel = buff.NumArg1;

			// How much we want to heal
			var amount = healTick;
			// Cannot heal negative
			amount = Math.Max(amount, 0f);
			// Cannot heal over max hp
			amount = Math.Min(buff.Target.MaxHp - buff.Target.Hp, amount);

			if (buff.Caster is Character caster)
			{
				if (caster == null || caster.IsDead || caster.Map != buff.Target.Map)
				{
					buff.Target.RemoveBuff(BuffId.Zalciai_Buff);
					return;
				}

				// Skill Lv 1: 50% Conversion from SP to HP
				// Skill Lv 10: 75% Conversion from SP to HP
				var conversionRate = (0.5f - (skillLevel * 0.025f));
				conversionRate = Math.Min(0.1f, conversionRate);

				var spConsume = amount * conversionRate;
				spConsume = Math.Min(caster.Sp, spConsume);
				spConsume = Math.Max(1, spConsume);

				if (!caster.TrySpendSp(spConsume))
				{
					buff.Target.RemoveBuff(BuffId.Zalciai_Buff);
					return;
				}

				var byAbility = 1f;
				if (caster.TryGetActiveAbilityLevel(AbilityId.Kriwi17, out var level))
					byAbility += level * 0.005f;
				amount *= byAbility;
			}

			buff.Target.Heal(amount, 0);
		}
	}
}
