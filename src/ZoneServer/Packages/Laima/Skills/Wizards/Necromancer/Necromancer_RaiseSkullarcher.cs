using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for the Necromancer skill Raise Skullarcher.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Necromancer_RaiseSkullarcher)]
	public class Necromancer_RaiseSkullarcherOverride : IGroundSkillHandler
	{
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

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
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

				var attack = RandomProvider.Get().Next((int)caster.Properties.GetFloat(PropertyName.MINMATK),
					(int)caster.Properties.GetFloat(PropertyName.MINMATK))
					* (.85f + (.15f * skill.Level));
				var life = caster.Properties.GetFloat(PropertyName.MHP) * (.44f + (.12f * skill.Level));
				var defense = (caster.Properties.GetFloat(PropertyName.DEF)
					+ caster.Properties.GetFloat(PropertyName.MDEF)) / 2
					* (1.02f + (17f * skill.Level));

				summon.Properties.SetFloat(PropertyName.FixedAttack, attack);
				summon.Properties.SetFloat(PropertyName.FixedLife, life);
				summon.Properties.SetFloat(PropertyName.FixedDefence, defense);
				summon.Properties.InvalidateAll();
				summon.Components.Add(new LifeTimeComponent(summon, TimeSpan.FromMinutes(30)));
				summon.SetState(true);

				skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
				Send.ZC_SYNC_START(caster, skillHandle, 1);
				summon.StartBuff(BuffId.Ability_buff_PC_Summon, TimeSpan.Zero, summon);
				Send.ZC_SYNC_END(caster, skillHandle, 0);
				Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, skill.Data.DefaultHitDelay);
			}
		}
	}
}
