using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Effects;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Defines the visual topology for link effects.
	/// </summary>
	public enum LinkTopology
	{
		/// <summary>
		/// Chain topology: A -> B -> C -> D (sequential)
		/// </summary>
		Chain,

		/// <summary>
		/// Star topology: A -> B, A -> C, A -> D (anchor connects to all)
		/// </summary>
		Star
	}

	/// <summary>
	/// Provides helper functions for Linker class skills.
	/// NOTE: This implementation makes several assumptions about the underlying systems,
	/// such as a Link Management system and specific network packets, which are commented accordingly.
	/// The damage sharing logic (TAKE_DMG_*) belongs in buff handlers and is not included here.
	/// </summary>
	public static class LinkerSkillHelper
	{
		#region Link Creation

		/// <summary>
		/// Initiates a link by selecting targets in a forward area.
		/// </summary>
		public static void MakeLink(ICombatEntity caster, Skill skill,
			float dist1, float angle1, float height, float width,
			RelationType targetType, BuffId buffId, TimeSpan maxTime, float splashRange,
			bool addCaster, int linkCount, string linkName, bool cancelByMove,
			float linkSecond, string linkEft, float linkEftScale, string linkSound)
		{
			// In Lua, this removes existing links. We replicate by destroying them.
			LinkDestruct(caster, buffId);
			if (buffId == BuffId.Link_Physical)
			{
				LinkDestruct(caster, BuffId.Link_Party);
			}

			var startPos = caster.Position.GetRelative(caster.Direction.AddDegreeAngle(angle1), dist1);
			var endPos = caster.Position.GetRelative(caster.Direction.AddDegreeAngle(angle1), height);
			var targets = caster.SelectObjectBySquareCoor(targetType, startPos, endPos, width, 50);

			if (targets.Length == 0) return;

			ICombatEntity firstTarget = null;
			foreach (var target in targets)
			{
				if (target.Handle == caster.Handle) continue;

				var ableLink = true;
				if (caster is Character && (buffId == BuffId.Link || buffId == BuffId.Link_Physical) && target is not Character)
				{
					ableLink = false;
				}

				if (ableLink)
				{
					firstTarget = target;
					break;
				}
			}

			if (buffId == BuffId.SpiritShock_Debuff)
			{
				var target = caster.GetTargets().FirstOrDefault();
				if (target != null && target.Rank != MonsterRank.Material)
				{
					firstTarget = target;
				}
			}

			if (firstTarget != null)
			{
				MakeLinkFromTarget(caster, skill, firstTarget, targetType, buffId, maxTime, splashRange, addCaster, linkCount, linkName, cancelByMove, linkSecond, linkEft, linkEftScale, linkSound);
			}
		}

		/// <summary>
		/// Creates a link starting from a specific target, connecting to nearby valid targets.
		/// </summary>
		public static void MakeLinkFromTarget(ICombatEntity caster, Skill skill, ICombatEntity firstTarget,
			RelationType targetType, BuffId buffId, TimeSpan maxTime, float splashRange,
			bool addCaster, int linkCount, string linkName, bool cancelByMove,
			float linkSecond, string linkEft, float linkEftScale, string linkSound, int hitCount = 0)
		{
			if (IsBuffIgnore(firstTarget, buffId))
			{
				firstTarget.PlayTextEffect("I_SYS_Text_Effect_Skill", "Debuff_Resister");
				firstTarget.PlayEffect("I_sphere001_mash2", 1.0f);
				return;
			}

			var linkedEntities = new List<ICombatEntity> { firstTarget };

			var nearbyTargets = caster.SelectObjectNear(firstTarget, splashRange, targetType);
			var minY = firstTarget.Position.Y;
			var maxY = firstTarget.Position.Y;

			foreach (var target in nearbyTargets.OrderBy(t => t.GetDistance(firstTarget)).Take(linkCount))
			{
				var ableLink = target.Handle != firstTarget.Handle;
				if (caster is Character && (buffId == BuffId.Link || buffId == BuffId.Link_Physical) && target is not Character)
				{
					ableLink = false;
				}

				if (ableLink)
				{
					if (IsBuffIgnore(target, buffId))
					{
						target.PlayTextEffect("I_SYS_Text_Effect_Skill", "Debuff_Resister");
						target.PlayEffect("I_sphere001_mash2", 1.0f);
					}
					else
					{
						var targetY = target.Position.Y;
						var currentMinY = minY;
						var currentMaxY = maxY;

						minY = Math.Min(minY, targetY);
						maxY = Math.Max(maxY, targetY);

						if (maxY - minY <= 25.0f) // Vertical distance check
						{
							linkedEntities.Add(target);
						}
						else
						{
							minY = currentMinY;
							maxY = currentMaxY;
						}
					}
				}
			}

			var shouldStartLink = (addCaster && linkedEntities.Count > 0) || (!addCaster && linkedEntities.Count > 1);
			if (addCaster && !linkedEntities.Contains(caster)) linkedEntities.Add(caster);

			if (shouldStartLink)
			{
				var handles = linkedEntities.Select(e => e.Handle).ToList();

				var linkId = ZoneServer.Instance.World.CreateLinkHandle();
				foreach (var entity in linkedEntities)
				{
					var buff = entity.StartBuff(buffId, skill.Level, 0, maxTime, caster, skill.Id);
					if (buff != null)
					{
						buff.Vars.Set("Melia.Link.Id", linkId);
						buff.Vars.Set("Melia.Link.Caster", caster.Handle);
						buff.Vars.Set("Melia.Link.Members", handles);
						buff.Vars.Set("Melia.Link.Topology", (int)LinkTopology.Chain);
						if (entity.Handle == firstTarget.Handle)
						{
							buff.Vars.Set("Melia.Link.IsAnchor", true);
							firstTarget.AddEffect("Melia.Link.Chain", new AttachEffect("I_chain004_mash_loop_multi", 2, EffectLocation.Bottom));
						}
						if (hitCount > 0)
						{
							buff.Vars.Set("Melia.HitCount", hitCount);
							buff.Vars.Set("Melia.RemainingHits", hitCount);
						}
						entity.SetTempVar("LINK_BUFF", buffId.ToString());
					}
				}

				var linkerEffect = new LinkerVisualEffect(linkId, linkName, true, handles, linkSecond, linkEft, linkEftScale, linkSound);
				caster.AddEffect($"Link_{linkId}", linkerEffect);
			}
		}

		#endregion

		#region Link Interaction

		/// <summary>
		/// Destroys all links of a specific type created by the entity.
		/// </summary>
		public static void LinkDestruct(ICombatEntity caster, BuffId buffId, float linkSecond = 0, string linkEft = "None", float linkEftScale = 0, string sound = "None", Skill skill = null)
		{
			var allLinks = FindAllLinks(caster, buffId);

			foreach (var linkMembers in allLinks)
			{
				if (linkMembers.Count == 0) continue;

				if (buffId == BuffId.Link_Enemy && skill != null && caster.TryGetActiveAbility(AbilityId.Linker7, out var linker7Abil))
				{
					var damage = caster.Properties.GetFloat(PropertyName.MAXMATK);
					damage *= (1 + (linker7Abil.Level - 1) * 0.1f);
					foreach (var member in linkMembers)
					{
						member.TakeDamage(damage, caster);
					}
				}

				// Removing the buff from each member will trigger its OnEnd handler,
				// which is now responsible for destroying the visual link effect.
				foreach (var member in linkMembers)
				{
					member.RemoveBuff(buffId);
				}
			}
		}

		/// <summary>
		/// Gathers linked enemies to a central point.
		/// </summary>
		public static async Task LinkGather(ICombatEntity caster, Skill skill, BuffId buffName, int gatherType, float time)
		{
			var allLinks = FindAllLinks(caster, buffName);
			foreach (var linkMembers in allLinks)
			{
				var enemies = linkMembers.Where(m => m.Handle != caster.Handle).ToList();
				if (enemies.Count < 2) continue;

				Position destination;
				switch (gatherType)
				{
					case 0: // Average position
						float sumX = 0, sumZ = 0, maxY = float.MinValue;
						foreach (var enemy in enemies)
						{
							sumX += enemy.Position.X;
							sumZ += enemy.Position.Z;
							if (enemy.Position.Y > maxY) maxY = enemy.Position.Y;
						}
						destination = new Position(sumX / enemies.Count, maxY + 5, sumZ / enemies.Count);
						break;
					case 1: // Position of the first enemy
						destination = enemies[0].Position + new Position(0, 5, 0);
						break;
					case 2: // In front of caster
					default:
						destination = caster.Position.GetRelative(caster.Direction, 10);
						destination.Y = caster.Map.Ground.GetHeightAt(destination) + 5;
						break;
				}

				foreach (var enemy in enemies)
				{
					if (enemy.Rank == MonsterRank.Boss) continue;

					enemy.CancelMonsterSkill();
					if (enemy.MoveType != MoveType.Holding)
					{
						enemy.ForceMoveTo(destination, -1, time);
					}

					enemy.StartBuff(BuffId.HangmansKnot_Debuff, 1, 0, TimeSpan.FromMilliseconds(1000 + skill.Level * 200), caster);

					if (caster.TryGetActiveAbility(AbilityId.Linker2, out var linker2Abil) && skill.Level >= 3)
					{
						var damageResult = SCR_SkillHit(caster, enemy, skill);
						var damage = damageResult.Damage * linker2Abil.Level * 0.2f;
						if (damage < 1) damage = 1;

						// Add a buff that deals damage after a delay.
						var dotBuff = enemy.StartBuff(BuffId.DelayDamage, damage, 0, TimeSpan.FromMilliseconds(time * 1000 + 300), caster);
						dotBuff?.Vars.Set("HitType", HitResultType.Hit);
					}

					if (caster.TryGetActiveAbility(AbilityId.Linker6, out var linker6Abil))
					{
						enemy.StartBuff(BuffId.Hangmansknot_SDR_Debuff, linker6Abil.Level, 0, TimeSpan.FromMilliseconds(3000), caster);
					}
				}
			}
		}

		#endregion

		#region Helpers

		// Helper method to simulate Lua's IsBuffIgnore.
		private static bool IsBuffIgnore(ICombatEntity target, BuffId buffId)
		{
			// Placeholder for actual buff immunity logic (e.g., checking for specific buffs like 'Invincible')
			return false;
		}

		// Helper to find all distinct links and their member lists.
		public static List<List<ICombatEntity>> FindAllLinks(ICombatEntity caster, BuffId buffId)
		{
			var allLinks = new List<List<ICombatEntity>>();
			var processedBuffs = new HashSet<int>();

			foreach (var actor in caster.Map.GetAttackableEntities(caster, 500))
			{
				if (actor.TryGetBuff(buffId, out var buff) && buff.Caster?.Handle == caster.Handle && buff.Vars != null)
				{
					if (processedBuffs.Contains(buff.Handle)) continue;

					if (buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
					{
						var members = new List<ICombatEntity>();
						var success = true;
						foreach (var handle in memberHandles)
						{
							if (caster.Map.TryGetCombatEntity(handle, out var member))
							{
								members.Add(member);
								if (member.TryGetBuff(buffId, out var memberBuff))
								{
									processedBuffs.Add(memberBuff.Handle);
								}
							}
							else
							{
								success = false;
								break;
							}
						}
						if (success && members.Any()) allLinks.Add(members);
					}
				}
			}
			return allLinks;
		}

		#endregion
	}
}
