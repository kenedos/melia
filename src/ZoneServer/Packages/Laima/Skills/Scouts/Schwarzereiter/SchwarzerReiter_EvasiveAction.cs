using System.Collections.Generic;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Schwarzereiter skill Evasive Action.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_EvasiveAction)]
	public class SchwarzerReiter_EvasiveActionOverride : ISelfSkillHandler
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

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			caster.StartBuff(BuffId.EvasiveAction_Buff, skill.Level, 0f, skill.Properties.CaptionTime, caster, skill.Id);

			this.RemoveDebuffs(caster);
		}

		/// <summary>
		/// Removes the removable debuffs from the caster.
		/// </summary>
		/// <param name="caster"></param>
		private void RemoveDebuffs(ICombatEntity caster)
		{
			var debuffsToRemove = new List<BuffId>();

			foreach (var buff in caster.Components.Get<BuffComponent>()
				.GetAll(a => a.Data.Type == BuffType.Debuff && a.Data.Removable))
			{
				debuffsToRemove.Add(buff.Id);
			}

			foreach (var buffId in debuffsToRemove)
				caster.RemoveBuff(buffId);
		}
	}
}
