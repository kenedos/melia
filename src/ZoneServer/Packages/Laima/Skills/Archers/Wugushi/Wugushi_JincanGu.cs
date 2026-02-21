using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Archers.Wugushi
{
	/// <summary>
	/// Handler for the Wugushi skill Jincan Gu (Golden Frog).
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Wugushi_JincanGu)]
	public class Wugushi_JincanGuOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int PotFlightDelayMs = 500;
		private const float ExplosionRadius = 25f;
		private const float WormLifetimeSeconds = 3f;
		private const float WormMoveSpeed = 40f;
		private const float MissileScale = 0.4f;
		private const float GroundEffectScale = 1f;
		private const float PadRadius = 10f;
		private const float ArcHeight = 0.6f;
		private const float PadDuration = 200f;
		private const float PadScale = 1f;
		private const float PadLifetime = 1000f;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_archer_jincangu_shot", "voice_archer_m_jincangu_shot");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_archer_jincangu_shot", "voice_archer_m_jincangu_shot");
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

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

			skill.Run(this.HandleSkill(caster, skill, targetPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position targetPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(PotFlightDelayMs));

			var padName = this.GetPadName(caster);

			var effectScale = GroundEffectScale;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Wugushi6, out var evasionReductionLevel))
				effectScale += evasionReductionLevel * 0.1f;

			await MissilePadThrow(skill, caster, targetPos, new MissileConfig
			{
				Effect = new EffectConfig("I_cleric_jincangu_force_mash#Dummy_effect_shoot", MissileScale),
				EndEffect = EffectConfig.None,
				DotEffect = new EffectConfig("I_force003_green", effectScale),
				Range = PadRadius,
				FlyTime = ArcHeight,
				DelayTime = 0f,
				Gravity = PadDuration,
				Speed = PadScale,
				HitTime = PadLifetime,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			}, 0f, padName);
		}

		private string GetPadName(ICombatEntity caster)
		{
			return PadName.Archer_JincanGu_Abil;
		}
	}
}
