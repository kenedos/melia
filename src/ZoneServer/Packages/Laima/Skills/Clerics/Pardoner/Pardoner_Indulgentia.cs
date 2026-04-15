using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Geometry.Shapes;

namespace Melia.Zone.Skills.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Pardoner skill Indulgentia.
	/// Applies the Indulgentia buff to nearby allies. While the buff is active,
	/// targets recover HP in regular intervals.
	/// The amount of HP recovered increases by 10% when the Guardian Saint buff is active.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Pardoner_Indulgentia)]
	public class Pardoner_IndulgentiaOverride : IGroundSkillHandler
	{
		private const float BuffDurationMs = 10000f; // 10 seconds
		private const float TargetRange = 150f;
		private const int MaxTargets = 10;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var buffDuration = TimeSpan.FromMilliseconds(BuffDurationMs);

			// Apply buff to caster first
			caster.StartBuff(BuffId.Indulgentia_Buff, skill.Level, 0f, buffDuration, caster);

			// Get friendly targets in range
			var targetPos = caster.Position.GetRelative(caster.Direction, TargetRange / 2);
			var skillTargets = GetFriendlyTargetsInRange(caster, skill, targetPos, TargetRange, MaxTargets);

			// Apply buff to all friendly targets
			foreach (var target in skillTargets)
			{
				if (target == caster)
					continue;

				target.StartBuff(BuffId.Indulgentia_Buff, skill.Level, 0f, buffDuration, caster);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(110));

			// Remove debuffs from all buffed targets (including caster)
			RemoveDebuffsFromTarget(caster);
			foreach (var target in skillTargets)
			{
				RemoveDebuffsFromTarget(target);
			}
		}

		/// <summary>
		/// Gets friendly targets in range (party members and self).
		/// </summary>
		private List<ICombatEntity> GetFriendlyTargetsInRange(ICombatEntity caster, Skill skill, Position center, float radius, int maxTargets)
		{
			var targets = new List<ICombatEntity>();

			if (caster is Character character)
			{
				// Add party members in range
				var party = character.Connection.Party;
				if (party != null)
				{
					var members = caster.Map.GetPartyMembersInRange(character, radius, true);
					foreach (var member in members)
					{
						if (targets.Count >= maxTargets)
							break;
						targets.Add(member);
					}
				}
				else
				{
					// No party, just add self
					targets.Add(caster);
				}
			}

			return targets;
		}

		/// <summary>
		/// Removes removable debuffs from the target.
		/// </summary>
		private void RemoveDebuffsFromTarget(ICombatEntity target)
		{
			var debuffsToRemove = new List<BuffId>();

			// Check if the buff is a debuff and can be removed
			foreach (var buff in target.Components.Get<BuffComponent>()
				.GetAll(buff => buff.Data.Type == BuffType.Debuff && buff.Data.Removable))
			{

				debuffsToRemove.Add(buff.Id);
			}

			foreach (var buffId in debuffsToRemove)
			{
				target.RemoveBuff(buffId);
			}
		}
	}
}
