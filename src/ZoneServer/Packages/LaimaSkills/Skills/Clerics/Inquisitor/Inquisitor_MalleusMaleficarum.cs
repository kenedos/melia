using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Inquisitor
{
	/// <summary>
	/// Handler for the Inquisitor skill Malleus Maleficarum, which fires 7
	/// bolts from a spellbook at up to 7 enemies ahead, halving their INT and
	/// SPR and doubling the SP their magic costs.
	/// </summary>
	/// <remarks>
	/// With Malleus Maleficarum: Mana Burn, enemy characters lose 5% of their
	/// SP per ability level and monsters are silenced.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Inquisitor_MalleusMaleficarum)]
	public class Inquisitor_MalleusMaleficarumOverride : IGroundSkillHandler
	{
		private const int BoltCount = 7;
		private const float Length = 180f;
		private const float Width = 80f;
		private const float ManaBurnPerLevel = 0.05f;
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(600);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: Length, width: Width, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea).Take(BoltCount).ToList();
			var duration = skill.Properties.CaptionTime;
			caster.TryGetActiveAbilityLevel(AbilityId.Inquisitor11, out var manaBurnLevel);

			var hits = new List<SkillHitInfo>();

			if (targets.Count > 0)
			{
				for (var bolt = 0; bolt < BoltCount; bolt++)
				{
					var hitTarget = targets[bolt % targets.Count];

					var skillHitResult = SCR_SkillHit(caster, hitTarget, skill);
					hitTarget.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, hitTarget, skill, skillHitResult, HitDelay, TimeSpan.Zero);
					skillHit.ForceId = ForceId.GetNew();
					skillHit.HitFrameIndex = (byte)bolt;
					skillHit.TargetIndex = (byte)(bolt / targets.Count);

					hits.Add(skillHit);
				}

				foreach (var hitTarget in targets.Where(t => !t.IsDead))
				{
					hitTarget.StartBuff(BuffId.MalleusMaleficarum_Debuff, skill.Level, 0, duration, caster, skill.Id);

					if (manaBurnLevel <= 0)
						continue;

					if (hitTarget is Character victim)
						victim.ModifySp(-victim.Properties.GetFloat(PropertyName.SP) * ManaBurnPerLevel * manaBurnLevel);
					else
						hitTarget.StartBuff(BuffId.Silence_Debuff, 1, 0, duration, caster, skill.Id);
				}
			}

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, hits);
		}
	}
}
