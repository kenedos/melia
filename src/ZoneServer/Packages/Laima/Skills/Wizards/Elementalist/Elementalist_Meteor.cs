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
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Elementalist
{
	/// <summary>
	/// Handler override for the Elementalist skill Meteor.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Elementalist_Meteor)]
	public class Elementalist_MeteorOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			caster.ClearTargets();
			caster.PlaySound("voice_wiz_meteor_cast", "voice_wiz_m_meteor_cast");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			caster.RemoveBuff(BuffId.Wizard_SklCasting_Avoid);
			caster.StopSound("voice_wiz_meteor_cast");
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

			var target = targets.FirstOrDefault();
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);

			skill.Run(this.HandleSkill(caster, skill, targetPos));

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position targetPos)
		{
			var range = 140;
			caster.MissileFall(skill.Data.ClassName, "I_wizard_meteorfall_force_dropfire", 6f, targetPos, range, 0f, 0.55f, 300f, 1f, "F_sys_target_pc2", 4f, 0f, "F_wizard_meteorfall_shot_explosion", 4f);

			var hitDelay = TimeSpan.FromMilliseconds(550);
			await skill.Wait(hitDelay);

			var area = new CircleF(targetPos, range);

			var enemies = caster.Map.GetAttackableEnemiesIn(caster, area);
			var damageDelay = TimeSpan.FromMilliseconds(50);
			var skillHits = new List<SkillHitInfo>();

			var maxTargets = 50;
			foreach (var enemy in enemies.Take(maxTargets))
			{
				var skillHitResult = SCR_SkillHit(caster, enemy, skill, SkillModifier.MultiHit(2));
				enemy.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, enemy, skill, skillHitResult, damageDelay, TimeSpan.Zero);
				skillHits.Add(skillHit);

				if (skillHitResult.Damage > 0)
				{
					var burnDamage = Math.Max(1, (int)(skillHitResult.Damage * 0.05f));
					enemy.StartBuff(BuffId.Fire, skill.Level, burnDamage, TimeSpan.FromSeconds(5), caster);
				}
			}

			Send.ZC_SKILL_HIT_INFO(caster, skillHits);
		}
	}
}

