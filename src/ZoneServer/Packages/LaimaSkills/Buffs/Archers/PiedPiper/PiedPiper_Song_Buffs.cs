using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Archers.PiedPiper
{
	/// <summary>
	/// Handler for Marschierendeslied, which keeps the target from being
	/// knocked back or down until it has been hit a number of times.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Marschierendeslied_Buff)]
	public class PiedPiper_Marschierendeslied_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler
	{
		private const string HitsTakenVar = "Melia.PiedPiper.MarchHitsTaken";

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
			=> KnockResult.Prevent;

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
			=> KnockResult.Prevent;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Marschierendeslied_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Damage <= 0 || !target.TryGetBuff(BuffId.Marschierendeslied_Buff, out var buff))
				return;

			var hitsTaken = buff.Vars.GetInt(HitsTakenVar) + 1;
			buff.Vars.SetInt(HitsTakenVar, hitsTaken);

			if (hitsTaken >= buff.NumArg2)
				target.StopBuff(BuffId.Marschierendeslied_Buff);
		}
	}

	/// <summary>
	/// Handler for Allegro, which raises movement speed by 15.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Allegro_Buff)]
	public class PiedPiper_Allegro_BuffOverride : BuffHandler
	{
		private const float MoveSpeedBonus = 15f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, MoveSpeedBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}

	/// <summary>
	/// Handler for Hameln Nagetier, which keeps the Pied Piper's mice
	/// around; they leave once it ends.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.HamelnNagetier_Buff)]
	public class PiedPiper_HamelnNagetier_BuffOverride : BuffHandler
	{
		public override void OnEnd(Buff buff)
		{
			PiedPiperSkillHelper.RemoveMice(buff.Target);
		}
	}

	/// <summary>
	/// Handler for Lied des Weltbaum, which raises damage dealt and draws
	/// the attention of nearby monsters to the target.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.LiedDerWeltbaum_Buff)]
	public class PiedPiper_LiedDerWeltbaum_BuffOverride : BuffHandler
	{
		private const float TauntRange = 100f;
		private const int TauntInterval = 1000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(TauntInterval);
		}

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;
			if (target.IsDead)
				return;

			foreach (var monster in target.Map.GetAttackableEnemiesInPosition(target, target.Position, TauntRange).OfType<Mob>())
				monster.InsertHate(target);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.LiedDerWeltbaum_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.LiedDerWeltbaum_Buff, out var buff))
				return;

			modifier.DamageMultiplier += GetCaptionRatio(buff, 1) / 100f;
		}
	}

	/// <summary>
	/// Handler for Lied des Weltbaum's protection, which nullifies the
	/// first 3 attacks the target takes.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.LiedDerWeltbaum_NoDamage_Buff)]
	public class PiedPiper_LiedDerWeltbaum_NoDamage_BuffOverride : BuffHandler
	{
		private const int MaxNullified = 3;
		private const string NullifiedVar = "Melia.PiedPiper.WeltbaumNullified";

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.LiedDerWeltbaum_NoDamage_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Damage <= 0 || !target.TryGetBuff(BuffId.LiedDerWeltbaum_NoDamage_Buff, out var buff))
				return;

			skillHitResult.Damage = 0;

			var nullified = buff.Vars.GetInt(NullifiedVar) + 1;
			buff.Vars.SetInt(NullifiedVar, nullified);

			if (nullified >= MaxNullified)
				target.StopBuff(BuffId.LiedDerWeltbaum_NoDamage_Buff);
		}
	}
}
