using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for Carnivory, whose plant eats at the target every second.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: Snapshotted damage per tick
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Carnivory_Debuff)]
	public class Druid_Carnivory_DebuffOverride : DamageOverTimeBuffHandler
	{
		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Poison;
		}
	}

	/// <summary>
	/// Handler for Chortasmata's Rash, which damages the target every
	/// second.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: Snapshotted damage per tick
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Chortasmata_Debuff)]
	public class Druid_Chortasmata_DebuffOverride : DamageOverTimeBuffHandler
	{
		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Poison;
		}
	}

	/// <summary>
	/// Handler for Thorn, whose vines root the target for 3 seconds and
	/// tear at it every second.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: Snapshotted damage per tick
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.ThornVine_Debuff)]
	public class Druid_ThornVine_DebuffOverride : DamageOverTimeBuffHandler
	{
		private static readonly TimeSpan RootDuration = TimeSpan.FromSeconds(3);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			base.OnActivate(buff, activationType);

			if (activationType == ActivationType.Start)
				buff.Target.AddState(StateType.Held, RootDuration);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Held);
		}
	}

	/// <summary>
	/// Handler for Chortasmata's Floral Scent, which heals the target by the
	/// skill's healing factor every second, 10% more under the Statue of
	/// Goddess Zemyna.
	/// </summary>
	/// <remarks>
	/// [Arts] Chortasmata: Healing Garden adds half the skill factor, at
	/// most 350, and may cure a debuff each second.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Chortasmata_Buff)]
	public class Druid_Chortasmata_BuffOverride : BuffHandler
	{
		private const float ZeminaBonus = 1.1f;
		private const float MaxGardenFactor = 350f;
		private const int GardenCureChance = 10;

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead || buff.Caster is not ICombatEntity caster || !caster.TryGetSkill(SkillId.Druid_Chortasmata, out var skill))
				return;

			var healFactor = GetCaptionRatio(buff, 1);
			var healingGarden = caster.IsAbilityActive(AbilityId.Druid23);

			if (healingGarden)
				healFactor += Math.Min(MaxGardenFactor, skill.Properties.GetFloat(PropertyName.SkillFactor) / 2);

			var healAmount = caster.Properties.GetFloat(PropertyName.HEAL_PWR) * healFactor / 100f;

			if (target.IsBuffActive(BuffId.CarveZemina_Buff))
				healAmount *= ZeminaBonus;

			target.Heal(healAmount, 0);

			if (healingGarden && GameRandom.Get().Next(100) < GardenCureChance)
				target.Components.Get<BuffComponent>()?.RemoveRandomDebuff();
		}
	}
}
