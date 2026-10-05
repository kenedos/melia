using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Archers.PiedPiper
{
	[Package("laima")]
	[SkillHandler(SkillId.PiedPiper_HamelnNagetier)]
	public class PiedPiper_HamelnNagetier : IGroundSkillHandler, IDynamicCasted
	{
		private const float TargetSearchRange = 30f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			var mice = PiedPiperHamelnNagetierHelper.GetMice(character);
			if (mice.Count == 0)
			{
				character.ServerMessage("No Hameln Nagetier mice are currently summoned.");
				return;
			}

			var targets = character.Map.GetAttackableEnemiesInPosition(character, farPos, TargetSearchRange)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => farPos.Get2DDistance(target.Position))
				.Take(mice.Count)
				.ToList();

			if (targets.Count == 0)
			{
				character.ServerMessage("No valid target was found at the selected position.");
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage("Not enough SP.");
				return;
			}

			skill.IncreaseOverheat();
			character.TurnTowards(farPos);
			character.SetAttackState(true);

			Send.ZC_SKILL_READY(character, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(character, 0, originPos, originPos.GetDirection(farPos), farPos);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, farPos);

			var enhanceMultiplier = PiedPiperHamelnNagetierHelper.GetEnhanceMultiplier(character);
			var hits = new List<SkillHitInfo>();

			for (var i = 0; i < targets.Count; i++)
			{
				var mouse = mice[i];
				var target = targets[i];

				mouse.MoveTo(target.Position);

				var modifier = SkillModifier.Default;
				modifier.DamageMultiplier *= enhanceMultiplier;

				var skillHitResult = SCR_SkillHit(character, target, skill, modifier);
				if (skillHitResult.Result == HitResultType.Dodge)
					continue;

				var skillHit = new SkillHitInfo(character, target, skill, skillHitResult, skill.Data.DefaultHitDelay, skill.Data.DefaultHitDelay);
				skillHit.HitEffect = HitEffect.Impact;
				skillHit.ApplyDamage();
				hits.Add(skillHit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(character, hits);

			PiedPiperHamelnNagetierHelper.RemoveMice(character);
			character.StopBuff(BuffId.HamelnNagetier_Buff);
			character.SetAttackState(false);
		}
	}
}
