using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Squire
{
	/// <summary>
	/// Handler for the Squire skill Arrest.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Squire_Arrest)]
	public class Squire_ArrestOverride : IGroundSkillHandler, IDynamicCasted
	{

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			// TODO: No Implementation SKL_CANCEL_CANCEL

		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			var skillTargets = SkillSelectEnemiesInSquare(caster, targetPos, 0f, 120f, 30f, 1 + skill.Level * 1);
			if (skillTargets == null || skillTargets.Count == 0)
				return;
			await skill.Wait(TimeSpan.FromMilliseconds(550));
			SkillTargetBuff(skill, caster, skillTargets, BuffId.Arrest, skill.Level, 0f, TimeSpan.FromMilliseconds(10000f));
			//Check get_remove_buff_tooltip_Squire_Arrest
		}
	}
}
