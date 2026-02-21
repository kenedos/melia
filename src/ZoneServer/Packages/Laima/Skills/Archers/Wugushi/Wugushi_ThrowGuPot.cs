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

namespace Melia.Zone.Skills.Handlers.Archers.Wugushi
{
	/// <summary>
	/// Handler for the Wugushi skill Throw Gu Pot (Poison Pot).
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Wugushi_ThrowGuPot)]
	public class Wugushi_ThrowGuPotOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int ThrowDelayMs = 200;
		private const float MissileScale = 0.5f;
		private const float PadRadius = 10f;
		private const float ArcHeight = 0.6f;
		private const float PadDuration = 500f;
		private const float PadScale = 1f;
		private const float PadLifetime = 1000f;
		private const string DefaultPadName = "Archer_VerminPot";

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_archer_throwgupot_shot", "voice_archer_m_throwgupot_shot");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_archer_throwgupot_shot", "voice_archer_m_throwgupot_shot");
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
			await skill.Wait(TimeSpan.FromMilliseconds(ThrowDelayMs));

			var padName = this.GetPadName(caster);

			await MissilePadThrow(skill, caster, targetPos, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_poison_pot_force#Bip01 R Hand", MissileScale),
				EndEffect = EffectConfig.None,
				DotEffect = EffectConfig.None,
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
			var bossCardId = caster.GetTempVar("Wugushi_bosscard");
			if (bossCardId > 0)
				return "Archer_VerminPot_Boss";

			if (caster.IsAbilityActive(AbilityId.Wugushi5))
				return "Archer_VerminPot_Enhanced";

			return DefaultPadName;
		}
	}
}
