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
	/// <summary>
	/// Handler for the Necromancer skill Disinter.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Necromancer_Disinter)]
	public class Necromancer_DisinterOverride : IGroundSkillHandler
	{
		private static readonly TimeSpan BuffDuration = TimeSpan.FromSeconds(30);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			var skeletons = character.Summons.GetSummons(s => !s.IsDead && IsSkeleton(s));
			if (skeletons.Count == 0)
			{
				caster.ServerMessage(Localization.Get("You have no skeletons to sacrifice."));
				return;
			}

			var targetPos = skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var groundPos) ? groundPos : farPos;
			var victim = skeletons.FirstOrDefault(s => s.Handle == target?.Handle) ?? skeletons.OrderBy(s => s.Position.Get2DDistance(targetPos)).First();

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, victim.Position);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, victim.Handle, originPos, originPos.GetDirection(victim.Position), victim.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, victim.Position);

			var buffId = victim.Id switch
			{
				MonsterId.SkeletonArcher => BuffId.Disinter_Archer_Buff,
				MonsterId.SkeletonMage => BuffId.Disinter_Wizard_Buff,
				_ => BuffId.Disinter_Soldier_Buff,
			};

			victim.Kill(caster);

			foreach (var summon in skeletons)
			{
				if (summon != victim && !summon.IsDead)
					summon.StartBuff(buffId, skill.Level, 0, BuffDuration, caster, skill.Id);
			}

			caster.StartBuff(BuffId.Disinter_PC_Buff, skill.Level, 0, BuffDuration, caster, skill.Id);
		}

		private static bool IsSkeleton(Summon summon)
			=> summon.Id == MonsterId.SkeletonSoldier || summon.Id == MonsterId.SkeletonArcher || summon.Id == MonsterId.SkeletonMage;
	}
}
