using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

namespace Melia.Zone.Buffs.Handlers.Scouts.Thaumaturge
{
	[Package("laima")]
	[BuffHandler(BuffId.SwellRightArm_Buff)]
	public class SwellRightArm_BuffOverride : BuffHandler
	{
		private const string VarItemGuid = "Melia.SwellArm.ItemGuid";

		private void ApplyOrUpdateEffects(Buff buff)
		{
			this.RemoveEffects(buff);

			if (buff.Target is not Character target || buff.Caster is not ICombatEntity caster) return;

			var skillLevel = buff.NumArg1;

			if (!target.TryGetEquipItem(EquipSlot.LeftHand, out var offHandItem))
			{
				caster.TryGetActiveAbilityLevel(AbilityId.Thaumaturge8, out var abilLevel);
				var hrBonus = abilLevel * 30f;
				var drBonus = abilLevel * 30f;

				AddPropertyModifier(buff, target, PropertyName.HR_BM, hrBonus);
				AddPropertyModifier(buff, target, PropertyName.DR_BM, drBonus);
			}
			else
			{
				var addValue = 0f;
				var isShield = false;

				if (offHandItem.Data.EquipType1 == EquipType.Shield)
				{
					addValue = 90f + (skillLevel - 1) * 20f;
					isShield = true;
				}
				else if (IsOffHandWeapon(offHandItem.Data.EquipType1))
				{
					addValue = 45f + (skillLevel - 1) * 10f;
				}
				else
				{
					return;
				}

				var casterInt = caster.Properties.GetFloat(PropertyName.INT);
				var casterMsp = caster.Properties.GetFloat(PropertyName.MSP);
				addValue += (skillLevel / 5f) * (float)Math.Pow((casterInt + casterMsp) * (isShield ? 0.7 : 0.6), 0.9);

				if (caster.TryGetSkill(SkillId.Thaumaturge_SwellRightArm, out var skill) && skill.Level >= 3)
				{
					if (caster.TryGetActiveAbilityLevel(AbilityId.Thaumaturge14, out var thaum14Level))
					{
						addValue *= (1 + thaum14Level * 0.01f);
					}
				}

				addValue = (float)Math.Floor(addValue);

				buff.Vars.SetString(VarItemGuid, offHandItem.ObjectId.ToString());

				if (isShield)
					AddPropertyModifier(buff, target, PropertyName.DEF_BM, addValue);
				else
					AddPropertyModifier(buff, target, PropertyName.PATK_SUB_BM, addValue);
			}
		}

		private void RemoveEffects(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.HR_BM);
			RemovePropertyModifier(buff, target, PropertyName.DR_BM);
			RemovePropertyModifier(buff, target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, target, PropertyName.PATK_SUB_BM);

			buff.Vars.Remove(VarItemGuid);
		}

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			ApplyOrUpdateEffects(buff);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character)
			{
				buff.Target.StopBuff(buff.Id);
				return;
			}

			character.TryGetEquipItem(EquipSlot.LeftHand, out var offHandItem);
			var currentGuid = offHandItem?.ObjectId.ToString() ?? "None";
			var storedGuid = buff.Vars.GetString(VarItemGuid, "None");

			if (currentGuid != storedGuid)
			{
				var remaining = buff.RemainingDuration - TimeSpan.FromSeconds(1);
				if (remaining > TimeSpan.Zero)
				{
					character.StartBuff(buff.Id, buff.NumArg1, buff.NumArg2, remaining, buff.Caster, buff.SkillId);
				}
				else
				{
					character.StopBuff(buff.Id);
				}
			}
		}

		public override void OnEnd(Buff buff)
		{
			this.RemoveEffects(buff);
		}

		private bool IsOffHandWeapon(EquipType attachType)
		{
			return attachType == EquipType.Dagger || attachType == EquipType.Pistol || attachType == EquipType.Sword || attachType == EquipType.Cannon;
		}
	}
}
