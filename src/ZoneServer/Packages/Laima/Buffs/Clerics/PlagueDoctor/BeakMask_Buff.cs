using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Util;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handles Beak Mask debuff protection and poison resistance.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.BeakMask_Buff)]
	public class BeakMask_BuffOverride : BuffHandler
	{
		private const int MaximumBlockedDebuffs = 5;
		private const float PoisonResistanceBonus = 5f;
		private const float LevelThreeResistanceChancePerSkillLevel = 5f;
		private const string BlockedDebuffCountVariable = "PlagueDoctor.BeakMask.BlockedDebuffs";
		private const string PoisonResistanceVariable = "PlagueDoctor.BeakMask.PoisonResistance";
		private bool _removingBlockedDebuff;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType != ActivationType.Start)
				return;

			buff.Vars.Set(BlockedDebuffCountVariable, 0);

			var buffComponent = buff.Target.Components.Get<BuffComponent>();

			if (buffComponent != null)
				buffComponent.BuffStarted += this.OnBuffStarted;

			if (buff.Target is Character character && character.IsAbilityActive(AbilityId.PlagueDoctor8))
			{
				buff.Vars.Set(PoisonResistanceVariable, PoisonResistanceBonus);
				character.Properties.Modify(PropertyName.ResPoison_BM, PoisonResistanceBonus);
			}
		}

		public override void OnEnd(Buff buff)
		{
			var buffComponent = buff.Target.Components.Get<BuffComponent>();

			if (buffComponent != null)
				buffComponent.BuffStarted -= this.OnBuffStarted;

			var poisonResistanceBonus = buff.Vars.GetFloat(PoisonResistanceVariable);

			if (poisonResistanceBonus != 0)
				buff.Target.Properties.Modify(PropertyName.ResPoison_BM, -poisonResistanceBonus);
		}

		private void OnBuffStarted(ICombatEntity entity, Buff appliedBuff)
		{
			if (_removingBlockedDebuff)
				return;

			if (entity == null || appliedBuff == null)
				return;

			if (!entity.TryGetBuff(BuffId.BeakMask_Buff, out var beakMask))
				return;

			if (!this.ShouldBlockDebuff(beakMask, appliedBuff))
				return;

			var buffComponent = entity.Components.Get<BuffComponent>();

			if (buffComponent == null)
				return;

			_removingBlockedDebuff = true;

			try
			{
				buffComponent.Remove(appliedBuff.Id);

				var blockedDebuffCount = beakMask.Vars.GetInt(BlockedDebuffCountVariable) + 1;
				beakMask.Vars.Set(BlockedDebuffCountVariable, blockedDebuffCount);
				beakMask.NotifyUpdate();

				if (blockedDebuffCount >= MaximumBlockedDebuffs)
					buffComponent.Remove(BuffId.BeakMask_Buff);
			}
			finally
			{
				_removingBlockedDebuff = false;
			}
		}

		private bool ShouldBlockDebuff(Buff beakMask, Buff appliedBuff)
		{
			if (appliedBuff.Id == BuffId.BeakMask_Buff)
				return false;

			if (appliedBuff.Data.Type != BuffType.Debuff)
				return false;

			if (this.IsExcludedStatus(appliedBuff))
				return false;

			if (appliedBuff.Data.RemoveBySkill)
				return true;

			if (appliedBuff.Data.Level != 3)
				return false;

			if (!beakMask.Target.IsAbilityActive(AbilityId.PlagueDoctor7))
				return false;

			var skillLevel = Math.Clamp((int)beakMask.NumArg1, 1, 10);
			var resistanceChance = skillLevel * LevelThreeResistanceChancePerSkillLevel;

			return RandomProvider.Get().Next(100) < resistanceChance;
		}

		private bool IsExcludedStatus(Buff appliedBuff)
		{
			return appliedBuff.Data.Tags.HasAny("Stun", "Hold", "Bind", "Immobilize", "Immovable", "Slow");
		}
	}
}
