using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Archers.Fletcher
{
	/// <summary>
	/// Handler for the Fletcher skill Magic Arrow.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fletcher_MagicArrow)]
	public class Fletcher_MagicArrowOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
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

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(MissilePadThrow(skill, caster, targetPos, new MissileConfig
			{
				Effect = new EffectConfig("I_arrow009_1#Dummy_Force", 0.5f),
				EndEffect = new EffectConfig("F_explosion092_hit_mint", 1f),
				DotEffect = EffectConfig.None,
				Range = 40f,
				FlyTime = 0.2f,
				DelayTime = 0f,
				Gravity = 30f,
				Speed = 1f,
				HitTime = 200f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			}, 0f, "Fletcher_MagicArrow"));
		}
	}
}
