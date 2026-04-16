using System;
using Melia.Shared.Packages;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Handlers.Bokor;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Maps;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Skills.SplashAreas;
using System.Collections.Generic;
using System.Linq;

namespace Melia.Zone.Skills.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler for the Bokor skill Damballa.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bokor_Damballa)]
	public class Bokor_DamballaOverride : IGroundSkillHandler
	{
		/// <summary>
		/// Handles skill behavior
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var maxRange = skill.Properties.GetFloat(PropertyName.MaxR);

			if (caster is not Character character)
				return;

			if (!caster.Position.InRange2D(farPos, maxRange))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				return;
			}

			if (!caster.TryGetBuff(BuffId.PowerOfDarkness_Buff, out var darkForceBuff) || darkForceBuff.OverbuffCounter < 10)
			{
				caster.ServerMessage(Localization.Get("Requires at least 10 stacks of Dark Force."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			darkForceBuff.OverbuffCounter -= 10;
			darkForceBuff.NotifyUpdate();

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			var summons = character.Summons.GetSummons();
			var zombiesKilled = summons.Count;

			// Cap respawn count by the CURRENT zombie type's max count so that
			// switching abilities (e.g., kill 6 default zombies then respawn as
			// giants which cap at 2) can never exceed the active cap.
			var zombieInfo = ZombifyHelper.GetZombieInfo(caster);
			var respawnCap = Math.Min(zombiesKilled, zombieInfo.MaxCount);
			var killedEnemyPositions = new List<Position>();

			foreach (var summon in summons)
			{
				var enemiesInRange = summon.Map.GetAttackableEntitiesInRangeAroundEntity(character, summon, 150f);
				var targetsToHit = enemiesInRange.Take(5);

				summon.Kill(caster);

				foreach (var t in targetsToHit)
				{
					var skillHitResult = SCR_SkillHit(caster, t, skill);
					t.PlayEffect("F_rize004_dark_damballa", 5f, 0);
					t.TakeDamage(skillHitResult.Damage, caster);

					var hitInfo = new HitInfo(caster, t, skill, skillHitResult.Damage, skillHitResult.Result);
					hitInfo.AniTime = TimeSpan.FromMilliseconds(100);

					Send.ZC_HIT_INFO(caster, t, hitInfo);

					if (t.IsDead && killedEnemyPositions.Count < respawnCap)
						killedEnemyPositions.Add(t.Position);
				}
			}

			// Respawn zombies on killed enemy corpses. Re-check the live summon
			// count after the kills above (all bokor summons are now dead, but
			// mixed-type remnants may still exist if the player recently switched
			// abilities) and only fill up to the current zombie type's cap.
			if (killedEnemyPositions.Count > 0 && caster.TryGetSkill(SkillId.Bokor_Zombify, out var zombifySkill))
			{
				if (ZoneServer.Instance.Data.MonsterDb.TryFind(zombieInfo.ClassName, out var monsterData))
				{
					var existingOfType = character.Summons.GetSummons(monsterData.Id).Count;
					var remaining = zombieInfo.MaxCount - existingOfType;
					var toSpawn = Math.Min(killedEnemyPositions.Count, remaining);

					for (var i = 0; i < toSpawn; i++)
					{
						ZombifyHelper.SummonZombieAt(zombifySkill, caster, zombieInfo, killedEnemyPositions[i]);
					}
				}
			}
		}
	}
}
