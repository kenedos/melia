using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Wizards.Onmyoji
{
	/// <summary>
	/// Handler for the Onmyoji skill Greenwood Shikigami, which grows a tree
	/// at the target location that strikes and pulls in the enemies around
	/// it, and slows them when it finishes growing.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Onmyoji_GreenwoodShikigami)]
	public class Onmyoji_GreenwoodShikigamiOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float Range = 100f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

			if (!caster.InSkillUseRange(skill, targetPos))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, ForceId.GetNew(), null);

			var effectPad = new Pad(PadName.GreenwoodShikigami_Pad_Effect, caster, skill, new Circle(targetPos, Range));
			effectPad.Position = targetPos;
			caster.Map.AddPad(effectPad);

			var pad = new Pad(PadName.GreenwoodShikigami_Pad, caster, skill, new Circle(targetPos, Range));
			pad.Position = targetPos;
			caster.Map.AddPad(pad);
		}
	}
}
