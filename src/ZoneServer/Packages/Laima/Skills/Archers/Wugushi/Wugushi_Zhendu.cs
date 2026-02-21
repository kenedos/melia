using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Archers.Wugushi
{
	/// <summary>
	/// Handler for the Wugushi skill Zhendu.
	/// Applies Zhendu buff to caster and party members.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Wugushi_Zhendu)]
	public class Wugushi_ZhenduOverride : ISelfSkillHandler
	{
		private const float BuffRange = 300f;
		private const int BuffDurationSeconds = 300;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, dir, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, Position.Zero, ForceId.GetNew(), null);

			caster.StartBuff(BuffId.Zhendu_Buff, skill.Level, 0f, TimeSpan.FromSeconds(BuffDurationSeconds), caster);

			if (caster is Character character)
			{
				var party = character.Connection.Party;
				if (party != null)
				{
					var members = caster.Map.GetPartyMembersInRange(character, BuffRange, true);
					foreach (var member in members)
					{
						if (member == caster)
							continue;
						member.StartBuff(BuffId.Zhendu_Buff, skill.Level, 0f, TimeSpan.FromSeconds(BuffDurationSeconds), caster);
					}
				}
			}
		}
	}
}
