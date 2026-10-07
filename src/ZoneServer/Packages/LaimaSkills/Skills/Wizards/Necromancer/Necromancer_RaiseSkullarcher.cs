using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for the Necromancer skill Raise Skullarcher.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Necromancer_RaiseSkullarcher)]
	public class Necromancer_RaiseSkullarcherOverride : IGroundSkillHandler
	{
		private const int CorpsePartsCost = 5;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!NecromancerSkillHelper.CanSummonSkeleton(caster, MonsterId.SkeletonArcher))
			{
				caster.ServerMessage(Localization.Get("You cannot summon any more Skeleton Archers."));
				return;
			}

			if (!NecromancerSkillHelper.HasCorpseParts(caster, CorpsePartsCost))
			{
				caster.ServerMessage(Localization.Get("Not enough corpse parts."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			NecromancerSkillHelper.SpendCorpseParts(caster, CorpsePartsCost);

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			if (caster is Character character)
			{
				var summon = new Summon(character, MonsterId.SkeletonArcher, RelationType.Friendly);
				character.Summons.AddSummon(summon);
				summon.Name = "!@#${Auto_1}_of_{Auto_2}$*$Auto_1$*$" + caster.Name + "$*$Auto_2$*$@dicID_^*$ETC_20150317_000235$*^#@!";
				summon.OwnerHandle = caster.Handle;
				summon.Faction = FactionType.Law;
				summon.Tendency = TendencyType.Aggressive;
				summon.FromGround = true;
				summon.Properties.SetFloat(PropertyName.Level, caster.Level);
				summon.Properties.SetFloat(PropertyName.FIXMSPD_BM, 140f);

				NecromancerSkillHelper.ApplySummonTransfer(summon, caster, NecromancerSkillHelper.GetReinforcedRatio(skill, PropertyName.CaptionRatio), skill.Properties.GetFloat(PropertyName.CaptionRatio2), skill.Properties.GetFloat(PropertyName.CaptionRatio3));
				summon.Components.Add(new LifeTimeComponent(summon, TimeSpan.FromMinutes(30)));
				summon.SetState(true);

				skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
				Send.ZC_SYNC_START(caster, skillHandle, 1);
				summon.StartBuff(BuffId.Ability_buff_PC_Summon, skill.Level, 0, TimeSpan.Zero, summon, skill.Id);
				Send.ZC_SYNC_END(caster, skillHandle, 0);
				Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, skill.Data.DefaultHitDelay);
			}
		}
	}
}
