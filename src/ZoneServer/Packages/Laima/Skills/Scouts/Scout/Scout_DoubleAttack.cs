using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Scout
{
	/// <summary>
	/// Handler for the Scout skill Double Attack.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Scout_DoubleAttack)]
	public class Scout_DoubleAttackOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles skill, applying a buff to the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="dir"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var duration = TimeSpan.FromSeconds(300);
			var doubleHitChance = 25f + skill.Level * 5f;

			var byAbility = 1f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Scout21, out var abilityLevel))
				byAbility += abilityLevel * 0.005f;

			doubleHitChance *= byAbility;

			caster.StartBuff(BuffId.DoubleAttack_Buff, skill.Level, doubleHitChance, duration, caster);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos);
		}
	}
}
