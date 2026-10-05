using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Concentration increases Accuracy and Critical Rate.
	/// It also reveals nearby cloaked enemies.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Concentration_Buff)]
	public class Concentration_Buff : BuffHandler
	{
		private const float BaseBonus = 0.25f;
		private const float BonusPerLevel = 0.05f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = this.GetRateBonus(buff);

			AddPropertyModifier(buff, buff.Target, PropertyName.HR_RATE_BM, bonus);
			AddPropertyModifier(buff, buff.Target, PropertyName.CRTHR_RATE_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_RATE_BM);
		}

		public override void WhileActive(Buff buff)
		{
			var targets = buff.Target.Map
				.GetAttackableEnemiesInPosition(buff.Target, buff.Target.Position, 100)
				.Where(target => target.IsBuffActiveByKeyword(BuffTag.Cloaking))
				.ToList();

			foreach (var target in targets)
				target.StopBuffByTag(BuffTag.Cloaking);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.Concentration_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Concentration_Buff, out var buff))
				return;

			if (buff.Target.TryGetActiveAbilityLevel(AbilityId.Archer39, out _))
				modifier.ForcedHit = true;
		}

		private float GetRateBonus(Buff buff)
		{
			var skillLevel = buff.NumArg1;
			return BaseBonus + skillLevel * BonusPerLevel;
		}
	}
}
