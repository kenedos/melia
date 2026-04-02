using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the WideMiasma Debuff, which ticks poison damage
	/// and gives all poison debuffs on the target a 20% chance to
	/// spread to a nearby enemy when struck.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.WideMiasma_Debuff)]
	public class WideMiasma_DebuffOverride : DamageOverTimeBuffHandler
	{
		private const float SpreadRange = 50f;
		private const float SpreadChance = 20f;

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Poison;
		}

		/// <summary>
		/// When the target is struck, each poison debuff on the target
		/// has a 20% chance to spread to 1 nearby enemy.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.WideMiasma_Debuff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.WideMiasma_Debuff, out var buff))
				return;

			if (buff.Caster is not ICombatEntity caster)
				return;

			if (target.IsDead)
				return;

			var map = target.Map;
			if (map == null)
				return;

			var buffs = target.Components.Get<BuffComponent>();
			if (buffs == null)
				return;

			var poisonBuffs = buffs.GetAll(b => b.Data.Tags.HasAny(BuffTag.Poison));
			if (poisonBuffs.Count == 0)
				return;

			foreach (var poisonBuff in poisonBuffs)
			{
				if (RandomProvider.Get().Next(100) >= SpreadChance)
					continue;

				var poisonCaster = poisonBuff.Caster as ICombatEntity ?? caster;

				var nearbyEnemy = map.GetAttackableEnemiesIn(poisonCaster, new Circle(target.Position, SpreadRange))
					.Where(e => e != target && !e.IsBuffActive(poisonBuff.Id))
					.FirstOrDefault();

				if (nearbyEnemy == null)
					continue;

				nearbyEnemy.StartBuff(poisonBuff.Id, poisonBuff.NumArg1, poisonBuff.NumArg2, poisonBuff.RemainingDuration, poisonCaster, poisonBuff.SkillId);
			}
		}
	}
}
