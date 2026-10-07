using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Handler for the Enchanter skill Over-Reinforce, which raises the
	/// attack of the party members around the Enchanter by a share of the
	/// Enchanter's own attack.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Enchanter_OverReinforce)]
	public class Enchanter_OverReinforceOverride : IGroundSkillHandler
	{
		private const float Range = 150f;
		private static readonly TimeSpan BuffDelay = TimeSpan.FromMilliseconds(300);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(skill, caster));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(BuffDelay);

			var atkBonus = skill.Properties.GetFloat(PropertyName.CaptionRatio);

			foreach (var ally in EnchanterSkillHelper.GetPartyTargets(caster, Range))
				ally.StartBuff(BuffId.OverReinforce_Buff, skill.Level, atkBonus, skill.Properties.CaptionTime, caster, skill.Id);
		}
	}
}
