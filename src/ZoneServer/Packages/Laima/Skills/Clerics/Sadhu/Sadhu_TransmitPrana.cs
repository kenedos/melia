using System;
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

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu skill Transmit Prana.
	/// Enchants party members' attacks with Psychokinesis element and increases damage.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_TransmitPrana)]
	public class Sadhu_TransmitPranaOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const float BuffRange = 300;
		private const int BuffDurationSeconds = 300;
		private const float BaseDamageMultiplierIncrease = 0.10f;
		private const float DamageMultiplierIncreasePerLevel = 0.02f;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var damageMultiplierIncrease = BaseDamageMultiplierIncrease + skill.Level * DamageMultiplierIncreasePerLevel;

			caster.StartBuff(BuffId.TransmitPrana_Buff, skill.Level, damageMultiplierIncrease, TimeSpan.FromSeconds(BuffDurationSeconds), caster);

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
						member.StartBuff(BuffId.TransmitPrana_Buff, skill.Level, damageMultiplierIncrease, TimeSpan.FromSeconds(BuffDurationSeconds), caster);
					}
				}
			}
		}
	}
}
