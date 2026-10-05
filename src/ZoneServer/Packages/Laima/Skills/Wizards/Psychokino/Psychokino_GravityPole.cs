using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.HandlersOverrides.Wizards.Psychokino
{
	/// <summary>
	/// Handler for the Psychokino skill Gravity Pole.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Psychokino_GravityPole)]
	public class Psychokino_GravityPoleOverride : IGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (caster == null || caster.IsDead)
				return;

			caster.ClearTargets();
			caster.StartBuff(BuffId.Wizard_SklCasting_Avoid, skill.Level, 0f, TimeSpan.FromSeconds(5.55), caster, skill.Id);
			if (skill.Vars.TryGet<float>("Psychokino3_EvasionBonus", out var previousBonus))
			{
				caster.Properties.Modify(PropertyName.DR_BM, -previousBonus);
				skill.Vars.Remove("Psychokino3_EvasionBonus");
			}
			if (caster.TryGetActiveAbilityLevel(AbilityId.Psychokino3, out var abilityLevel))
			{
				var evasionBonus = abilityLevel * 10f;
				caster.Properties.Modify(PropertyName.DR_BM, evasionBonus);
				skill.Vars.Set("Psychokino3_EvasionBonus", evasionBonus);
			}
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (skill.Vars.TryGet<float>("Psychokino3_EvasionBonus", out var evasionBonus))
			{
				caster.Properties.Modify(PropertyName.DR_BM, -evasionBonus);
				skill.Vars.Remove("Psychokino3_EvasionBonus");
			}
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
			if (caster.TryGetBuff(BuffId.Wizard_SklCasting_Avoid, out var protection)
				&& protection.SkillId == skill.Id && protection.Caster == caster)
				caster.RemoveBuff(BuffId.Wizard_SklCasting_Avoid);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster == null)
				return;
			if (caster.IsDead || caster.Map == null)
			{
				this.EndDynamicCast(skill, caster, 0f);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				this.EndDynamicCast(skill, caster, 0f);
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.TurnTowards(farPos);
			caster.StartBuff(BuffId.Wizard_SklCasting_Avoid, skill.Level, 0f, TimeSpan.FromSeconds(5.55), caster, skill.Id);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var castMap = caster.Map;
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			if (caster.IsDead || castMap == null || caster.Map != castMap || !caster.IsCasting(skill))
			{
				this.EndDynamicCast(skill, caster, 0f);
				return;
			}

			var padLength = 110f;
			var padWidth = 60f;
			var direction = caster.Direction;
			var padPosition = caster.Position;

			var pad = new Pad(PadName.GravityPole_PVP, caster, skill, new Square(padPosition, direction, padLength, padWidth));
			pad.Position = padPosition;
			pad.Direction = direction;
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(10);

			caster.Map.AddPad(pad);
		}
	}
}
