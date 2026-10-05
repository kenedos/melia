using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Druid;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Druid
{
	[Package("laima")]
	[SkillHandler(SkillId.Druid_Lycanthropy)]
	public class Druid_Lycanthropy : IGroundSkillHandler, IDynamicCasted
	{
		private const int WolfDurationSeconds = 30;
		private const int HumanDurationSeconds = 60;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			character.StopBuff(BuffId.Lycanthropy_Buff);
			character.StopBuff(BuffId.Lycanthropy_Half_Buff);

			var humanForm = Druid_LycanthropyHumanFormAbility.IsActive(character);
			var durationSeconds = humanForm ? HumanDurationSeconds : WolfDurationSeconds;

			if (humanForm && Druid_WolfSpiritAbility.IsActive(character))
				durationSeconds += skill.Level * 2;

			var buffId = humanForm ? BuffId.Lycanthropy_Half_Buff : BuffId.Lycanthropy_Buff;

			character.SetAttackState(true);
			skill.IncreaseOverheat();

			Send.ZC_SKILL_READY(character, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, character.Position);

			character.StartBuff(buffId, skill.Level, 0, TimeSpan.FromSeconds(durationSeconds), character, skill.Id);
			character.SetAttackState(false);
		}
	}

	public static class Druid_LycanthropyRestriction
	{
		public static bool CanUseSkill(Character character, SkillId skillId)
		{
			if (character == null || !character.IsBuffActive(BuffId.Lycanthropy_Buff))
				return true;

			return skillId == SkillId.Druid_Lycanthropy
				|| skillId == SkillId.Mon_pcskill_boss_werewolf_Skill_1
				|| skillId == SkillId.Mon_pcskill_boss_werewolf_Skill_3
				|| skillId == SkillId.Mon_pcskill_boss_werewolf_Skill_4
				|| skillId == SkillId.Mon_pcskill_boss_werewolf_Skill_5;
		}
	}
}
