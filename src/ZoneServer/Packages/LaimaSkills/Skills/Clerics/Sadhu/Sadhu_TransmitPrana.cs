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
	[Package("laima-skills")]
	[SkillHandler(SkillId.Sadhu_TransmitPrana)]
	public class Sadhu_TransmitPranaOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float BuffRange = 300;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var damageMultiplierIncrease = skill.Properties.GetFloat(PropertyName.CaptionRatio) / 100f;

			caster.StartBuff(BuffId.TransmitPrana_Buff, skill.Level, damageMultiplierIncrease, skill.Properties.CaptionTime, caster, skill.Id);

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
						member.StartBuff(BuffId.TransmitPrana_Buff, skill.Level, damageMultiplierIncrease, skill.Properties.CaptionTime, caster, skill.Id);
					}
				}
			}
		}
	}
}
