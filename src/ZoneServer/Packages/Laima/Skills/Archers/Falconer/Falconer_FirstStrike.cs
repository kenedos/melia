using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Falconer skill First Strike.
	/// Toggle skill that makes the hawk automatically attack enemies
	/// when the owner is in combat. The hawk will use Blistering Thrash
	/// on enemies the owner attacks, and Hovering on enemies approaching
	/// the owner.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_FirstStrike)]
	public class Falconer_FirstStrikeOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!FalconerHawkHelper.TryGetHawk(caster, out var hawk))
			{
				if (caster is Character character)
					character.SystemMessage("CompanionIsNotActive");
				Send.ZC_SKILL_DISABLE(caster);
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

			// Toggle behavior
			var currentToggle = skill.Vars.GetBool("Melia.Skill.Toggled");
			skill.Vars.SetBool("Melia.Skill.Toggled", !currentToggle);
			var isToggled = !currentToggle;
			if (caster is Character toggleChar)
				Send.ZC_NORMAL.SkillToggle(toggleChar, isToggled ? skill.Id : SkillId.None);

			if (isToggled)
			{
				// Apply First Strike buff - enables auto-attack behavior
				// Buff duration is infinite (toggle), but consumes SP periodically
				caster.StartBuff(BuffId.FirstStrike_Buff, skill.Level, 0f, TimeSpan.Zero, caster);

				// Visual activation effect
				caster.PlayEffect("skl_eff_whistle_4", scale: 1f, heightOffset: EffectLocation.Top);

				// Start hawk auto-attack script
				hawk.Vars.Set("Hawk.FirstStrike.Active", true);
			}
			else
			{
				// Remove buff
				caster.StopBuff(BuffId.FirstStrike_Buff);

				// Deactivation effect
				caster.PlayEffect("skl_eff_whistle_4", scale: 0.8f, heightOffset: EffectLocation.Top);

				// Stop hawk auto-attack script
				hawk.Vars.Set("Hawk.FirstStrike.Active", false);
			}
		}

	}
}
