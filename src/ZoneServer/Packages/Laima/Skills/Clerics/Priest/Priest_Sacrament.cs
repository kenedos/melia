using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Priest
{
	/// <summary>
	/// Handler for the Priest skill Sacrament.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Priest_Sacrament)]
	public class Priest_SacramentOverride : IMeleeGroundSkillHandler
	{
		private const int HolyWeaponBuffDurationSeconds = 300;
		private const float HolyWeaponBuffRadius = 100f;
		private const int DebuffDurationMilliseconds = 15000;
		private const float DebuffRadius = 100f;
		private const float DebuffHolyDamageBonusPerLevel = 0.01f;
		private const float AbilityBonus = 0.005f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			var targetPos = caster.Position;
			var targetCount = 6;
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, DebuffRadius, targetCount));
			var targets = caster.GetTargets();
			await skill.Wait(TimeSpan.FromMilliseconds(200));

			var damageBonus = skill.Level * DebuffHolyDamageBonusPerLevel;

			var byAbility = 1f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Priest29, out var abilityLevel))
				byAbility += abilityLevel * AbilityBonus;
			damageBonus *= byAbility;

			var hits = new List<SkillHitInfo>();
			foreach (var target in targets)
			{
				target.StartBuff(BuffId.Sacrament_Debuff, skill.Level, damageBonus, TimeSpan.FromMilliseconds(DebuffDurationMilliseconds), caster);

				var modifier = new SkillModifier();
				modifier.HitCount = 4;
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var hit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
				hits.Add(hit);
			}

			this.ApplyHolyWeaponBuff(skill, caster);

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		/// <summary>
		/// Enchants caster and party member weapons with holy.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		private void ApplyHolyWeaponBuff(Skill skill, ICombatEntity caster)
		{
			caster.StartBuff(BuffId.Sacrament_Buff, TimeSpan.FromSeconds(HolyWeaponBuffDurationSeconds));

			if (caster is Character character)
			{
				var party = character.Connection.Party;
				var members = caster.Map.GetPartyMembersInRange(character, HolyWeaponBuffRadius, true);

				if (party != null)
				{
					foreach (var member in members)
					{
						if (member == caster)
							continue;
						member.StartBuff(BuffId.Sacrament_Buff, TimeSpan.FromSeconds(HolyWeaponBuffDurationSeconds), caster);
					}
				}
			}
		}
	}
}
