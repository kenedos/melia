using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for the Necromancer skill Disinter.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Necromancer_Disinter)]
	public class Necromancer_DisinterOverride : IGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime) { }

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime) { }

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			if (target != null && caster is Character character && character.Summons.TryGetSummon(target.Handle, out var victimizedSummon))
			{
				target.Kill(null);

				var buffId = BuffId.Disinter_Soldier_Buff;
				switch (victimizedSummon.Id)
				{
					case MonsterId.SkeletonArcher:
						buffId = BuffId.Disinter_Archer_Buff;
						break;
					case MonsterId.SkeletonSoldier:
						buffId = BuffId.Disinter_Soldier_Buff;
						break;
					case MonsterId.SkeletonMage:
						buffId = BuffId.Disinter_Wizard_Buff;
						break;
				}

				var skeletonSummons = character.Summons.GetSummons(s => s.Id == MonsterId.SkeletonSoldier || s.Id == MonsterId.SkeletonArcher || s.Id == MonsterId.SkeletonMage);
				foreach (var summon in skeletonSummons)
				{
					summon.StartBuff(buffId, skill.Level, 0, TimeSpan.FromSeconds(30), caster, skill.Id);
				}

				caster.StartBuff(BuffId.Disinter_PC_Buff, TimeSpan.FromSeconds(30), caster);
			}
		}
	}
}
