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
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Musketeer
{
	/// <summary>
	/// Handler for the Musketeer skill Volleyfire.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Musketeer_Volleyfire)]
	public class Musketeer_VolleyfireOverride : IForceSkillHandler
	{
		private const int ShotCount = 4;
		private static readonly TimeSpan FirstShotDelay = TimeSpan.FromMilliseconds(560);
		private static readonly TimeSpan ShotDelay = TimeSpan.FromMilliseconds(120);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (target == null || target.IsDead || !caster.CanDamage(target))
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target.Handle;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill);

			skill.Run(this.HandleSkill(caster, skill, target));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, ICombatEntity target)
		{
			caster.SetTarget(target);

			await skill.Wait(FirstShotDelay);

			for (var i = 0; i < ShotCount; ++i)
			{
				SkillTargetDamage(skill, caster, caster.GetTargets());

				if (i < ShotCount - 1)
					await skill.Wait(ShotDelay);
			}
		}
	}
}
