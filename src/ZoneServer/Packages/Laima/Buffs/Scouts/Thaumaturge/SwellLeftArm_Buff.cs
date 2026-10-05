using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Thaumaturge
{
	[Package("laima")]
	[BuffHandler(BuffId.SwellLeftArm_Buff)]
	public class SwellLeftArm_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			if (target is not Character character || !character.TryGetEquipItem(EquipSlot.RightHand, out _))
			{
				OnEnd(buff);
				return;
			}

			if (buff.Caster is not ICombatEntity caster) return;

			var skillLevel = buff.NumArg1;
			if (buff.Vars.TryGetFloat("Melia.Pad.SkillLevel", out var padLevel))
			{
				skillLevel = padLevel;
			}

			const float defaultAdd = 70f;
			var casterInt = caster.Properties.GetFloat(PropertyName.INT);
			var casterMsp = caster.Properties.GetFloat(PropertyName.MSP);

			var addValue = defaultAdd + ((skillLevel - 1) * 12) + (skillLevel / 5f) * (float)Math.Pow((casterInt + casterMsp) * 0.6, 0.9);

			if (caster.TryGetSkill(SkillId.Thaumaturge_SwellLeftArm, out var swellLeftArmSkill) && swellLeftArmSkill.Level >= 3)
			{
				if (caster.TryGetActiveAbilityLevel(AbilityId.Thaumaturge11, out var thaum11Level))
				{
					addValue *= (1 + thaum11Level * 0.01f);
				}
			}

			var thaum6Level = 0;
			var thaum7Level = 0;
			if (caster.TryGetSkill(SkillId.Thaumaturge_ShrinkBody, out var shrinkSkill) && shrinkSkill.Level >= 2)
			{
				caster.TryGetActiveAbilityLevel(AbilityId.Thaumaturge6, out thaum6Level);
			}
			if (caster.TryGetSkill(SkillId.Thaumaturge_SwellBody, out var swellBodySkill) && swellBodySkill.Level >= 2)
			{
				caster.TryGetActiveAbilityLevel(AbilityId.Thaumaturge7, out thaum7Level);
			}
			buff.NumArg3 = thaum6Level;
			buff.NumArg4 = thaum7Level;

			var patkBonus = (float)Math.Floor(addValue);
			var matkBonus = (float)Math.Floor(addValue);

			AddPropertyModifier(buff, target, PropertyName.PATK_MAIN_BM, patkBonus);
			AddPropertyModifier(buff, target, PropertyName.MATK_BM, matkBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.PATK_MAIN_BM);
			RemovePropertyModifier(buff, target, PropertyName.MATK_BM);
		}
	}
}
