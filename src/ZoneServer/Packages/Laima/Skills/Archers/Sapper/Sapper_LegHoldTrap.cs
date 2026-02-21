using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Archers.Sapper
{
	/// <summary>
	/// Handler for the Sapper skill Leg Hold Trap.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sapper_LegHoldTrap)]
	public class Sapper_LegHoldTrapOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int CastDelayMs = 200;
		private const float SpawnDistance = 22.4f;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(CastDelayMs));

			var targetPos = originPos.GetRelative(caster.Direction, distance: SpawnDistance);
			await MissilePadThrow(skill, caster, targetPos, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_shot_LegholdTrap_mash#Bip01 R Hand", 0.3f),
				EndEffect = new EffectConfig("F_smoke008##1", 0.7f),
				DotEffect = EffectConfig.None,
				Range = 0f,
				FlyTime = 0.4f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			}, 0f, PadName.Sapper_LegHoldTrap_Mine);
		}
	}
}
