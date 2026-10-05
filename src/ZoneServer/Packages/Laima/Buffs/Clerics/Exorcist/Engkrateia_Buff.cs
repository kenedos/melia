using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Exorcist;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Exorcist
{
	[Package("laima")]
	[BuffHandler(BuffId.Engkrateia_Buff)]
	public class Engkrateia_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler, IBuffOnHitInfoCreatedHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const float MinimumDamageReductionRate = 0.05f;
		private const float MaximumDamageReductionRate = 0.15f;
		private const float DemonOrDarkReductionRate = 0.10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return KnockResult.Prevent;
		}

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return KnockResult.Prevent;
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (buff.Target == null || skillHitInfo == null || skillHitInfo.Target != buff.Target || skillHitInfo.HitInfo == null)
				return;

			if (skillHitInfo.HitInfo.Damage <= 0)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var reductionRate = MinimumDamageReductionRate + (skillLevel - MinimumSkillLevel) * (MaximumDamageReductionRate - MinimumDamageReductionRate) / (MaximumSkillLevel - MinimumSkillLevel);

			skillHitInfo.HitInfo.Damage *= 1f - reductionRate;

			if (this.IsDemonAttacker(skillHitInfo.Attacker) || this.IsDarkAttack(skillHitInfo))
				skillHitInfo.HitInfo.Damage *= 1f - DemonOrDarkReductionRate;

			if (buff.Target is Character character && Exorcist_EngkrateiaGoddessReplyAbility.IsActive(character))
			{
				var currentHp = Math.Max(0f, character.Properties.GetFloat(PropertyName.HP));
				var maximumAllowedDamage = Math.Max(0f, currentHp - 1f);

				if (skillHitInfo.HitInfo.Damage > maximumAllowedDamage)
					skillHitInfo.HitInfo.Damage = maximumAllowedDamage;
			}
		}

		private bool IsDemonAttacker(ICombatEntity attacker)
		{
			if (attacker == null)
				return false;

			if (!Enum.TryParse<RaceType>(attacker.Properties.GetString(PropertyName.RaceType), true, out var race))
				return false;

			return race == RaceType.Velnias;
		}

		private bool IsDarkAttack(SkillHitInfo skillHitInfo)
		{
			return skillHitInfo.Skill != null && skillHitInfo.Skill.Data.Attribute == AttributeType.Dark;
		}
	}
}
