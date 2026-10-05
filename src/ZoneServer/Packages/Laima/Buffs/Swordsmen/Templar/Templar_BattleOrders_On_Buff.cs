using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Templar
{
	[Package("laima")]
	[BuffHandler(BuffId.BattleOrders_On_Buff)]
	public class Templar_BattleOrders_On_BuffOverride : BuffHandler
	{
		private const int UpdateInterval = 1000;
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int MaximumSpCost = 18;
		private const int MinimumSpCost = 8;
		private const float AuraRange = 120f;
		private static readonly TimeSpan PartyBuffDuration = TimeSpan.FromMilliseconds(1500);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(UpdateInterval);
			this.UpdateAura(buff, true);
		}

		public override void WhileActive(Buff buff)
		{
			this.UpdateAura(buff, true);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			this.RemoveDistributedBuff(character);

			if (character.Map == null)
				return;

			var partyMembers = character.Map
				.GetPartyMembersInRange(character, AuraRange, true)
				.Where(member => member != null && member != character)
				.Distinct()
				.ToList();

			foreach (var member in partyMembers)
				this.RemoveDistributedBuff(member, character);
		}

		private void UpdateAura(Buff buff, bool consumeSp)
		{
			if (buff.Target is not Character character || character.IsDead)
			{
				buff.Target?.StopBuff(BuffId.BattleOrders_On_Buff);
				return;
			}

			if (character.Map == null)
			{
				character.StopBuff(BuffId.BattleOrders_On_Buff);
				return;
			}

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);

			if (consumeSp)
			{
				var spCost = this.GetSpCost(skillLevel);

				if (!character.TrySpendSp(spCost))
				{
					character.StopBuff(BuffId.BattleOrders_On_Buff);
					return;
				}
			}

			character.StartBuff(BuffId.BattleOrders_Buff, skillLevel, 0f, PartyBuffDuration, character, buff.SkillId);

			var partyMembers = character.Map
				.GetPartyMembersInRange(character, AuraRange, true)
				.Where(member => member != null && !member.IsDead && member != character)
				.Distinct()
				.ToList();

			foreach (var member in partyMembers)
				member.StartBuff(BuffId.BattleOrders_Buff, skillLevel, 0f, PartyBuffDuration, character, buff.SkillId);
		}

		private int GetSpCost(int skillLevel)
		{
			var levelProgress = skillLevel - MinimumSkillLevel;
			var totalDifference = MaximumSpCost - MinimumSpCost;
			var reduction = (int)Math.Floor(levelProgress * totalDifference / (float)(MaximumSkillLevel - MinimumSkillLevel));
			return Math.Max(MinimumSpCost, MaximumSpCost - reduction);
		}

		private void RemoveDistributedBuff(Character target, Character expectedCaster = null)
		{
			if (!target.TryGetBuff(BuffId.BattleOrders_Buff, out var distributedBuff))
				return;

			if (expectedCaster != null && distributedBuff.Caster != expectedCaster)
				return;

			if (expectedCaster == null && distributedBuff.Caster != target)
				return;

			target.StopBuff(BuffId.BattleOrders_Buff);
		}
	}
}
