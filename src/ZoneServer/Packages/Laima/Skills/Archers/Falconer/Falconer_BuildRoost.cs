using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Falconer skill Build Roost.
	/// Creates a hawk roost at the target location that provides buffs to
	/// nearby allies and allows the hawk to recover stamina faster.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_BuildRoost)]
	public class Falconer_BuildRoostOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float BaseRoostRadius = 100f;
		private const int RoostDurationSeconds = 20;
		private const int BuffTickIntervalMs = 3000;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!FalconerHawkHelper.TryGetHawk(caster, out var hawk))
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

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, targetPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position targetPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(400));

			// Spawn roost structure
			var roost = MonsterSkillCreateMob(skill, caster, "pcskill_falconer_roost", targetPos, 0f, "", "", 0, RoostDurationSeconds, "None", "");

			if (roost == null)
				return;

			roost.SetHittable(false);
			roost.MonsterType = RelationType.Friendly;
			roost.Faction = FactionType.Law;
			roost.StartBuff(BuffId.Invincible);

			// Register roost with hawk helper (removes old roost if exists)
			FalconerHawkHelper.RegisterRoost(caster, roost);

			// Tell the hawk to fly to the roost and land
			if (FalconerHawkHelper.TryGetHawk(caster, out var hawk))
				hawk.SetRoost(roost);

			// Run the roost auto-destroy check in background
			skill.Run(this.CheckRoostAutoDestroy(skill, caster, roost));
		}

		/// <summary>
		/// Monitors the roost and destroys it if owner moves too far.
		/// </summary>
		private async Task CheckRoostAutoDestroy(Skill skill, ICombatEntity caster, Mob roost)
		{
			for (int i = 0; i < RoostDurationSeconds; i++)
			{
				if (caster == null || caster.IsDead)
				{
					this.DestroyRoost(caster, roost);
					return;
				}

				if (FalconerHawkHelper.TryGetHawk(caster, out var hawk))
				{
					if (!hawk.IsActivated)
					{
						this.DestroyRoost(caster, roost);
						return;
					}
				}

				await skill.Wait(TimeSpan.FromMilliseconds(1000));
			}

			this.DestroyRoost(caster, roost);
		}

		/// <summary>
		/// Destroys the roost and unregisters it.
		/// </summary>
		private void DestroyRoost(ICombatEntity caster, Mob roost)
		{
			if (caster != null)
			{
				FalconerHawkHelper.UnregisterRoost(caster);

				if (FalconerHawkHelper.TryGetHawk(caster, out var hawk))
					hawk.ClearRoost();
			}

			if (roost != null && !roost.IsDead)
			{
				roost.Kill(null);
			}
		}
	}
}
