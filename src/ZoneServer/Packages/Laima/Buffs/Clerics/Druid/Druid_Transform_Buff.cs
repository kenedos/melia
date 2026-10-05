using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Druid;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Druid
{
	[Package("laima")]
	[BuffHandler(BuffId.transform)]
	public class Druid_Transform_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			if (!Druid_TransformationSpecialityAbility.IsActive(character))
				return;

			var criticalRate = character.Properties.GetFloat(PropertyName.CRTHR);
			var evasion = character.Properties.GetFloat(PropertyName.DR);
			var hpRecovery = character.Properties.GetFloat(PropertyName.RHP);

			AddPropertyModifier(buff, character, PropertyName.CRTHR_BM, criticalRate * 0.10f);
			AddPropertyModifier(buff, character, PropertyName.DR_BM, evasion * 0.10f);
			AddPropertyModifier(buff, character, PropertyName.RHP_BM, hpRecovery * 0.10f);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			var skills = character.Skills.GetList();

			foreach (var skill in skills)
			{
				var variableName = Melia.Zone.Packages.Laima.Skills.Clerics.Druid.Druid_TransformationHelper.AddedSkillVariable(skill.Id);

				if (buff.Vars.GetInt(variableName) == 0)
					continue;

				character.Skills.Remove(skill.Id);

				if (character.Skills.Has(skill.Id))
				{
					character.Skills.RemoveSilent(skill.Id);
					Send.ZC_SKILL_REMOVE(character, skill.Id);
				}
			}

			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.None);
			RemovePropertyModifier(buff, character, PropertyName.CRTHR_BM);
			RemovePropertyModifier(buff, character, PropertyName.DR_BM);
			RemovePropertyModifier(buff, character, PropertyName.RHP_BM);
		}
	}
}
