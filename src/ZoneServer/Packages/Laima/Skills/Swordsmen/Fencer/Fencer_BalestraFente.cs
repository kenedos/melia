using System;
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

namespace Melia.Zone.Skills.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Fencer skill Balestra Fente.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fencer_BalestraFente)]
	public class Fencer_BalestraFenteOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_NORMAL.UpdateSkillEffect(caster, target?.Handle ?? 0, originPos, caster.Direction, Position.Zero);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var startPos = caster.Position.GetRelative(caster.Direction, 0);
			var targetPos = caster.Position.GetRelative(caster.Direction, 80);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var value = skill.GetPVPValue(10);
			var skillTargets = SkillSelectEnemiesInSquare(caster, startPos, 0f, 120f, 25f, value);
			if (skillTargets == null || skillTargets.Count == 0)
				return;
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			SkillTargetDamage(skill, caster, skillTargets, 1f);
			caster.Position = targetPos;
			Send.ZC_MOVE_STOP(caster, targetPos, 1);
		}
	}
}
