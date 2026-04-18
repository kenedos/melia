using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for the Necromancer skill Gather Corpse.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Necromancer_GatherCorpse)]
	public class Necromancer_GatherCorpseOverride : IForceSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity designatedTarget)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			if (designatedTarget == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			caster.TurnTowards(designatedTarget);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, designatedTarget.Position, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, designatedTarget.Handle, caster.Position, designatedTarget.Direction, Position.Zero);

			var damageDelay = TimeSpan.Zero;
			var skillHitDelay = skill.Properties.HitDelay;

			var splashArea = new CircleF(designatedTarget.Position, (int)skill.Properties.GetFloat(PropertyName.SplRange));
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			var skillHits = new List<SkillHitInfo>();

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.ForceId = ForceId.GetNew();

				target.StartBuff(BuffId.NecromancerPoison_Debuff, TimeSpan.FromSeconds(20), caster);
				target.StartBuff(BuffId.GatherCorpse_Debuff, TimeSpan.FromSeconds(6), caster);

				skillHits.Add(skillHit);
			}

			Send.ZC_SKILL_FORCE_TARGET(caster, designatedTarget, skill, skillHits);

			Send.ZC_NORMAL.Skill_45(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
		}
	}
}
