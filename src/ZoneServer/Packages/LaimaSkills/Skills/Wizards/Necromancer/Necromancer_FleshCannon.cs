using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for the Necromancer skill Flesh Cannon.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Necromancer_FleshCannon)]
	public class Necromancer_FleshCannonOverride : IGroundSkillHandler
	{
		private const int CorpsePartsCost = 15;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

			if (!NecromancerSkillHelper.HasCorpseParts(caster, CorpsePartsCost))
			{
				caster.ServerMessage(Localization.Get("Not enough corpse parts."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			NecromancerSkillHelper.SpendCorpseParts(caster, CorpsePartsCost);

			skill.IncreaseOverheat();
			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, targetPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, caster.Position);

			var effectHandle = ZoneServer.Instance.World.CreateEffectHandle();

			Send.ZC_NORMAL.PlayCorpsePartsRing(caster, effectHandle, 0.25f, 20, 15, CorpsePartsCost, 400981);

			var targetList = caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, (int)skill.Data.SplashRange * 4);
			var damageDelay = TimeSpan.FromMilliseconds(200);

			var hits = new List<SkillHitInfo>();

			foreach (var currentTarget in targetList.LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, currentTarget, skill);
				currentTarget.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, currentTarget, skill, skillHitResult, damageDelay, TimeSpan.Zero);
				hits.Add(skillHit);
			}

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, hits);

			if (caster.IsAbilityActive(AbilityId.Necromancer2))
			{
				foreach (var hit in hits)
				{
					Send.ZC_SYNC_START(caster, skillHandle, 1);
					NecromancerSkillHelper.ApplyDemoralize(caster, hit.Target, skill);
					Send.ZC_SYNC_END(caster, skillHandle, 0);
					Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, TimeSpan.FromMilliseconds(400));
				}
			}

			caster.StopBuff(BuffId.FleshHoop_Buff);
		}
	}
}
