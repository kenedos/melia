using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Rogue
{
	/// <summary>
	/// Handler for the Rogue skill Burrow.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Rogue_Burrow)]
	public class Rogue_BurrowOverride : IMeleeGroundSkillHandler
	{
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
			var buffActive = caster.IsBuffActive(BuffId.Burrow_Rogue);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, buffActive ? 1 : 0, targetHandle, caster.Position, caster.Direction, Position.Zero);

			skill.Run(this.HandleSkill(skill, caster));

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster)
		{
			if (caster.IsBuffActive(BuffId.Burrow_Rogue))
			{
				caster.StopBuff(BuffId.Burrow_Rogue);
			}
			else
			{
				SkillResetCooldown(skill, caster);
				await skill.Wait(TimeSpan.FromMilliseconds(600));
				caster.StartBuff(BuffId.Burrow_Rogue, skill.Level, 0, TimeSpan.FromSeconds(60), caster);
			}
		}
	}
}
