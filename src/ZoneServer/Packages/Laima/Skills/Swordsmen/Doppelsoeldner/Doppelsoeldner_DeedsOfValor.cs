using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.HandlersOverrides.Swordsmen.Doppelsoeldner
{
	/// <summary>
	/// Handler for the Doppelsoeldner skill Deeds of Valor.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Doppelsoeldner_DeedsOfValor)]
	public class Doppelsoeldner_DeedsOfValorOverride : ISelfSkillHandler
	{
		public const float BaseDamageMultiplier = 1.15f;
		public const float DamageMultiplierPerLevel = 0.04f;

		/// <summary>
		/// Handles skill, applying the buff to the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="dir"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var target = caster;

			var duration = TimeSpan.FromMinutes(5);
			var damage = BaseDamageMultiplier + DamageMultiplierPerLevel * skill.Level;

			target.StartBuff(BuffId.DeedsOfValor, skill.Level, damage, duration, caster, skill.Id);

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, target, null);
		}
	}
}
