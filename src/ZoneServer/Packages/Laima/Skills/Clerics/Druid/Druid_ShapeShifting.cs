using System;
using System.Linq;
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
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Druid
{
	[Package("laima")]
	[SkillHandler(SkillId.Druid_ShapeShifting)]
	public class Druid_ShapeShifting : IGroundSkillHandler, IDynamicCasted
	{
		private const float MaximumRange = 60f;
		private static readonly TimeSpan TransformationDuration = TimeSpan.FromSeconds(60);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			var monster = selectedTarget as Mob;

			if (monster == null || monster.IsDead || !character.IsEnemy(monster) || character.Position.Get2DDistance(monster.Position) > MaximumRange)
			{
				character.ServerMessage(Localization.Get("Select a valid nearby monster."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			if (!this.IsAllowedRace(monster))
			{
				character.ServerMessage(Localization.Get("Only Beast, Plant, or Insect monsters can be transformed into."));
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
			character.TurnTowards(monster.Position);
			character.SetAttackState(true);
			skill.IncreaseOverheat();

			Send.ZC_SKILL_READY(character, skill, 1, originPos, monster.Position);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, monster.Position);

			Druid_TransformationHelper.SaveTransformation(character, monster);

			var buff = character.StartBuff(BuffId.transform, (int)monster.Id, skill.Level, TransformationDuration, character, skill.Id);

			if (buff != null)
				Druid_TransformationHelper.ApplyStoredSkills(character, buff);

			character.SetAttackState(false);
		}

		private bool IsAllowedRace(Mob monster)
		{
			if (!Enum.TryParse<RaceType>(monster.Properties.GetString(PropertyName.RaceType), true, out var race))
				return false;

			return race == RaceType.Widling || race == RaceType.Forester || race == RaceType.Klaida;
		}
	}
}
