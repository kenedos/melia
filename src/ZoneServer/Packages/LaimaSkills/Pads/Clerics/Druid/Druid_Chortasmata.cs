using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for the sprouting grass of Chortasmata, which only shows the
	/// grass growing.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.plant_pad_born)]
	public class Druid_ChortasmataBornOverride : ICreatePadHandler, IDestroyPadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, true);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}
	}

	/// <summary>
	/// Handler for Chortasmata's grass, which gives the enemies on it a rash
	/// and the party on it Floral Scent.
	/// </summary>
	/// <remarks>
	/// [Arts] Chortasmata: Healing Garden gives no rash.
	/// </remarks>
	[Package("laima-skills")]
	[PadHandler(PadName.plant_pad)]
	public class Druid_ChortasmataOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler, ILeavePadHandler
	{
		private const int UpdateInterval = 1000;
		private static readonly TimeSpan RashDuration = TimeSpan.FromSeconds(20);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetUpdateInterval(UpdateInterval);
			pad.Trigger.LifeTime = pad.Skill.Properties.CaptionTime;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.Chortasmata_Buff);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var skill = pad.Skill;

			if (caster.IsDead)
				return;

			foreach (var ally in pad.Trigger.GetAlliedEntities(caster).Append(caster))
			{
				if (ally.IsDead || !pad.Trigger.Area.IsInside(ally.Position) || ally.IsBuffActive(BuffId.Chortasmata_Buff))
					continue;

				ally.StartBuff(BuffId.Chortasmata_Buff, skill.Level, 0, pad.Trigger.RemainingLifeTime, caster, skill.Id);
			}

			if (caster.IsAbilityActive(AbilityId.Druid23))
				return;

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio3);

			foreach (var enemy in pad.Trigger.GetAttackableEntities(caster).Take(maxTargets))
			{
				if (enemy.IsBuffActive(BuffId.Chortasmata_Debuff))
					continue;

				var damage = SCR_SkillHit(caster, enemy, skill).Damage;
				if (damage > 0)
					enemy.StartBuff(BuffId.Chortasmata_Debuff, skill.Level, damage, RashDuration, caster, skill.Id);
			}
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			args.Initiator.StopBuff(BuffId.Chortasmata_Buff);
		}
	}
}
