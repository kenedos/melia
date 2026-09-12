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

namespace Melia.Zone.Skills.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Pardoner skill Discern Evil.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Pardoner_DiscernEvil)]
	public class Pardoner_DiscernEvilOverride : IGroundSkillHandler
	{
		private const float CenterDistance = 35f;
		private const float Radius = 70f;
		private const int MaxTargets = 5;

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

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(600));

			var centerPos = caster.Position.GetRelative(caster.Direction, CenterDistance);

			caster.SetTargets(SkillSelectEnemiesInCircle(caster, centerPos, Radius, MaxTargets));

			SkillTargetBuff(skill, caster, caster.GetTargets(), BuffId.DiscernEvil_Buff, skill.Level, skill.Level, skill.Properties.CaptionTime, skill.Id);
		}
	}
}
