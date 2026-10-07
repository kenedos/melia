using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for the Exorcist skill Entity, which reveals the hidden
	/// enemies around the Exorcist and strikes them, dealing 70% to those
	/// that weren't hidden or marked by Gregorate.
	/// </summary>
	/// <remarks>
	/// [Arts] Entity: Search instead raises the Exorcist's movement speed
	/// and searches around them for 5 + level seconds.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Exorcist_Entity)]
	public class Exorcist_EntityOverride : ISelfSkillHandler
	{
		private const float Range = 150f;
		private const int MaxTargets = 10;
		private const float VisibleDamageRate = 0.7f;
		private const float SearchRange = 100f;
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(850);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, ForceId.GetNew(), null);

			if (caster.IsAbilityActive(AbilityId.Exorcist27))
			{
				this.StartSearch(skill, caster);
				return;
			}

			skill.Run(this.Reveal(skill, caster));
		}

		/// <summary>
		/// Reveals and strikes the enemies around the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Reveal(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(HitDelay);

			if (caster.IsDead)
				return;

			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, Range)
				.OrderByDescending(a => a.IsBuffActiveByKeyword(BuffTag.Cloaking))
				.Take(MaxTargets);

			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var modifier = new SkillModifier();

				if (target.IsBuffActiveByKeyword(BuffTag.Cloaking))
					target.StopBuffByTag(BuffTag.Cloaking);
				else if (!target.IsBuffActive(BuffId.GregorateATK_Buff))
					modifier.FinalDamageMultiplier *= VisibleDamageRate;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		/// <summary>
		/// Starts [Arts] Entity: Search, which follows the caster and
		/// reveals the enemies around them.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		private void StartSearch(Skill skill, ICombatEntity caster)
		{
			var duration = TimeSpan.FromSeconds(5 + skill.Level);

			caster.StartBuff(BuffId.Entity_Pad_Buff, skill.Level, 0, duration, caster, skill.Id);

			var pad = new Pad(PadName.Exorcist_Entity_Abil, caster, skill, new Circle(caster.Position, SearchRange));
			pad.Position = caster.Position;
			pad.Trigger.LifeTime = duration;
			pad.FollowsTarget(caster);

			caster.Map.AddPad(pad);
		}
	}
}
