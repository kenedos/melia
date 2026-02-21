using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Linker
{
	/// <summary>
	/// Handler for the Link_Party buff (Spiritual Chain).
	/// Shares buffs among linked party members with reduced duration.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Link_Party)]
	public class Link_PartyOverride : BuffHandler
	{
		private const float MaxHorizontalDistance = 250f;
		private const int UpdateIntervalMs = 200;

		private static readonly HashSet<BuffId> ExcludedBuffs = new HashSet<BuffId>
		{
			BuffId.Link_Party,
			BuffId.Link_Physical,
			BuffId.Link_Sacrifice,
			BuffId.Link_Enemy,
			BuffId.Link,
		};

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(UpdateIntervalMs);

			if (buff.Target.Map != null)
				buff.Vars.Set("Melia.Link.MapId", buff.Target.Map.Id);

			buff.Vars.Set("Melia.Link.ProcessedBuffs", new HashSet<int>());
		}

		public override void WhileActive(Buff buff)
		{
			var isCaster = buff.Vars.GetBool("Melia.Link.IsCaster");

			// Chain breaking logic
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

				// Buff sharing logic - only caster processes to avoid duplicates
				this.ProcessBuffSharing(buff);
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
		/// Calculates the debuff duration reduction from Unbind passive.
		/// Formula: 50% + (UnbindLevel * 5%)
		/// At level 10, debuffs are not shared (100% reduction).
		/// </summary>
		private float GetUnbindDebuffReduction(int unbindLevel)
		{
			if (unbindLevel <= 0)
				return 0f;

			var reduction = 0.50f + (unbindLevel * 0.05f);
			return Math.Min(reduction, 1f);
		}

		private void ProcessBuffSharing(Buff buff)
		{
			if (!buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
				return;

			var processedBuffs = buff.Vars.Get<HashSet<int>>("Melia.Link.ProcessedBuffs", new HashSet<int>());
			var skillLevel = buff.NumArg1;
			var durationMultiplier = 0.10f + 0.02f * skillLevel;

			var unbindLevel = this.GetUnbindSkillLevel(buff);
			var debuffReduction = this.GetUnbindDebuffReduction(unbindLevel);

			// Collect linked members
			var linkedMembers = new List<ICombatEntity>();
			foreach (var handle in memberHandles)
			{
				if (buff.Target.Map != null && buff.Target.Map.TryGetCombatEntity(handle, out var member))
				{
					if (!member.IsDead && member.IsBuffActive(BuffId.Link_Party))
						linkedMembers.Add(member);
				}
			}

			if (linkedMembers.Count < 2)
				return;

			// Check each member for new buffs to share
			foreach (var member in linkedMembers)
			{
				if (!(member is Character character))
					continue;

				foreach (var memberBuff in character.Buffs.GetList())
				{
					// Skip if already processed
					if (processedBuffs.Contains(memberBuff.Handle))
						continue;

					// Skip excluded buffs (link buffs, etc.)
					if (ExcludedBuffs.Contains(memberBuff.Id))
						continue;

					// Skip if this buff was propagated from another member
					if (memberBuff.Vars.GetBool("Melia.Link.Propagated"))
					{
						processedBuffs.Add(memberBuff.Handle);
						continue;
					}

					// Skip infinite duration buffs
					if (memberBuff.Duration == TimeSpan.Zero || memberBuff.Duration.TotalMilliseconds < 0)
					{
						processedBuffs.Add(memberBuff.Handle);
						continue;
					}

					// Mark as processed
					processedBuffs.Add(memberBuff.Handle);

					// Check if this is a debuff and apply Unbind passive effect
					var isDebuff = memberBuff.Data.Type == BuffType.Debuff;
					if (isDebuff && debuffReduction >= 1f)
						continue;

					// Calculate reduced duration
					var effectiveMultiplier = durationMultiplier;
					if (isDebuff && debuffReduction > 0f)
						effectiveMultiplier *= (1f - debuffReduction);

					var newDuration = TimeSpan.FromTicks((long)(memberBuff.Duration.Ticks * effectiveMultiplier));
					if (newDuration.TotalSeconds < 1)
						newDuration = TimeSpan.FromSeconds(1);

					// Propagate to other linked members
					foreach (var otherMember in linkedMembers)
					{
						if (otherMember.Handle == member.Handle)
							continue;

						// Don't apply if target already has this buff with longer duration
						if (otherMember.TryGetBuff(memberBuff.Id, out var existingBuff))
						{
							if (existingBuff.RemainingDuration >= newDuration)
								continue;
						}

						var newBuff = otherMember.StartBuff(
							memberBuff.Id,
							memberBuff.NumArg1,
							memberBuff.NumArg2,
							newDuration,
							memberBuff.Caster,
							memberBuff.SkillId
						);

						if (newBuff != null)
							newBuff.Vars.Set("Melia.Link.Propagated", true);
					}
				}
			}

			buff.Vars.Set("Melia.Link.ProcessedBuffs", processedBuffs);
		}

		private void RemoveMemberChain(Buff buff)
		{
			if (buff.Vars.TryGet<int>("Melia.Link.Id", out var linkId) &&
				buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles) &&
				buff.Caster != null)
			{
				var memberIndex = memberHandles.IndexOf(buff.Target.Handle);
				if (memberIndex > 0)
					buff.Caster.RemoveEffect($"Link_{linkId}_{memberIndex}");
			}

			buff.Target.StopBuff(BuffId.Link_Party);
		}

		private void RemoveAllChains(Buff buff)
		{
			if (!buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
			{
				buff.Target.StopBuff(BuffId.Link_Party);
				return;
			}

			foreach (var handle in memberHandles)
			{
				if (buff.Target.Map != null && buff.Target.Map.TryGetCombatEntity(handle, out var member))
					member.StopBuff(BuffId.Link_Party);
			}
		}

		public override void OnEnd(Buff buff)
		{
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
