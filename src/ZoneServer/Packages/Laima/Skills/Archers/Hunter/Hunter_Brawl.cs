using System;
using System.Collections.Generic;
using System.Linq;
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
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;

namespace Melia.Zone.Skills.Handlers.Hunter
{
	/// <summary>
	/// Handler for the Hunter skill Brawl.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Hunter_Brawl)]
	public class Hunter_BrawlOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TryGetActiveCompanion(out var companion))
			{
				if (caster is Character character)
					character.SystemMessage("CompanionIsNotActive");
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(skill, caster, companion, targetPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Companion companion, Position targetPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var bait = this.ThrowBrawlBait(caster, targetPos);
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			Hunter_PetAttackOverride.TryActivate(caster, companion, bait);

			var enemies = caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, 90);
			if (enemies.Any())
			{
				var pad = SkillCreatePad(caster, skill, targetPos, 0f, PadName.Hunter_Brawl);

				var companionClassName = companion.Data.ClassName;

				await skill.Wait(TimeSpan.FromMilliseconds(500));

				for (var i = 0; i < 6; i++)
				{
					await skill.Wait(TimeSpan.FromMilliseconds(500));

					if (caster == null || companion == null || companion.Owner == null)
						break;

					var copy = this.SpawnCompanionCopy(caster, companion, companionClassName, targetPos);
				}
			}
		}

		private Mob SpawnCompanionCopy(ICombatEntity caster, Companion companion, string companionClassName, Position centerPos)
		{
			var rnd = RandomProvider.Get();
			var angle = rnd.NextDouble() * Math.PI * 2;
			var distance = rnd.Next(20, 60);
			var offsetX = (float)(Math.Cos(angle) * distance);
			var offsetZ = (float)(Math.Sin(angle) * distance);

			var spawnPos = new Position(centerPos.X + offsetX, centerPos.Y, centerPos.Z + offsetZ);

			var copy = CreateMonster(caster, companionClassName, spawnPos, 0, companion.Level, relationType: RelationType.Friendly);
			copy.SetHittable(false);
			copy.FromGround = true;
			copy.Faction = caster.Faction;
			copy.Tendency = TendencyType.Aggressive;

			copy.Components.Add(new MovementComponent(copy));
			copy.Components.Add(new AiComponent(copy, "BasicMonster", caster));

			caster.Map.AddMonster(copy);
			copy.DelayEnterWorld();
			copy.EnterDelayedActor();
			copy.DisappearTime = DateTime.Now.AddSeconds(2);

			return copy;
		}

		private Mob ThrowBrawlBait(ICombatEntity caster, Position targetPos)
		{
			var meat = CreateMonster(caster, "meat2", targetPos, 0, caster.Level, relationType: RelationType.Friendly);
			meat.SetHittable(false);
			meat.Faction = caster.Faction;
			caster.Map.AddMonster(meat);
			meat.DelayEnterWorld();
			meat.EnterDelayedActor();
			meat.DisappearTime = DateTime.Now.AddSeconds(5);
			Send.ZC_NORMAL.Skill_08(meat, "F_smoke109_2", 1.5f, targetPos, 0.5f, 0f, 900f, 1f, 0f);
			return meat;
		}
	}
}
