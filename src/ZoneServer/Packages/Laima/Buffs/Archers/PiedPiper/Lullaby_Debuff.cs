using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Archers.PiedPiper
{
	[Package("laima")]
	[BuffHandler(BuffId.Lullaby_Debuff)]
	public class Lullaby_DebuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler, IBuffOnHitInfoCreatedHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int MaximumLullabyAttacks = 1;
		private const string ReceivedAttackCountVariable = "Wiegenlied.ReceivedAttackCount";
		private const string WokenByAttackVariable = "Wiegenlied.WokenByAttack";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Vars.SetInt(ReceivedAttackCountVariable, 0);
			buff.Vars.SetBool(WokenByAttackVariable, false);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead)
				return;

			buff.Target.RemoveBuff(BuffId.Sleep_Debuff);

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var effectMultiplier = Math.Clamp(buff.NumArg2, 0.5f, 1f);
			var drowsyDuration = TimeSpan.FromSeconds(this.GetDrowsyDuration(skillLevel).TotalSeconds * effectMultiplier);

			buff.Target.StartBuff(BuffId.Wiegenlied_Debuff, skillLevel, effectMultiplier, drowsyDuration, buff.Caster, buff.SkillId);
		}

		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (buff.Target == null || buff.Target.IsDead || target != buff.Target)
				return;

			modifier.ForcedCritical = true;
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (skillHitInfo == null || skillHitInfo.Target != buff.Target)
				return;

			var attackCount = buff.Vars.GetInt(ReceivedAttackCountVariable) + 1;

			buff.Vars.SetInt(ReceivedAttackCountVariable, attackCount);

			if (attackCount < MaximumLullabyAttacks)
				return;

			buff.Vars.SetBool(WokenByAttackVariable, true);
			buff.Target.RemoveBuff(BuffId.Lullaby_Debuff);
		}

		private TimeSpan GetDrowsyDuration(int skillLevel)
		{
			var durationSeconds = 7f + (skillLevel - 1) * (8f / 9f);
			return TimeSpan.FromSeconds(durationSeconds);
		}
	}
}
