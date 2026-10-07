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
	/// Handler for the Enchanter skill Agility, which lowers the stamina
	/// consumption and raises the movement speed of the party members
	/// around the Enchanter who wear boots.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Enchanter_Agility)]
	public class Enchanter_AgilityOverride : IGroundSkillHandler
	{
		private const float Range = 160f;
		private static readonly TimeSpan BuffDelay = TimeSpan.FromMilliseconds(800);

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

			foreach (var ally in EnchanterSkillHelper.GetPartyTargets(caster, Range, EquipSlot.Shoes))
				ally.StartBuff(BuffId.Agility_Buff, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);
		}
	}
}
