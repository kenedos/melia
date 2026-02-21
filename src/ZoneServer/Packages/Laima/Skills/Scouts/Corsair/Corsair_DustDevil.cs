using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Corsair skill Dust Devil.
	/// Spinning AoE attack around the caster that launches enemies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Corsair_DustDevil)]
	public class Corsair_DustDevilOverride : IMeleeGroundSkillHandler
	{
		private const float SplashRadius = 60f;
		private const int ConfusionChance = 12;
		private const int ConfusionDurationMs = 2000;
		private const float JollyRogerDamageBonus = 0.2f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			this.PlayVisualEffects(caster);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(skill, caster));
		}

		private void PlayVisualEffects(ICombatEntity caster)
		{
			var position = GetRelativePosition(PosType.Self, caster, height: 10);
			Send.ZC_GROUND_EFFECT(caster, position, "F_warrior_Cleave_swordtrail1", 1f, 0f, 0f, 90f);

			position = GetRelativePosition(PosType.Self, caster, height: 15);
			Send.ZC_GROUND_EFFECT(caster, position, "F_warrior_Cleave_swordtrail2", 1.2f, 0f, 0f, 90f);

			position = GetRelativePosition(PosType.Self, caster, height: 20);
			Send.ZC_GROUND_EFFECT(caster, position, "F_warrior_Cleave_swordtrail1", 1.4f, 0f, 0f, 90f);

			position = GetRelativePosition(PosType.Self, caster, height: 25);
			Send.ZC_GROUND_EFFECT(caster, position, "F_warrior_Cleave_swordtrail2", 1.6f, 0f, 0f, 90f);

			position = GetRelativePosition(PosType.Self, caster, height: 30);
			Send.ZC_GROUND_EFFECT(caster, position, "F_warrior_Cleave_swordtrail1", 1.8f, 0f, 0f, 90f);

			position = GetRelativePosition(PosType.Self, caster, height: 35);
			Send.ZC_GROUND_EFFECT(caster, position, "F_warrior_Cleave_swordtrail2", 2f, 0f, 0f, 90f);

			position = GetRelativePosition(PosType.Self, caster, height: 15);
			Send.ZC_GROUND_EFFECT(caster, position, "F_scout_redemption_ground", 0.5f, 0f, 0f, 90f);
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster)
		{
			var hits = new List<SkillHitInfo>();
			var confusedHits = new List<SkillHitInfo>();

			var splashParam = skill.GetSplashParameters(caster, caster.Position, caster.Position, length: 0, width: SplashRadius);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);

			await SkillAttack(caster, skill, splashArea, hitDelay: 250, damageDelay: 0, hits, this.ModifyDamage);
			foreach (var hit in hits)
			{
				SkillResultTargetBuff(caster, skill, BuffId.Confuse, skill.Level, 0, ConfusionDurationMs, 1, ConfusionChance, -1, hit);
				if (hit.Target.IsBuffActive(BuffId.Confuse))
					confusedHits.Add(hit);
			}

			hits.Clear();

			await SkillAttack(caster, skill, splashArea, hitDelay: 50, damageDelay: 0, hits, this.ModifyDamage);
			foreach (var hit in hits)
			{
				SkillResultTargetBuff(caster, skill, BuffId.Confuse, skill.Level, 0, ConfusionDurationMs, 1, ConfusionChance, -1, hit);
				if (hit.Target.IsBuffActive(BuffId.Confuse) && !confusedHits.Exists(h => h.Target == hit.Target))
					confusedHits.Add(hit);
			}

			hits.Clear();

			await SkillAttack(caster, skill, splashArea, hitDelay: 50, damageDelay: 0, hits, this.ModifyDamage);
			foreach (var hit in hits)
			{
				SkillResultTargetBuff(caster, skill, BuffId.Confuse, skill.Level, 0, ConfusionDurationMs, 1, ConfusionChance, -1, hit);
				if (hit.Target.IsBuffActive(BuffId.Confuse) && !confusedHits.Exists(h => h.Target == hit.Target))
					confusedHits.Add(hit);
			}

			hits.Clear();

			await SkillAttack(caster, skill, splashArea, hitDelay: 50, damageDelay: 0, hits, this.ModifyDamage);
			foreach (var hit in hits)
			{
				SkillResultTargetBuff(caster, skill, BuffId.Confuse, skill.Level, 0, ConfusionDurationMs, 1, ConfusionChance, -1, hit);
				if (hit.Target.IsBuffActive(BuffId.Confuse) && !confusedHits.Exists(h => h.Target == hit.Target))
					confusedHits.Add(hit);
			}

			hits.Clear();

			await SkillAttack(caster, skill, splashArea, hitDelay: 50, damageDelay: 0, hits, this.ModifyDamage);
			foreach (var hit in hits)
			{
				SkillResultTargetBuff(caster, skill, BuffId.Confuse, skill.Level, 0, ConfusionDurationMs, 1, ConfusionChance, -1, hit);
				if (hit.Target.IsBuffActive(BuffId.Confuse) && !confusedHits.Exists(h => h.Target == hit.Target))
					confusedHits.Add(hit);
			}

			if (confusedHits.Count > 0)
				await SkillResultSpinObject(caster, skill, 0f, 5, 0.2f, 1f, 2000f, hits: confusedHits);
		}

		private SkillHitResult ModifyDamage(Skill skill, ICombatEntity caster, ICombatEntity target, SkillHitResult skillHitResult)
		{
			if (target.IsBuffActive(BuffId.JollyRoger_Enemy_Debuff))
				skillHitResult.Damage *= 1 + JollyRogerDamageBonus;

			return skillHitResult;
		}
	}
}
