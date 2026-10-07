using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.PiedPiper
{
	/// <summary>
	/// Handler for the Pied Piper skill Hameln Nagetier, which sends each
	/// of the Pied Piper's mice at an enemy near the target spot.
	/// </summary>
	/// <remarks>
	/// Rare white mice from Hameln Nagetier: Rare Species hit twice.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.PiedPiper_HamelnNagetier)]
	public class PiedPiper_HamelnNagetierOverride : IGroundSkillHandler
	{
		private const float AttackRange = 50f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var mice = PiedPiperSkillHelper.GetMice(caster);
			if (mice.Count == 0)
			{
				caster.ServerMessage(Localization.Get("You have no mice to send."));
				Send.ZC_SKILL_CAST_CANCEL(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var attackPos))
				attackPos = farPos;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, attackPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, attackPos, ForceId.GetNew(), null);

			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, attackPos, AttackRange);
			var hits = new List<SkillHitInfo>();

			foreach (var (mouse, mouseTarget) in mice.Zip(targets))
			{
				mouse.MoveTo(mouseTarget.Position);

				var hitCount = mouse.Id == MonsterId.PiedPiperMouseWhite ? 2 : 1;
				var skillHitResult = SCR_SkillHit(caster, mouseTarget, skill, SkillModifier.MultiHit(hitCount));
				mouseTarget.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, mouseTarget, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
