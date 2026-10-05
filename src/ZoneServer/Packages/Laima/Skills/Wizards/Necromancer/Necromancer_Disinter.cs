using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Necromancer_Disinter)]
	public class Necromancer_DisinterOverride : IGroundSkillHandler, IDynamicCasted
	{
		private static readonly TimeSpan BuffDuration = TimeSpan.FromSeconds(30);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime) { }

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime) { }

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			var skeletonSummons = character.Summons.GetSummons(s => !s.IsDead && (s.Id == MonsterId.SkeletonSoldier || s.Id == MonsterId.SkeletonArcher || s.Id == MonsterId.SkeletonMage)).ToArray();

			if (skeletonSummons.Length == 0)
			{
				caster.ServerMessage(Localization.Get("You do not have any active skeleton summons."));
				return;
			}

			var targetPosition = skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var groundPosition) ? groundPosition : farPos;

			Summon victimizedSummon = null;

			if (target != null && !target.IsDead && character.Summons.TryGetSummon(target.Handle, out var selectedSummon))
			{
				if (selectedSummon.Id == MonsterId.SkeletonSoldier || selectedSummon.Id == MonsterId.SkeletonArcher || selectedSummon.Id == MonsterId.SkeletonMage)
					victimizedSummon = selectedSummon;
			}

			victimizedSummon ??= skeletonSummons.OrderBy(summon => summon.Position.Get2DDistance(targetPosition)).First();

			BuffId buffId;

			switch (victimizedSummon.Id)
			{
				case MonsterId.SkeletonSoldier:
					buffId = BuffId.Disinter_Soldier_Buff;
					break;

				case MonsterId.SkeletonArcher:
					buffId = BuffId.Disinter_Archer_Buff;
					break;

				case MonsterId.SkeletonMage:
					buffId = BuffId.Disinter_Wizard_Buff;
					break;

				default:
					return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			var remainingSummons = skeletonSummons.Where(summon => summon.Handle != victimizedSummon.Handle).ToArray();

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, victimizedSummon.Position);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, victimizedSummon.Handle, originPos, originPos.GetDirection(victimizedSummon.Position), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, victimizedSummon.Position);

			victimizedSummon.Kill(caster);

			foreach (var summon in remainingSummons)
			{
				if (!summon.IsDead)
					summon.StartBuff(buffId, skill.Level, 0, BuffDuration, caster, skill.Id);
			}

			caster.StartBuff(BuffId.Disinter_PC_Buff, skill.Level, 0, BuffDuration, caster, skill.Id);
		}
	}
}
