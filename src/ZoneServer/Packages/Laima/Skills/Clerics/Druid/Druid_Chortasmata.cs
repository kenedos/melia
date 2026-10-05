using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Druid
{
	[Package("laima")]
	[SkillHandler(SkillId.Druid_Chortasmata)]
	public class Druid_Chortasmata : IGroundSkillHandler, IDynamicCasted
	{
		private const string PadName = "GroundAura_GrowingGrass_Green_01";

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_cleric_chortasmata_shot", "voice_cleric_m_chortasmata_shot");
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_cleric_chortasmata_shot", "voice_cleric_m_chortasmata_shot");
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead)
				return;

			var targetPos = farPos;

			if (skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var toolGroundPos))
				targetPos = toolGroundPos;

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			character.TurnTowards(targetPos);
			character.SetAttackState(true);
			skill.IncreaseOverheat();

			Send.ZC_SKILL_READY(character, skill, 1, originPos, targetPos);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, targetPos);

			skill.Run(MissilePadThrow(skill, character, targetPos, new MissileConfig
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
			}, 0f, PadName));

			character.SetAttackState(false);
		}
	}
}
