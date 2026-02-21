using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Wizards.Wizard
{
	/// <summary>
	/// Handler for the Wizard skill Magic Shield.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Wizard_ReflectShield)]
	public class Wizard_ReflectShieldOverride : ISelfSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles skill, applying a buff to the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="dir"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var target = caster;

			var abilityLevel = 0;
			if (caster.TryGetAbility(AbilityId.Wizard26, out var ability))
				abilityLevel = ability.Level;

			var duration = TimeSpan.FromSeconds(300);
			target.StartBuff(BuffId.ReflectShield_Buff, skill.Level, abilityLevel, duration, caster);

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, target, null);

			// Packet logs show an empty hit being sent here, but it doesn't
			// appear to be necessary.
			//var hit = new SkillHitInfo(caster, target, skill, 0, TimeSpan.FromMilliseconds(50), TimeSpan.Zero);
			//hit.HitInfo.ResultType = HitResultType.None;
			//Send.ZC_SKILL_HIT_INFO(caster, hit);
		}
	}
}
