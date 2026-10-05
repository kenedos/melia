using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Shinobi
{
	/// <summary>
	/// Handler for Shinobi skill Mokuton no Jutsu.
	/// SkillId: 50503
	///
	/// Move Speed: +10.
	/// Damage Reduction: 18% at Lv1 to 50% at Lv10.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Shinobi_Mokuton_no_jutsu)]
	public class Shinobi_MokutonNoJutsu : IGroundSkillHandler
	{
		private static readonly TimeSpan BuffDuration = TimeSpan.FromSeconds(8);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			this.ApplyMokuton(character, skill);

			caster.SetAttackState(false);
		}

		private void ApplyMokuton(Character character, Skill skill)
		{
			character.StartBuff(BuffId.Mokuton_no_jutsu, skill.Level, 0f, BuffDuration, character, skill.Id);
		}
	}
}
