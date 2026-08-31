using System;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Fencer skill Esquive Toucher.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fencer_EsquiveToucher)]
	public class Fencer_EsquiveToucherOverride : IGroundSkillHandler
	{
		private static readonly (int HitDelay, int AniTime)[] HitTimings = [(950, 750), (1400, 450), (1800, 400), (1850, 100), (2100, 200)];
		private const int BuffDurationMs = 3000;
		private const int MovementCheckIntervalMs = 100;

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
			caster.StartBuff(BuffId.EsquiveToucher_Buff, skill.Level, 0f, TimeSpan.FromMilliseconds(BuffDurationMs), caster, skill.Id);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 68, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			foreach (var timing in HitTimings)
			{
				if (!await this.WaitWhileStanding(skill, caster, timing.AniTime))
					break;

				await SkillAttack(caster, skill, splashArea, timing.HitDelay);
			}
		}

		/// <summary>
		/// Waits for the given duration, returning false if the caster
		/// started moving before it elapsed.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="durationMs"></param>
		private async Task<bool> WaitWhileStanding(Skill skill, ICombatEntity caster, int durationMs)
		{
			var remaining = durationMs;

			while (remaining > 0)
			{
				var waitTime = Math.Min(remaining, MovementCheckIntervalMs);
				await skill.Wait(TimeSpan.FromMilliseconds(waitTime));
				remaining -= waitTime;

				if (caster.Components.TryGet<MovementComponent>(out var movement) && movement.IsMoving)
					return false;
			}

			return true;
		}
	}
}
