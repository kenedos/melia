using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Corsair skill Iron Hook.
	/// Throws a hook at enemies in target area to pull and restrain them.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Corsair_IronHook)]
	public class Corsair_IronHookOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int HookDelayMs = 300;
		private const float HookTargetRadius = 30f;

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

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, targetPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position targetPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(HookDelayMs));

			this.MakeHook(caster, skill, targetPos);

			skill.Run(this.MonitorCasterMovement(skill, caster));
		}

		private async Task MonitorCasterMovement(Skill skill, ICombatEntity caster)
		{
			if (caster is not Character character)
				return;

			while (caster.IsBuffActive(BuffId.IronHook))
			{
				await skill.Wait(TimeSpan.FromMilliseconds(100));

				if (character.Movement.IsMoving)
				{
					caster.RemoveBuff(BuffId.IronHook);
					break;
				}
			}
		}

		private void MakeHook(ICombatEntity caster, Skill skill, Position targetPos)
		{
			var maxTargets = (int)Math.Ceiling(skill.Level / 2f);
			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, HookTargetRadius)
				.Where(t => t.EffectiveSize != SizeType.XL)
				.Where(t => t.MoveType != MoveType.Holding)
				.Where(t => t.Rank != MonsterRank.MISC && t.Rank != MonsterRank.NPC)
				.Take(maxTargets)
				.ToList();

			var time = 3000 + skill.Level * 200;

			if (targets.Count == 0)
			{
				Send.ZC_NORMAL.ShowHookEffect(caster, "None", 1, "Warrior_Pull", "Dummy_R_HAND", 500, 2, targetPos);
				return;
			}

			var buff = caster.StartBuff(BuffId.IronHook, skill.Level, 0, TimeSpan.FromMilliseconds(time), caster);
			if (buff == null)
			{
				Send.ZC_NORMAL.ShowHookEffect(caster, "None", 1, "Warrior_Pull", "Dummy_R_HAND", 500, 2, targetPos);
				return;
			}

			var hookedTargets = new List<ICombatEntity>();

			foreach (var target in targets)
			{
				target.InsertHate(caster, 150);
				Send.ZC_NORMAL.MakeHookEffect(caster, target, "None", 1, "Warrior_Pull", "Dummy_R_HAND", "Bip01 Spine2", 500, 2);

				target.SetTempVar("Melia.IronHook.Effect", "None");
				target.SetTempVar("Melia.IronHook.Scale", 1f);
				target.SetTempVar("Melia.IronHook.Node", "Bip01 Spine2");

				var targetBuff = target.StartBuff(BuffId.IronHooked, skill.Level, 1, TimeSpan.FromMilliseconds(time), caster);
				if (targetBuff != null)
				{
					targetBuff.Vars.Set("Melia.IronHook.Effect", "None");
					targetBuff.Vars.Set("Melia.IronHook.Scale", 1f);
					targetBuff.Vars.Set("Melia.IronHook.Node", "Bip01 Spine2");
					hookedTargets.Add(target);
				}
			}

			if (hookedTargets.Count == 0)
			{
				caster.RemoveBuff(BuffId.IronHook);
				Send.ZC_NORMAL.RemoveHookEffect(caster);
				return;
			}

			buff.Vars.Set("Melia.IronHook.Targets", hookedTargets);
		}
	}
}
