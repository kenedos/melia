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

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Schwarzereiter skill Marching Fire.
	/// The caster keeps firing at the enemies in front of them for as long
	/// as the skill is held.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_AssaultFire)]
	public class SchwarzerReiter_AssaultFireOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float FireDistance = 150f;
		private const float FireWidth = 40f;
		private static readonly TimeSpan FireInterval = TimeSpan.FromMilliseconds(200);
		private static readonly TimeSpan MaxDuration = TimeSpan.FromSeconds(5);
		private const int MaxTargets = 15;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StartBuff(BuffId.AssaultFire_Buff, skill.Level, 0f, MaxDuration, caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopBuff(BuffId.AssaultFire_Buff);
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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Fire(caster, skill));
		}

		/// <summary>
		/// Damages the enemies in front of the caster until the hold ends.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		private async Task Fire(ICombatEntity caster, Skill skill)
		{
			while (caster.IsBuffActive(BuffId.AssaultFire_Buff))
			{
				await skill.Wait(FireInterval);

				if (caster.IsDead)
					break;

				var targets = SkillSelectEnemiesInSquare(caster, caster.Position, 0f, FireDistance, FireWidth, MaxTargets);
				if (targets.Count > 0)
					SkillTargetDamage(skill, caster, targets);
			}
		}
	}
}
