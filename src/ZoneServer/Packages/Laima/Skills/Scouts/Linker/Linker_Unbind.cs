using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Linker
{
	/// <summary>
	/// Handler for the Linker skill Unbind.
	/// Removes all links created by the caster.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Linker_Unbind)]
	public class Linker_UnbindOverride : IMeleeGroundSkillHandler
	{
		private const int CastDelayMs = 600;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(CastDelayMs));

			// Only cancel party member related links
			LinkerSkillHelper.LinkDestruct(caster, BuffId.Link_Physical);  // Physical Link
			LinkerSkillHelper.LinkDestruct(caster, BuffId.Link_Party);     // Lifeline
			LinkerSkillHelper.LinkDestruct(caster, BuffId.Link);           // Spiritual Chain
		}
	}
}
