using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Hunter
{
	/// <summary>
	/// Handler for the Hunter skill Praise.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Hunter_Praise)]
	public class Hunter_PraiseOverride : IMeleeGroundSkillHandler
	{
		private const int PraiseAtkBuffDurationSeconds = 10;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TryGetActiveCompanion(out var companion))
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

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(skill, caster, companion));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, ICombatEntity companion)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(300));

			// Apply Praise_Atk_Buff to companion (attack buff + bleeding on attack, 10 seconds)
			companion.StartBuff(BuffId.Praise_Atk_Buff, skill.Level, 0f, TimeSpan.FromSeconds(PraiseAtkBuffDurationSeconds), caster);

			// Hunter26 ability: grant temporary debuff immunity to companion
			if (caster.IsAbilityActive(AbilityId.Hunter26))
				companion.StartBuff(BuffId.Cure_Buff, 1, 0f, TimeSpan.FromSeconds(PraiseAtkBuffDurationSeconds), caster);
		}
	}
}
