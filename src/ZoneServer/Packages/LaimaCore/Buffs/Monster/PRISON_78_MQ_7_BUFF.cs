using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Laima.Monster
{
	/// <summary>
	/// Handler for PRISON_78_MQ_7_BUFF, the Demon Barrier protecting
	/// Mandara in the Kalejimas Visiting Room, which turns aside nearly
	/// all damage until a Magic Control Scroll suppresses it.
	/// </summary>
	[Package("laima-core")]
	[BuffHandler(BuffId.PRISON_78_MQ_7_BUFF)]
	public class PRISON_78_MQ_7_BUFFOverride : BuffHandler
	{
		private const float DamageTakenRate = 0.1f;

		/// <summary>
		/// Reduces the damage the barrier's bearer takes.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.PRISON_78_MQ_7_BUFF)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.IsBuffActive(BuffId.PRISON_78_MQ_7_BUFF))
				return;

			skillHitResult.Damage *= DamageTakenRate;
		}
	}
}
