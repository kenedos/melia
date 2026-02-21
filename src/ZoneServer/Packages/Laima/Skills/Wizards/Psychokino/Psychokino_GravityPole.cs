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
	public class Psychokino_GravityPoleOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.ClearTargets();
			caster.SetCastingState(true, skill);

			if (caster.TryGetActiveAbilityLevel(AbilityId.Psychokino3, out var abilityLevel))
			{
				var evasionBonus = abilityLevel * 10f;
				caster.Properties.Modify(PropertyName.DR_BM, evasionBonus);
				skill.Vars.Set("Psychokino3_EvasionBonus", evasionBonus);
			}

			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (skill.Vars.TryGet<float>("Psychokino3_EvasionBonus", out var evasionBonus))
			{
				caster.Properties.Modify(PropertyName.DR_BM, -evasionBonus);
				skill.Vars.Remove("Psychokino3_EvasionBonus");
			}

			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
			caster.RemoveBuff(BuffId.Wizard_SklCasting_Avoid);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
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

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(550));

			var padLength = 110f;
			var padWidth = 60f;
			var direction = caster.Direction;
			var padPosition = caster.Position;

			var pad = new Pad(PadName.GravityPole_PVP, caster, skill, new Square(padPosition, direction, padLength, padWidth));
			pad.Position = padPosition;
			pad.Direction = direction;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);

			caster.Map.AddPad(pad);
		}
	}
}
