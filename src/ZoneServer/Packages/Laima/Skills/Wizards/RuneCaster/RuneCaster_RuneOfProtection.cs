using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers.Wizards.RuneCaster;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for Rune Caster skill Rune of Protection.
	/// SkillId: 21305
	/// ClassName: RuneCaster_Algiz
	///
	/// Behavior:
	/// - MELEE_GROUND/self-like magic buff.
	/// - Applies Rune of Protection buff to the caster.
	/// - Reduces damage received while casting.
	/// - Supports RuneCaster10: Rune of Protection Enhance inside the buff handler.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.RuneCaster_Algiz)]
	public class RuneCaster_AlgizOverride : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private TimeSpan GetDuration(ICombatEntity caster, Skill skill)
		{
			if (caster.TryGetAbility(AbilityId.RuneCaster11, out _))
				return TimeSpan.FromMinutes(5);

			return TimeSpan.FromMinutes(skill.Level);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.ApplyBuff(caster, skill));
		}

		private async Task ApplyBuff(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(800));

			var duration = this.GetDuration(caster, skill);

			if (caster.TryGetAbility(AbilityId.RuneCaster22, out _))
			{
				caster.StartBuff(
					BuffId.RuneOfProtection_Giant_Buff,
					skill.Level,
					0f,
					TimeSpan.FromSeconds(skill.Level * 60),
					caster,
					skill.Id);

				caster.SetAttackState(false);
				return;
			}

			caster.StartBuff(
				BuffId.RuneOfProtection_Buff,
				skill.Level,
				0f,
				duration,
				caster,
				skill.Id);

			if (caster is Character character)
				RuneCasterSkilledCastingHelper.Apply(character, skill);

			caster.SetAttackState(false);
		}

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			if (buff.Target.TryGetAbility(AbilityId.RuneCaster11, out _))
				return KnockResult.Prevent;

			if (this.TryPreventByKnockdownResistance(buff))
				return KnockResult.Prevent;

			return KnockResult.Allow;
		}

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			if (buff.Target.TryGetAbility(AbilityId.RuneCaster11, out _))
				return KnockResult.Prevent;

			if (this.TryPreventByKnockdownResistance(buff))
				return KnockResult.Prevent;

			return KnockResult.Allow;
		}

		private bool TryPreventByKnockdownResistance(Buff buff)
		{
			if (!buff.Target.TryGetAbility(AbilityId.RuneCaster7, out var ability))
				return false;

			var chance = ability.Level;

			return RandomProvider.Get().Next(100) < chance;
		}

		private bool HasMaintainCasting(Buff buff)
		{
			return buff.Target.TryGetAbility(AbilityId.RuneCaster6, out _);
		}
	}
}
