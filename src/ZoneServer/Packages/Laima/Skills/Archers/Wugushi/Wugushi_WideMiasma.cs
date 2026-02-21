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
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Wugushi
{
	/// <summary>
	/// Handler for the Wugushi skill Wide Miasma.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Wugushi_WideMiasma)]
	public class Wugushi_WideMiasmaOverride : IMeleeGroundSkillHandler
	{
		private const float CasterBuffDurationMs = 5000f;
		private const float TargetDebuffDurationMs = 15000f;
		private const float SplashRadius = 120f;
		private const int BaseTargetCount = 10;
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

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(200));

			var targetPos = originPos.GetRelative(farPos);

			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, SplashRadius, BaseTargetCount));
			var skillTargets = caster.GetTargets();

			foreach (var target in skillTargets)
			{
				var damage = (int)SCR_SkillHit(caster, target, skill).Damage;
				if (damage <= 0)
					continue;

				target.StartBuff(BuffId.WideMiasma_Debuff, skill.Level, damage, TimeSpan.FromMilliseconds(TargetDebuffDurationMs), caster, skill.Id);
			}

			caster.StartBuff(BuffId.WideMiasma_Buff, skill.Level, 0f, TimeSpan.FromMilliseconds(CasterBuffDurationMs), caster);
		}
	}
}
