using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Yggdrasil.Extensions;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Hunter
{
	/// <summary>
	/// Handler for the Hunter skill Bolas.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Hunter_Bolas)]
	public class Hunter_BolasOverride : IMeleeGroundSkillHandler, IDynamicCasted
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

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(skill, caster, targetPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position targetPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(450));
			await MissilePadThrow(skill, caster, targetPos, new MissileConfig
			{
				Effect = new EffectConfig("I_bolas_mesh#Bip01 R Hand", 1.125f),
				EndEffect = new EffectConfig("F_archer_explosiontrap_shot_smoke", 0.5f),
				DotEffect = EffectConfig.None,
				Range = 0f,
				FlyTime = 0.3f,
				DelayTime = 0f,
				Gravity = -10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			}, 0f, PadName.Shootpad_Bolas);
			
			if (caster.TryGetActiveCompanion(out var companion))
			{
				var enemies = caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, 40f);

				foreach (var enemy in enemies)
				{
					companion.InsertHate(enemy);
				}
			}
		}
	}
}
