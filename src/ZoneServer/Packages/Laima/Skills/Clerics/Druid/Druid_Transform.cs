using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Druid
{
	[Package("laima")]
	[SkillHandler(SkillId.Druid_Transform)]
	public class Druid_Transform : IGroundSkillHandler, IDynamicCasted
	{
		private static readonly TimeSpan TransformationDuration = TimeSpan.FromSeconds(60);

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

			var monsterId = Druid_TransformationHelper.GetMonsterId(character);

			if (monsterId <= 0)
			{
				character.ServerMessage(Localization.Get("No previous transformation was found."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			character.StopBuff(BuffId.transform);
			character.SetAttackState(true);
			skill.IncreaseOverheat();

			Send.ZC_SKILL_READY(character, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, character.Position);

			var buff = character.StartBuff(BuffId.transform, monsterId, skill.Level, TransformationDuration, character, skill.Id);

			if (buff != null)
				Druid_TransformationHelper.ApplyStoredSkills(character, buff);

			character.SetAttackState(false);
		}
	}
}
