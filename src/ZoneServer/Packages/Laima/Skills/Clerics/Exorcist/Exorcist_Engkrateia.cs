using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Exorcist;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Exorcist
{
	[Package("laima")]
	[SkillHandler(SkillId.Exorcist_Engkrateia)]
	public class Exorcist_Engkrateia : ISelfSkillHandler, IDynamicCasted, ICancelSkillHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const float MinimumDurationSeconds = 3f;
		private const float MaximumDurationSeconds = 13f;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			StopSkill(caster);
		}

		public void Handle(Skill skill, ICombatEntity caster)
		{
			StopSkill(caster);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position position, Direction direction)
		{
			if (caster is not Character character || character.IsDead)
			{
				StopSkill(caster);
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				StopSkill(character);
				return;
			}

			try
			{
				var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
				var durationSeconds = MinimumDurationSeconds + (skillLevel - MinimumSkillLevel) * (MaximumDurationSeconds - MinimumDurationSeconds) / (MaximumSkillLevel - MinimumSkillLevel);
				durationSeconds += Exorcist_EngkrateiaPatienceAbility.GetDurationBonus(character);

				skill.IncreaseOverheat();
				character.SetAttackState(true);
				character.Direction = direction;

				Send.ZC_NORMAL.UpdateSkillEffect(character, 0, character.Position, character.Direction, Position.Zero);

				character.StopBuff(BuffId.Engkrateia_Buff);
				character.StartBuff(BuffId.Engkrateia_Buff, skillLevel, 0, TimeSpan.FromSeconds(durationSeconds), character, skill.Id);
			}
			finally
			{
				StopSkill(character);
			}
		}

		private static void StopSkill(ICombatEntity caster)
		{
			if (caster is not Character character)
				return;

			character.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(character);
		}
	}
}
