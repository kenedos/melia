using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Linker
{
	/// <summary>
	/// Handler for the Linker skill Hangman's Knot.
	/// Gathers all linked targets to a single position and deals damage.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Linker_HangmansKnot)]
	public class Linker_HangmansKnotOverride : IMeleeGroundSkillHandler
	{
		private const int CastDelayMs = 700;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(CastDelayMs));

			// Get all linked enemies
			var allLinks = LinkerSkillHelper.FindAllLinks(caster, BuffId.Link_Enemy);
			var linkedTargets = new List<ICombatEntity>();
			foreach (var linkMembers in allLinks)
			{
				foreach (var member in linkMembers)
				{
					if (!linkedTargets.Contains(member))
						linkedTargets.Add(member);
				}
			}

			if (linkedTargets.Count == 0)
				return;

			// Calculate destination 50 units in front of caster
			var destination = caster.Position.GetRelative(caster.Direction, 50);
			destination.Y = caster.Map.Ground.GetHeightAt(destination) + 5;

			// Gather linked enemies to destination
			var hits = new List<SkillHitInfo>();
			var debuffDuration = TimeSpan.FromMilliseconds(1000 + skill.Level * 200);

			foreach (var target in linkedTargets)
			{
				if (target.Rank == MonsterRank.Boss)
					continue;

				target.CancelMonsterSkill();
				if (target.MoveType != MoveType.Holding)
				{
					target.ForceMoveTo(destination, -1, 0.2f);
				}

				target.StartBuff(BuffId.HangmansKnot_Debuff, 1, 0, debuffDuration, caster);

				// Deal damage
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				if (skillHitResult.Result > HitResultType.Dodge)
				{
					target.TakeDamage(skillHitResult.Damage, caster);
				}

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult);
				skillHit.ForceId = ForceId.GetNew();
				hits.Add(skillHit);
			}

			if (hits.Count > 0)
			{
				Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}
	}
}
