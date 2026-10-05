using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Wizards.Onmyoji
{
	[Package("laima")]
	[BuffHandler(BuffId.GreenwoodShikigami_Debuff)]
	public class Onmyoji_GreenwoodShikigami_DebuffOverride : BuffHandler
	{
		private const string MovementSpeedProperty = PropertyName.MSPD_BM;
		private const float MovementSpeedReductionRate = 0.50f;
		private const int PoisonIntervalMilliseconds = 900;

		public override void OnStart(Buff buff)
		{
			buff.SetUpdateTime(PoisonIntervalMilliseconds);
			var movementSpeed = buff.Target.Properties.GetFloat(PropertyName.MSPD);
			AddPropertyModifier(buff, buff.Target, MovementSpeedProperty, -(movementSpeed * MovementSpeedReductionRate));
			Send.ZC_MSPD(buff.Target);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target.IsDead || buff.Caster is not ICombatEntity caster || !caster.IsAbilityActive(AbilityId.Onmyoji5))
				return;

			if (!caster.TryGetSkill(buff.SkillId, out var skill) && !caster.TryGetSkill(SkillId.Onmyoji_GreenwoodShikigami, out skill))
				return;

			var skillHitResult = SCR_SkillHit(caster, buff.Target, skill);
			buff.Target.TakeDamage(skillHitResult.Damage, caster);
			var hitInfo = new HitInfo(caster, buff.Target, skillHitResult.Damage, skillHitResult.Result);
			Send.ZC_HIT_INFO(caster, buff.Target, hitInfo);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, MovementSpeedProperty);
			Send.ZC_MSPD(buff.Target);
		}
	}
}
