using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for the Exorcist skill Engkrateia, a prayer that reduces
	/// the damage the Exorcist takes and makes them immune to knockback.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Exorcist_Engkrateia)]
	public class Exorcist_EngkrateiaOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, ForceId.GetNew(), null);

			var duration = TimeSpan.FromSeconds(skill.Properties.GetFloat(PropertyName.CaptionRatio2));
			caster.StartBuff(BuffId.Engkrateia_Buff, skill.Level, 0, duration, caster, skill.Id);
		}
	}
}
