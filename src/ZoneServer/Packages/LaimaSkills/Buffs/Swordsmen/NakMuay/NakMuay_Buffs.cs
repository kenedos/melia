using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.NakMuay
{
	/// <summary>
	/// Handler for Muay Thai, which raises the final damage of Nak Muay
	/// skills by the skill's ratio in percent.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.MuayThai_Abil_Buff)]
	public class NakMuay_MuayThai_Abil_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.MuayThai_Abil_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!skill.Data.ClassName.StartsWith("NakMuay_", StringComparison.Ordinal))
				return;

			if (attacker.TryGetBuff(BuffId.MuayThai_Abil_Buff, out var buff))
				modifier.FinalDamageMultiplier += GetCaptionRatio(buff, 1) / 100f;
		}
	}

	/// <summary>
	/// Handler for Te Kha's Stiff Legs, which roots the target.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.TeKha_Debuff)]
	public class NakMuay_TeKha_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Held, buff.Duration);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Held);
		}
	}

	/// <summary>
	/// Handler for Sok Chiang's bleeding, which deals a tenth of the elbow
	/// slashes' damage every second.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: Snapshotted damage per tick
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.SokChiang_Debuff)]
	public class NakMuay_SokChiang_DebuffOverride : DamageOverTimeBuffHandler
	{
		private const int TickInterval = 1000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			base.OnActivate(buff, activationType);
			buff.SetUpdateTime(TickInterval);
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Bleeding;
		}
	}
}
