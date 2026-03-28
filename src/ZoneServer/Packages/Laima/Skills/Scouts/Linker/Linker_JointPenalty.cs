using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Effects;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Linker
{
	/// <summary>
	/// Handler for the Linker skill Joint Penalty.
	/// Creates links between enemies that share damage.
	/// Linked targets take reduced damage (-30% base) offset by skill level (+3% per level).
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Linker_JointPenalty)]
	public class Linker_JointPenaltyOverride : IForceSkillHandler
	{
		private const int LinkDurationSeconds = 12;
		private const float LinkSplashRange = 120f;
		private const int MaxLinkTargets = 3;
		private const float MaxVerticalDistance = 25f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var skillHitResult = SCR_SkillHit(caster, target, skill);

			if (skillHitResult.Result > HitResultType.Dodge)
			{
				target.TakeDamage(skillHitResult.Damage, caster);
				this.CreateEnemyLink(caster, target, skill);
			}

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult);
			skillHit.ForceId = ForceId.GetNew();
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
		}

		/// <summary>
		/// Creates or extends the enemy link with star topology.
		/// If the target is already linked, new targets are added to the existing link.
		/// </summary>
		private void CreateEnemyLink(ICombatEntity caster, ICombatEntity firstTarget, Skill skill)
		{
			// Check if first target is already in a link group
			var existingMembers = new List<ICombatEntity>();
			var existingLinkId = 0;
			ICombatEntity existingAnchor = null;

			if (firstTarget.TryGetBuff(BuffId.Link_Enemy, out var existingBuff))
			{
				if (existingBuff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
				{
					existingLinkId = existingBuff.Vars.GetInt("Melia.Link.Id");

					foreach (var handle in memberHandles)
					{
						if (firstTarget.Map.TryGetCombatEntity(handle, out var member))
						{
							existingMembers.Add(member);

							if (member.TryGetBuff(BuffId.Link_Enemy, out var memberBuff) &&
								memberBuff.Vars.GetBool("Melia.Link.IsAnchor"))
							{
								existingAnchor = member;
							}
						}
					}
				}
			}

			// Get nearby targets, excluding those already in the existing link
			var existingHandles = new HashSet<int>(existingMembers.Select(e => e.Handle));
			var nearbyTargets = caster.SelectObjectNear(firstTarget, LinkSplashRange, RelationType.Enemy);

			var minY = firstTarget.Position.Y;
			var maxY = firstTarget.Position.Y;

			// Prioritize targets that don't already have Link_Enemy, then by distance
			var sortedTargets = nearbyTargets
				.Where(t => !existingHandles.Contains(t.Handle))
				.OrderBy(t => t.IsBuffActive(BuffId.Link_Enemy) ? 1 : 0)
				.ThenBy(t => t.GetDistance(firstTarget))
				.Take(MaxLinkTargets);

			var newTargets = new List<ICombatEntity>();

			foreach (var target in sortedTargets)
			{
				if (target.Handle == firstTarget.Handle)
					continue;

				var targetY = target.Position.Y;
				var currentMinY = minY;
				var currentMaxY = maxY;

				minY = Math.Min(minY, targetY);
				maxY = Math.Max(maxY, targetY);

				if (maxY - minY <= MaxVerticalDistance)
				{
					newTargets.Add(target);
				}
				else
				{
					minY = currentMinY;
					maxY = currentMaxY;
				}
			}

			// If no new targets found and no existing link, need at least the first target + 1 other
			if (newTargets.Count == 0 && existingMembers.Count == 0)
				return;

			// Combine existing members with new targets
			var allLinkedTargets = new List<ICombatEntity>();
			if (existingMembers.Count > 0)
			{
				allLinkedTargets.AddRange(existingMembers);
			}
			else
			{
				allLinkedTargets.Add(firstTarget);
			}
			allLinkedTargets.AddRange(newTargets);

			// Need at least 2 targets total
			if (allLinkedTargets.Count < 2)
				return;

			// Remove Link_Enemy from new targets only (they might have been in a different link)
			foreach (var target in newTargets)
			{
				target.StopBuff(BuffId.Link_Enemy);
			}

			// Determine anchor - use existing anchor or first target
			var anchor = existingAnchor ?? firstTarget;
			var linkId = existingLinkId != 0 ? existingLinkId : ZoneServer.Instance.World.CreateLinkHandle();
			var handles = allLinkedTargets.Select(e => e.Handle).ToList();
			var duration = TimeSpan.FromSeconds(LinkDurationSeconds);

			// Update existing members with new member list
			foreach (var entity in existingMembers)
			{
				if (entity.TryGetBuff(BuffId.Link_Enemy, out var buff))
				{
					buff.Vars.Set("Melia.Link.Members", handles);
				}
			}

			// Create link buffs on new targets
			foreach (var entity in newTargets)
			{
				var buff = entity.StartBuff(BuffId.Link_Enemy, skill.Level, 0, duration, caster, skill.Id);
				if (buff != null)
				{
					buff.Vars.Set("Melia.Link.Id", linkId);
					buff.Vars.Set("Melia.Link.Caster", caster.Handle);
					buff.Vars.Set("Melia.Link.Members", handles);
					buff.Vars.Set("Melia.Link.Topology", 1); // 1 = Star topology

					entity.SetTempVar("LINK_BUFF", BuffId.Link_Enemy.ToString());
				}
			}

			// If this is a new link (no existing members), set up the anchor
			if (existingMembers.Count == 0)
			{
				var buff = firstTarget.StartBuff(BuffId.Link_Enemy, skill.Level, 0, duration, caster, skill.Id);
				if (buff != null)
				{
					buff.Vars.Set("Melia.Link.Id", linkId);
					buff.Vars.Set("Melia.Link.Caster", caster.Handle);
					buff.Vars.Set("Melia.Link.Members", handles);
					buff.Vars.Set("Melia.Link.Topology", 1);
					buff.Vars.Set("Melia.Link.IsAnchor", true);

					firstTarget.AddEffect("Melia.Link.Chain", new AttachEffect("I_chain004_mash_loop_multi", 2, EffectLocation.Bottom));
					firstTarget.SetTempVar("LINK_BUFF", BuffId.Link_Enemy.ToString());
				}
			}

			// Create visual effects only for new connections (anchor to new targets)
			var anchorHandle = anchor.Handle;
			var existingVisualCount = existingMembers.Count > 0 ? existingMembers.Count - 1 : 0;

			for (var i = 0; i < newTargets.Count; i++)
			{
				var pairHandles = new List<int> { anchorHandle, newTargets[i].Handle };
				var pairLinkId = ZoneServer.Instance.World.CreateLinkHandle();

				var visualIndex = existingVisualCount + i + 1;
				var linkerEffect = new LinkerVisualEffect(pairLinkId, "Linker_cable_basic", true, pairHandles, 0.2f, "F_scout_JointPenalty_hit", 0.4f, "swd_blow_cloth2");
				caster.AddEffect($"Link_{linkId}_{visualIndex}", linkerEffect);
			}
		}
	}
}
