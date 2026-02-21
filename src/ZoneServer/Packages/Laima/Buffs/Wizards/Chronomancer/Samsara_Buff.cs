using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Chronomancer
{
	[Package("laima")]
	[BuffHandler(BuffId.Samsara_Buff)]
	public class Samsara_BuffOverride : BuffHandler
	{
		private const string VarFailed = "Melia.Skill.Samsara.Failed";
		private const string VarFirstTick = "Melia.Skill.Samsara.FirstTick";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var caster = buff.Caster as Character;
			var party = caster?.Connection?.Party;

			if (target is not Mob monster || monster.Data.Rank != MonsterRank.Boss)
			{
				buff.Vars.SetBool(VarFailed, true);
				return;
			}

			var successRate = 2;
			if (party != null)
			{
				var partyMemberCount = party.GetAliveMemberCount() - 1;
				if (partyMemberCount > 0)
				{
					successRate += 2 * partyMemberCount;
				}
			}

			if (RandomProvider.Next(1, 101) > successRate)
			{
				buff.Vars.SetBool(VarFailed, true);
			}
		}

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (target.IsBuffActive(BuffId.SamsaraAfter_Buff) || target.CheckBoolTempVar("Melia.Skill.Kabbalist.Copied"))
			{
				target.StopBuff(buff.Id);
				return;
			}

			if (buff.Vars.GetBool(VarFailed))
			{
				var caster = buff.Caster as Character;
				var party = caster?.Connection?.Party;
				if (party != null)
				{
					foreach (var member in caster.GetPartyMembersInRange(500))
					{
						member.SystemMessage("SkillSamsaraFail");
					}
				}
				else
				{
					caster?.SystemMessage("SkillSamsaraFail");
				}

				target.StopBuff(buff.Id);
				return;
			}

			if (!buff.Vars.GetBool(VarFirstTick))
			{
				Send.ZC_NORMAL.PlayTextEffect(target, buff.Caster, "SHOW_BUFF_TEXT", (float)buff.Id, null, "Item");
				buff.Vars.SetBool(VarFirstTick, true);
			}
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (buff.Vars.GetBool(VarFailed) || !target.IsDead)
				return;

			if (target.IsBuffActive(BuffId.SamsaraAfter_Buff))
				return;

			var caster = buff.Caster as Character;
			var createCount = 1;

			if (caster == null)
				return;

			if (caster.TryGetActiveAbilityLevel(AbilityId.Chronomancer3, out var abilityLevel)
				&& RandomProvider.Next(1, 101) <= abilityLevel)
			{
				createCount++;
			}

			for (var i = 0; i < createCount; i++)
			{
				if (target is not Mob originalMonster) continue;

				if (!target.Map.TryGetRandomPositionInRange(target.Position, 10, out var position))
					position = target.Position.GetRandomInRange2D(10);

				var clone = Mob.CopyFrom(originalMonster, position);
				if (clone == null) continue;

				clone.Properties.SetFloat(PropertyName.HP, (int)(clone.Properties.GetFloat(PropertyName.MHP) * 0.05f));

				clone.StartBuff(BuffId.SamsaraAfter_Buff, TimeSpan.Zero, caster);
				clone.Vars.SetBool("Melia.Skill.Samsara.Created", true);
			}
		}
	}
}
