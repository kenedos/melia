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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler for the Bokor skill Tetmamakla.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bokor_Tetmamakla)]
	public class Bokor_TetmamaklaOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
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
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			await MissileThrow(skill, caster, targetPos, new MissileConfig
			{
				Effect = new EffectConfig("I_cleric_tetmamakla_mash#Bip01 Head", 0.6f),
				EndEffect = new EffectConfig("F_explosion092_hit", 1f),
				Range = 0f,
				FlyTime = 0.6f,
				DelayTime = 0f,
				Gravity = 100f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var spawnPos = GetRelativePosition(PosType.TargetFront, caster, caster, height: 5);
			MonsterSkillCreateMob(skill, caster, "Tetmamakla", spawnPos, 0f, "", "", 0, 20f, "tetmamakla_AI", "");
		}
	}
}
