using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Linker
{
	/// <summary>
	/// Handler for the Link_Sacrifice buff (Lifeline).
	/// Shares the highest base stats among linked party members.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Link_Sacrifice)]
	public class Link_SacrificeOverride : BuffHandler
	{
		private const float MaxHorizontalDistance = 250f;
		private const int UpdateIntervalMs = 500;

		private static readonly string[] SharedStats = { "STR", "DEX", "CON", "INT", "MNA" };

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(UpdateIntervalMs);

			if (buff.Target.Map != null)
				buff.Vars.Set("Melia.Link.MapId", buff.Target.Map.Id);

			// Initialize bonus tracking
			foreach (var stat in SharedStats)
				buff.Vars.Set($"Melia.Link.Bonus.{stat}", 0f);
		}

		public override void WhileActive(Buff buff)
		{
			var isCaster = buff.Vars.GetBool("Melia.Link.IsCaster");

			if (isCaster)
			{
				if (buff.Target.IsDead)
				{
					this.RemoveAllChains(buff);
					return;
				}

				var storedMapId = buff.Vars.GetInt("Melia.Link.MapId", 0);
				if (buff.Target.Map == null || buff.Target.Map.Id != storedMapId)
				{
					this.RemoveAllChains(buff);
					return;
				}

				// Stat sharing - caster processes for all members
				this.ProcessStatSharing(buff);
			}
			else
			{
				if (buff.Target.IsDead)
				{
					this.RemoveMemberChain(buff);
					return;
				}

				var storedMapId = buff.Vars.GetInt("Melia.Link.MapId", 0);
				if (buff.Target.Map == null || buff.Target.Map.Id != storedMapId)
				{
					this.RemoveMemberChain(buff);
					return;
				}

				if (buff.Caster == null || buff.Caster is not ICombatEntity casterEntity)
				{
					this.RemoveMemberChain(buff);
					return;
				}

				var distance = buff.Target.Position.Get2DDistance(casterEntity.Position);
				if (distance > MaxHorizontalDistance)
				{
					this.RemoveMemberChain(buff);
					return;
				}
			}
		}

		/// <summary>
		/// Gets the Unbind skill level from the link caster.
		/// </summary>
		private int GetUnbindSkillLevel(Buff buff)
		{
			if (buff.Caster is Character caster && caster.TryGetSkill(SkillId.Linker_Unbind, out var unbindSkill))
				return unbindSkill.Level;

			return 0;
		}

		/// <summary>
		/// Calculates the share rate bonus from Unbind passive.
		/// Formula: 10% + (UnbindLevel * 1%)
		/// </summary>
		private float GetUnbindShareRateBonus(int unbindLevel)
		{
			if (unbindLevel <= 0)
				return 0f;

			return 0.10f + (unbindLevel * 0.01f);
		}

		private void ProcessStatSharing(Buff buff)
		{
			if (!buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
				return;

			var skillLevel = buff.NumArg1;
			var baseShareRate = 0.30f + 0.03f * skillLevel;

			var unbindLevel = this.GetUnbindSkillLevel(buff);
			var unbindBonus = this.GetUnbindShareRateBonus(unbindLevel);

			var shareRate = Math.Min(baseShareRate + unbindBonus, 1.0f);

			// Collect linked members and their buffs
			var linkedMembers = new List<(ICombatEntity entity, Buff buff)>();
			foreach (var handle in memberHandles)
			{
				if (buff.Target.Map != null && buff.Target.Map.TryGetCombatEntity(handle, out var member))
				{
					if (!member.IsDead && member.TryGetBuff(BuffId.Link_Sacrifice, out var memberBuff))
						linkedMembers.Add((member, memberBuff));
				}
			}

			if (linkedMembers.Count < 2)
			{
				// Clear bonuses for any remaining single member since there's no one to share with
				foreach (var (member, memberBuff) in linkedMembers)
					this.ClearMemberBonuses(member, memberBuff);

				return;
			}

			// Calculate base stats for each member (current value - applied bonus)
			var memberBaseStats = new Dictionary<int, Dictionary<string, float>>();
			foreach (var (member, memberBuff) in linkedMembers)
			{
				memberBaseStats[member.Handle] = new Dictionary<string, float>();
				foreach (var stat in SharedStats)
				{
					var currentValue = member.Properties.GetFloat(stat);
					var appliedBonus = memberBuff.Vars.GetFloat($"Melia.Link.Bonus.{stat}", 0);
					memberBaseStats[member.Handle][stat] = currentValue - appliedBonus;
				}
			}

			// Find highest base stat for each stat type
			var highestStats = new Dictionary<string, float>();
			foreach (var stat in SharedStats)
			{
				highestStats[stat] = 0;
				foreach (var (member, _) in linkedMembers)
				{
					var baseValue = memberBaseStats[member.Handle][stat];
					if (baseValue > highestStats[stat])
						highestStats[stat] = baseValue;
				}
			}

			// Apply bonuses to each member and track who needs property updates
			var membersToUpdate = new HashSet<ICombatEntity>();

			foreach (var (member, memberBuff) in linkedMembers)
			{
				foreach (var stat in SharedStats)
				{
					var baseValue = memberBaseStats[member.Handle][stat];
					var newBonus = (float)Math.Ceiling((highestStats[stat] - baseValue) * shareRate);
					var currentBonus = memberBuff.Vars.GetFloat($"Melia.Link.Bonus.{stat}", 0);

					if (Math.Abs(newBonus - currentBonus) > 0.01f)
					{
						var propName = $"{stat}_BM";

						if (currentBonus > 0)
							RemovePropertyModifier(memberBuff, member, propName);

						if (newBonus > 0)
							AddPropertyModifier(memberBuff, member, propName, newBonus);

						memberBuff.Vars.Set($"Melia.Link.Bonus.{stat}", newBonus);
						membersToUpdate.Add(member);
					}
				}
			}

			// Invalidate and send property updates for members whose stats changed
			foreach (var member in membersToUpdate)
			{
				if (member is Character character)
					character.InvalidateProperties();
			}
		}

		private void RemoveMemberChain(Buff buff)
		{
			// Remove stat bonuses first
			this.RemoveStatBonuses(buff);

			if (buff.Vars.TryGet<int>("Melia.Link.Id", out var linkId) &&
				buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles) &&
				buff.Caster != null)
			{
				var memberIndex = memberHandles.IndexOf(buff.Target.Handle);
				if (memberIndex > 0)
					buff.Caster.RemoveEffect($"Link_{linkId}_{memberIndex}");
			}

			buff.Target.StopBuff(BuffId.Link_Sacrifice);
		}

		private void RemoveAllChains(Buff buff)
		{
			if (!buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
			{
				this.RemoveStatBonuses(buff);
				buff.Target.StopBuff(BuffId.Link_Sacrifice);
				return;
			}

			foreach (var handle in memberHandles)
			{
				if (buff.Target.Map != null && buff.Target.Map.TryGetCombatEntity(handle, out var member))
					member.StopBuff(BuffId.Link_Sacrifice);
			}
		}

		private void RemoveStatBonuses(Buff buff)
		{
			foreach (var stat in SharedStats)
			{
				RemovePropertyModifier(buff, buff.Target, $"{stat}_BM");
				buff.Vars.Set($"Melia.Link.Bonus.{stat}", 0f);
			}

			if (buff.Target is Character character)
				character.InvalidateProperties();
		}

		private void ClearMemberBonuses(ICombatEntity member, Buff memberBuff)
		{
			foreach (var stat in SharedStats)
			{
				RemovePropertyModifier(memberBuff, member, $"{stat}_BM");
				memberBuff.Vars.Set($"Melia.Link.Bonus.{stat}", 0f);
			}

			if (member is Character character)
				character.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
			// Remove stat bonuses
			this.RemoveStatBonuses(buff);

			if (buff.Vars.GetBool("Melia.Link.IsCaster"))
			{
				buff.Target.RemoveEffect("Melia.Link.Chain");

				if (buff.Vars.TryGet<int>("Melia.Link.Id", out var linkId) && linkId != 0)
				{
					if (buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var members))
					{
						for (var i = 1; i < members.Count; i++)
							buff.Caster?.RemoveEffect($"Link_{linkId}_{i}");
					}
				}
			}
		}
	}
}
