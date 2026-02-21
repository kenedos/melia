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
		private const string VarAddType = "Melia.SwellArm.AddType";
		private const string VarItemGuid = "Melia.SwellArm.ItemGuid";
		private const string VarBonusValue = "Melia.SwellArm.BonusValue";
		private const string VarHrBonus = "Melia.SwellArm.HrBonus";
		private const string VarDrBonus = "Melia.SwellArm.DrBonus";

		private void ApplyOrUpdateEffects(Buff buff)
		{
			this.RemoveEffects(buff);

			if (buff.Target is not Character target || buff.Caster is not ICombatEntity caster) return;

			var skillLevel = buff.NumArg1;

			if (!target.TryGetEquipItem(EquipSlot.LeftHand, out var offHandItem))
			{
				caster.TryGetActiveAbilityLevel(AbilityId.Thaumaturge8, out var abilLevel);
				var hrBonus = abilLevel * 30;
				var drBonus = abilLevel * 30;

				buff.Vars.SetFloat(VarAddType, 0);
				buff.Vars.SetFloat(VarHrBonus, hrBonus);
				buff.Vars.SetFloat(VarDrBonus, drBonus);

				target.Properties.Modify(PropertyName.HR_BM, hrBonus);
				target.Properties.Modify(PropertyName.DR_BM, drBonus);

				target.Properties.Invalidate(PropertyName.HR, PropertyName.DR);
			}
			else
			{
				var addValue = 0f;
				var addType = -1;
				var propertyToModify = "";

				if (offHandItem.Data.EquipType1 == EquipType.Shield)
				{
					addValue = 90f + (skillLevel - 1) * 20f;
					propertyToModify = PropertyName.DEF_BM;
					addType = 1;
				}
				else if (IsOffHandWeapon(offHandItem.Data.EquipType1))
				{
					addValue = 45f + (skillLevel - 1) * 10f;
					propertyToModify = PropertyName.PATK_SUB_BM;
					addType = 2;
				}

				if (addType != -1)
				{
					var casterInt = caster.Properties.GetFloat(PropertyName.INT);
					var casterMsp = caster.Properties.GetFloat(PropertyName.MSP);
					addValue += (skillLevel / 5f) * (float)Math.Pow((casterInt + casterMsp) * (addType == 1 ? 0.7 : 0.6), 0.9);

					if (caster.TryGetSkill(SkillId.Thaumaturge_SwellRightArm, out var skill) && skill.Level >= 3)
					{
						if (caster.TryGetActiveAbilityLevel(AbilityId.Thaumaturge14, out var thaum14Level))
						{
							addValue *= (1 + thaum14Level * 0.01f);
						}
					}

					addValue = (float)Math.Floor(addValue);

					buff.Vars.SetFloat(VarAddType, addType);
					buff.Vars.SetFloat(VarBonusValue, addValue);
					buff.Vars.SetString(VarItemGuid, offHandItem.ObjectId.ToString());

					target.Properties.Modify(propertyToModify, addValue);
					target.Properties.InvalidateAll();
				}
			}
		}

		private void RemoveEffects(Buff buff)
		{
			var target = buff.Target;
			var addType = buff.Vars.GetFloat(VarAddType, -1);

			switch (addType)
			{
				case 0:
					if (buff.Vars.TryGetFloat(VarHrBonus, out var hrBonus))
						target.Properties.Modify(PropertyName.HR_BM, -hrBonus);
					if (buff.Vars.TryGetFloat(VarDrBonus, out var drBonus))
						target.Properties.Modify(PropertyName.DR_BM, -drBonus);
					break;
				case 1:
					if (buff.Vars.TryGetFloat(VarBonusValue, out var defBonus))
						target.Properties.Modify(PropertyName.DEF_BM, -defBonus);
					break;
				case 2:
					if (buff.Vars.TryGetFloat(VarBonusValue, out var patkBonus))
						target.Properties.Modify(PropertyName.PATK_SUB_BM, -patkBonus);
					break;
			}

			buff.Vars.Remove(VarAddType);
			buff.Vars.Remove(VarBonusValue);
			buff.Vars.Remove(VarHrBonus);
			buff.Vars.Remove(VarDrBonus);
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
