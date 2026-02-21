using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Newtonsoft.Json.Linq;

namespace Melia.Zone.Skills.Handlers.Cryomancer
{
	/// <summary>
	/// Handler for the Cryomancer skill Icicle Pike.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cryomancer_IciclePike)]
	public class Cryomancer_IciclePikeOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const float BaseFreezeChance = 40f;
		private const float FreezeChancePerLevel = 4f;
		private const int MaxTargets = 16;
		private const int FreezeDurationMilliSeconds = 7000;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

			if (!caster.InSkillUseRange(skill, targetPos))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, targetPos);

			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, targetPos);

			var targetList = caster.Map.GetAttackableEnemiesIn(caster, new CircleF(targetPos, 100), MaxTargets);
			var damageDelay = TimeSpan.FromMilliseconds(500);

			var freezeChance = BaseFreezeChance + (skill.Level * FreezeChancePerLevel);

			if (caster.TryGetActiveAbility(AbilityId.Cryomancer9, out var abilCryomancer9))
				freezeChance = (int)Math.Floor(freezeChance * (1 + abilCryomancer9.Level * 0.05));

			var hits = new List<SkillHitInfo>();
			var random = RandomProvider.Get();
			foreach (var currentTarget in targetList)
			{
				var skillHitResult = SCR_SkillHit(caster, currentTarget, skill, SkillModifier.MultiHit(4));
				currentTarget.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, currentTarget, skill, skillHitResult, damageDelay, TimeSpan.Zero);
				hits.Add(skillHit);

				if (currentTarget.IsDead)
					continue;

				if ((random.Next(100) < freezeChance) && skillHitResult.Damage > 0)
				{
					Send.ZC_SYNC_START(caster, skillHandle, 1);
					currentTarget.StartBuff(BuffId.Cryomancer_Freeze, TimeSpan.FromMilliseconds(FreezeDurationMilliSeconds), caster);
					Send.ZC_SYNC_END(caster, skillHandle, 0);
					Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, TimeSpan.FromMilliseconds(100));
				}
			}

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, hits);
		}
	}
}
