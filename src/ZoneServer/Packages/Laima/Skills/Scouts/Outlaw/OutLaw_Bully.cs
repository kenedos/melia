using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Scouts.OutLaw
{
	/// <summary>
	/// Handler for the Outlaw skill Bully.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.OutLaw_Bully)]
	public class OutLaw_BullyOverride : ISelfSkillHandler
	{
		/// <summary>
		/// Handles skill, applying a buff to the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="dir"></param>
		/// <exception cref="NotImplementedException"></exception>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (caster is Character character
				&& character.IsWearingArmorOfType(ArmorMaterialType.Iron))
			{
				caster.ServerMessage(Localization.Get("Can't use while wearing [Plate] armor."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			// Uses a totally different buff if Outlaw19 is active
			if (caster.IsAbilityActive(AbilityId.Outlaw19))
			{
				caster.StartBuff(BuffId.BullyPainBarrier_Buff, skill.Level, 0, TimeSpan.FromSeconds(20), caster);
			}
			else
			{
				caster.StartBuff(BuffId.Bully_Buff, skill.Level, 0, TimeSpan.FromSeconds(60), caster);
			}

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);
		}
	}
}
