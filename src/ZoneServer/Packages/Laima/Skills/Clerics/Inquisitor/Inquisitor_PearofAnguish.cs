using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Inquisitor
{
	[Package("laima")]
	[SkillHandler(SkillId.Inquisitor_PearofAnguish)]
	public class Inquisitor_PearofAnguish : IGroundSkillHandler, IDynamicCasted, ICancelSkillHandler
	{
		private const int InstallationCount = 5;
		private const int InstallationSpread = 25;
		private const string PearPadName = "F_smoke008_pearofanguish##1";
		private static readonly TimeSpan DelayBetweenInstallations = TimeSpan.FromMilliseconds(50);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (caster == null)
				return;

			caster.PlaySound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			StopCastSound(caster);
		}

		public void Handle(Skill skill, ICombatEntity caster)
		{
			StopSkill(caster, skill);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead)
			{
				StopSkill(caster, skill);
				return;
			}

			var targetPosition = farPos;

			if (skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var toolGroundPosition))
				targetPosition = toolGroundPosition;

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				StopSkill(character, skill);
				return;
			}

			try
			{
				var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

				character.TurnTowards(targetPosition);
				character.SetAttackState(true);
				skill.IncreaseOverheat();

				Send.ZC_SKILL_READY(character, skill, skillHandle, originPos, targetPosition);
				Send.ZC_SKILL_MELEE_GROUND(character, skill, targetPosition);

				skill.Run(this.InstallPears(skill, character, targetPosition));
			}
			catch
			{
				StopSkill(character, skill);
				throw;
			}
		}

		private async Task InstallPears(Skill skill, Character caster, Position centerPosition)
		{
			try
			{
				for (var installationIndex = 0; installationIndex < InstallationCount; installationIndex++)
				{
					if (caster.IsDead || caster.Map == null)
						break;

					var position = centerPosition.GetRandomInRange2D(0, InstallationSpread);

					await MissilePadThrow(skill, caster, position, new MissileConfig
					{
						Effect = EffectConfig.None,
						EndEffect = EffectConfig.None,
						DotEffect = EffectConfig.None,
						GroundEffect = EffectConfig.None,
						Range = 100f,
						FlyTime = 0.1f,
						DelayTime = 0f,
						Gravity = 0f,
						Speed = 1f,
						HitTime = 0f,
						HitCount = 0,
						GroundDelay = 0f,
						EffectMoveDelay = 0f,
					}, 0f, PearPadName);

					if (installationIndex + 1 < InstallationCount)
						await skill.Wait(DelayBetweenInstallations);
				}
			}
			finally
			{
				StopSkill(caster, skill);
			}
		}

		private static void StopSkill(ICombatEntity caster, Skill skill)
		{
			if (caster == null)
				return;

			StopCastSound(caster);
			caster.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(caster);

			if (skill == null)
				return;

			Send.ZC_NORMAL.Skill_45(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
		}

		private static void StopCastSound(ICombatEntity caster)
		{
			if (caster == null)
				return;

			caster.StopSound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
		}
	}
}
