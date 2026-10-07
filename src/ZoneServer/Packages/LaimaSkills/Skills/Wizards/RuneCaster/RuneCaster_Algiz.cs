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

namespace Melia.Zone.Skills.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for the Rune Caster skill Rune of Protection, which lowers the
	/// damage the Rune Caster takes while casting.
	/// </summary>
	/// <remarks>
	/// With Rune of Protection: Giant, it turns the Rune Caster or the
	/// selected party member into a giant instead.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.RuneCaster_Algiz)]
	public class RuneCaster_AlgizOverride : IGroundSkillHandler
	{
		private static readonly TimeSpan GiantDurationPerLevel = TimeSpan.FromSeconds(60);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			RuneCasterSkillHelper.ApplySkilledCasting(caster);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			if (!caster.IsAbilityActive(AbilityId.RuneCaster22))
			{
				caster.StartBuff(BuffId.Algiz_Buff, skill.Level, 0, TimeSpan.FromMinutes(skill.Properties.GetFloat(PropertyName.CaptionRatio2)), caster, skill.Id);
				return;
			}

			var giant = target is Character && !target.IsDead && (target == caster || target.IsAlly(caster)) ? target : caster;
			giant.StartBuff(BuffId.Thurisaz_Buff, skill.Level, 0, GiantDurationPerLevel * skill.Level, caster, skill.Id);
		}
	}
}
