using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Archers.PiedPiper
{
	/// <summary>
	/// Handler for Hypnotische Floete on the Pied Piper, who can't be
	/// knocked back or down while playing.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Fluting_Buff)]
	public class PiedPiper_Fluting_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler
	{
		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
			=> KnockResult.Prevent;

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
			=> KnockResult.Prevent;
	}

	/// <summary>
	/// Handler for Hypnotische Floete on monsters, which follow the Pied
	/// Piper without attacking and can't evade or block. They are confused
	/// for 3 seconds once it ends.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Fluting_DeBuff)]
	public class PiedPiper_Fluting_DeBuffOverride : BuffHandler
	{
		private const float FollowDistance = 20f;
		private static readonly TimeSpan ConfusionDuration = TimeSpan.FromSeconds(3);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Fluting);
		}

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead || buff.Caster is not ICombatEntity piper || piper.IsDead || piper.Map != target.Map)
			{
				target.StopBuff(BuffId.Fluting_DeBuff);
				return;
			}

			if (!target.Position.InRange2D(piper.Position, FollowDistance))
				target.MoveTo(piper.Position, target.Properties.GetFloat(PropertyName.MSPD), suspendAI: true);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			target.RemoveState(StateType.Fluting);

			if (!target.IsDead)
				target.StartBuff(BuffId.Confuse, buff.NumArg1, 0, ConfusionDuration, buff.Caster, buff.SkillId);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Fluting_DeBuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.IsBuffActive(BuffId.Fluting_DeBuff))
				return;

			modifier.ForcedHit = true;
			modifier.Unblockable = true;
		}
	}
}
