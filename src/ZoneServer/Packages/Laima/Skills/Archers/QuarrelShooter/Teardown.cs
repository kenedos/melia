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
using Melia.Zone.World.Actors.Characters;
using System.Linq;

namespace Melia.Zone.Skills.Handlers.Archers.QuarrelShooter
{
	/// <summary>
	/// Handler for the QuarrelShooter skill Teardown.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.QuarrelShooter_Teardown)]
	public class QuarrelShooterTeardown : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			if (caster is Character character)
			{
				var paviseList = character.Summons.GetSummons(s => s.Data.ClassName == "pavise").ToList();
				foreach (var pavise in paviseList)
				{
					pavise.Kill(null);

					// Knocks back and stuns near enemies
					var enemiesNearby = caster.Map.GetAttackableEntitiesInRangeAroundEntity(caster, pavise, 50);
					var hits = new List<SkillHitInfo>();
					foreach (var enemy in enemiesNearby)
					{
						var skillHitResult = new SkillHitResult { Damage = 0, Result = HitResultType.Hit };
						var skillHit = new SkillHitInfo(caster, enemy, skill, skillHitResult);

						if (enemy.IsKnockdownable())
						{
							var direction = pavise.Position.GetDirection(enemy.Position);
							skillHit.KnockBackInfo = new KnockBackInfo(pavise.Position, enemy, HitType.KnockBack, 20, 10);
							skillHit.HitInfo.Type = HitType.KnockBack;
							enemy.ApplyKnockback(caster, skill, skillHit);
						}

						enemy.StartBuff(BuffId.Stun, TimeSpan.FromSeconds(2), caster);
						hits.Add(skillHit);
					}

					if (hits.Count > 0)
						Send.ZC_SKILL_HIT_INFO(caster, hits);
				}
			}
			caster.ClearTargets();
		}
	}


}
