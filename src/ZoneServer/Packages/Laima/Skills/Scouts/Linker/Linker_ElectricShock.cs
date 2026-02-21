using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Effects;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Linker
{
	/// <summary>
	/// Handler for the Linker skill Electric Shock.
	/// Deals damage to a target and creates a lightning link from the caster.
	/// Multiple targets can be linked simultaneously, each with their own
	/// independent duration and visual effect.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Linker_ElectricShock)]
	public class Linker_ElectricShockOverride : IForceSkillHandler
	{
		private const float LinkDurationSeconds = 12f;
		private const string LinkTexture = "Linker_cable_blue";
		private const string LinkEffect = "F_archer_bodkinpoint_finish";
		private const float LinkEffectScale = 1.0f;
		private const string LinkSound = "swd_blow_cloth2";
		private const float LinkSecond = 0.3f;

		/// <summary>
		/// Handles the Electric Shock skill execution.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult);
			skillHit.ForceId = ForceId.GetNew();
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);

			// Don't create link if initial hit dealt no damage
			if (skillHitResult.Damage <= 0)
				return;

			skill.Run(this.CreateLink(caster, target, skill));
		}

		/// <summary>
		/// Creates an independent lightning link from the caster to the target.
		/// Each link has its own linkId and visual effect that is cleaned up
		/// when that specific buff ends. If the target already has the debuff,
		/// the DoT damage will stack via the overbuff system.
		/// </summary>
		private async Task CreateLink(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(750));

			if (target.IsDead || caster.IsDead)
				return;

			var duration = TimeSpan.FromSeconds(LinkDurationSeconds);
			var alreadyHasDebuff = target.IsBuffActive(BuffId.ElectricShock_Debuff);

			// StartBuff handles stacking automatically via OnActivate/OnExtend
			var buff = target.StartBuff(BuffId.ElectricShock_Debuff, skill.Level, 0, duration, caster, skill.Id);
			if (buff == null)
				return;

			// Only create visual effects for first application
			if (!alreadyHasDebuff)
			{
				var linkId = ZoneServer.Instance.World.CreateLinkHandle();
				var effectName = $"ElectricShock_Link_{linkId}";

				buff.Vars.Set("Melia.Link.Id", linkId);
				buff.Vars.Set("Melia.Link.Caster", caster.Handle);
				buff.Vars.Set("Melia.Link.EffectName", effectName);

				target.AddEffect("Melia.Link.Chain", new AttachEffect("I_chain004_mash_loop_multi", 2, EffectLocation.Bottom));

				var linkedHandles = new List<int> { caster.Handle, target.Handle };
				var linkerEffect = new LinkerVisualEffect(linkId, LinkTexture, true, linkedHandles, LinkSecond, LinkEffect, LinkEffectScale, LinkSound);
				caster.AddEffect(effectName, linkerEffect);
			}
		}
	}
}
