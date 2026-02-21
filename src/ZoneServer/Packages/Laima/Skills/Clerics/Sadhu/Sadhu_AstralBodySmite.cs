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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu skill Astral Body Smite.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_AstralBodySmite)]
	public class Sadhu_AstralBodySmiteOverride : IMeleeGroundSkillHandler
	{
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

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos, targets));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos, ICombatEntity[] targets)
		{
			var targetPos = originPos.GetRelative(farPos, distance: 50f);
			var value = skill.GetPVPValue(5);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 60f, value));
			caster.StartBuff(BuffId.Sadhu_Soul_Pre_Buff, 1f, 0f, TimeSpan.Zero, caster);
			caster.StartBuff(BuffId.Sadhu_Soul_Buff, 1f, 0f, TimeSpan.FromMilliseconds(60000f), caster);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			SkillTargetDamage(skill, caster, targets, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			targetPos = originPos.GetRelative(farPos, distance: 50f);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Sadhu_AstralBodySmite);
			SkillTargetEffects(skill, caster, targets, "F_wizard_compulsionlink_shot_explosion_blue", 0.8f, false);
		}
	}
}
