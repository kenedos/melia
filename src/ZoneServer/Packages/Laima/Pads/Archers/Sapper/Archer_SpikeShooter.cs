using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Logging;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Scripting;

namespace Melia.Zone.Pads.HandlersOverride.Archers.Sapper
{
	/// <summary>
	/// Handler for the Sapper Spike Shooter pad.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Archer_SpikeShooter)]
	public class Archer_SpikeShooterOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int UpdateIntervalMs = 1000;
		private const string EffectMonsterClassName = "pcskill_stake_stockades2";
		private const float SplashLength = 150f;
		private const int TrapMaxHP = 10;
		private const string StartMonsterKey = "Melia.SpikeShooter.StartMonster";
		private const string EndMonsterKey = "Melia.SpikeShooter.EndMonster";

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			pad.Direction = creator.Direction.Left;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetUpdateInterval(UpdateIntervalMs);
			pad.NumArg1 = skill.Level;

			var durationMs = 120000;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(durationMs);

			var size = skill.Level * 2 + 17;
			var startPos = pad.Position.GetRelative(pad.Direction, -size / 2);
			var endPos = pad.Position.GetRelative(pad.Direction, size / 2);

			var startMonster = PadCreateMonster(pad, EffectMonsterClassName, startPos, pad.Direction.DegreeAngle, durationMs, "None", 1f);
			var endMonster = PadCreateMonster(pad, EffectMonsterClassName, endPos, pad.Direction.DegreeAngle, durationMs, "None", 1f);

			if (startMonster != null)
			{
				var propertyOverrides = new PropertyOverrides();
				propertyOverrides.Add(PropertyName.HPCount, TrapMaxHP);
				((Mob)startMonster).ApplyOverrides(propertyOverrides);
				startMonster.Properties.InvalidateAll();
				((Mob)startMonster).HealToFull();
				((Mob)startMonster).Died += (mob, killer) => pad.Destroy();
				pad.Variables.Set(StartMonsterKey, startMonster);
			}

			if (endMonster != null)
			{
				var propertyOverrides = new PropertyOverrides();
				propertyOverrides.Add(PropertyName.HPCount, TrapMaxHP);
				((Mob)endMonster).ApplyOverrides(propertyOverrides);
				endMonster.Properties.InvalidateAll();
				((Mob)endMonster).HealToFull();
				((Mob)endMonster).Died += (mob, killer) => pad.Destroy();
				pad.Variables.Set(EndMonsterKey, endMonster);
			}

			if (startMonster != null)
				Send.ZC_NORMAL.PadLinkEffect(pad, startPos, endPos, "SpikeShooter", startMonster.Id);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			if (pad.Variables.TryGet<IMonster>(StartMonsterKey, out var startMonster))
				startMonster.Map?.RemoveMonster(startMonster);

			if (pad.Variables.TryGet<IMonster>(EndMonsterKey, out var endMonster))
				endMonster.Map?.RemoveMonster(endMonster);

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			this.ShootSpikeShooter(pad, skill, creator);
		}

		private void ShootSpikeShooter(Pad pad, Skill skill, ICombatEntity caster)
		{
			if (!pad.Variables.TryGet<IMonster>(StartMonsterKey, out var startMonster))
				return;

			if (!pad.Variables.TryGet<IMonster>(EndMonsterKey, out var endMonster))
				return;

			this.ShootFromMonster(pad, skill, caster, startMonster);
			this.ShootFromMonster(pad, skill, caster, endMonster);
		}

		private void ShootFromMonster(Pad pad, Skill skill, ICombatEntity caster, IMonster sourceMonster)
		{
			var splashWidth = 70f + skill.Level;
			var splashArea = new Square(sourceMonster.Position, pad.Direction.Right, SplashLength, splashWidth);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var targetList = targets.Take(1);

			foreach (var target in targetList)
			{
				var modifier = SkillModifier.Default;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var hit = new HitInfo(caster, target, skill, skillHitResult);
				hit.ForceId = ForceId.GetNew();

				Send.ZC_NORMAL.PlayForceEffect(caster, sourceMonster, target, hit.ForceId, "I_arrow009", 0.5f, "None", "None", 0, "None", "FAST", 500, 1, 20, 5, 0, 1);
				Send.ZC_HIT_INFO(caster, target, hit);
			}
		}
	}
}
