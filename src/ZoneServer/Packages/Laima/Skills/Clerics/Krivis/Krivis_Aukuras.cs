using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Kriwi
{
	/// <summary>
	/// Handler for the Kriwi skill Aukuras.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Kriwi_Aukuras)]
	public class Krivis_AukurasOverride : IGroundSkillHandler, IDynamicCasted
	{

		/// <summary>
		/// Handles skill
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill));
		}
		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			SkillRemovePad(caster, skill);
			await skill.Wait(TimeSpan.FromMilliseconds(300));

			if (caster.TryGetActiveAbilityLevel(AbilityId.Kriwi14, out var abilityLevel))
				SkillCreatePad(caster, skill, caster.Position, abilityLevel, PadName.Cleric_New_Aukuras);
			else
				SkillCreatePad(caster, skill, caster.Position, 0, PadName.Cleric_New_Aukuras);
		}
	}
}
