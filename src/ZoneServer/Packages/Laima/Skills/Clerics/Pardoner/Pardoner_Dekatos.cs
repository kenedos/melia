using System;
using System.Collections.Generic;
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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Pardoner skill Dekatos.
	/// Attack an enemy with money collected as offerings.
	/// Applies a debuff that has a chance to instantly kill non-boss monsters on expiration.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Pardoner_Dekatos)]
	public class Pardoner_DekatosOverride : IGroundSkillHandler
	{
		private const float BuffDurationMs = 5000f;
		private const int HitDelay = 500;
		private const int AniTime = 300;

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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hits = new List<SkillHitInfo>();

			await SkillAttack(caster, skill, splashArea, HitDelay, AniTime, hits);

			var buffDuration = TimeSpan.FromMilliseconds(BuffDurationMs);

			foreach (var hit in hits)
			{
				var target = hit.Target;
				if (target != null && !target.IsDead)
				{
					target.StartBuff(BuffId.Dekatos_Buff, skill.Level, 0, buffDuration, caster, skill.Id);
				}
			}
		}
	}
}
