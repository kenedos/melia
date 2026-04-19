using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Schwarzereiter skill Concentrated Fire.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_ConcentratedFire)]
	public class SchwarzerReiter_ConcentratedFireOverride : IGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_archer_multishot_cast", "voice_archer_m_multishot_cast");
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_archer_multishot_cast", "voice_archer_m_multishot_cast");
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
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

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, targetPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position targetPos)
		{
			var config = new MissileConfig
			{
				Effect = new EffectConfig { Name = "None", Scale = 0.6f },
				EndEffect = new EffectConfig { Name = "None", Scale = 0.6f },
				DotEffect = new EffectConfig { Name = "None", Scale = 1f },
				GroundEffect = new EffectConfig { Name = "None", Scale = 1f },
				Range = 40f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 30f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
			};
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			await MissileThrow(skill, caster, targetPos, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			for (var i = 0; i < 10 - 1; i++)
			{
				await MissileThrow(skill, caster, targetPos, config);
				await skill.Wait(TimeSpan.FromMilliseconds(170));
			}
		}
	}
}
