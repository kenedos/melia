using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Handler for Enchant Aura's area, which damages up to 5 enemies in it
	/// every few seconds and drains the Enchanter's SP each time, ending the
	/// aura when the SP runs out.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Enchanter_EnchantAura)]
	public class Enchanter_EnchantAuraOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float Range = 75f;
		private const int MaxTargets = 5;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var interval = TimeSpan.FromSeconds(pad.Skill.Properties.GetFloat(PropertyName.CaptionRatio2));

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Range);
			pad.SetUpdateInterval((int)interval.TotalMilliseconds);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);

			if (pad.Skill.Vars.TryGet<Pad>(EnchanterSkillHelper.AuraPadVar, out var current) && current == pad)
			{
				pad.Skill.Vars.Remove(EnchanterSkillHelper.AuraPadVar);
				args.Creator.StopBuff(BuffId.EnchantAura_Buff);
			}
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var skill = pad.Skill;

			if (caster.IsDead || caster.Map != pad.Map || !caster.IsBuffActive(BuffId.EnchantAura_Buff))
			{
				pad.Destroy();
				return;
			}

			if (!caster.TrySpendSp(skill.Properties.GetFloat(PropertyName.CaptionRatio)))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				pad.Destroy();
				return;
			}

			var hits = new List<SkillHitInfo>();

			foreach (var target in pad.Trigger.GetAttackableEntities(caster).Take(MaxTargets))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
