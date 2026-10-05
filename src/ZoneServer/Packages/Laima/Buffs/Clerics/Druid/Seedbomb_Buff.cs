using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Packages.Laima.Skills.Clerics.Druid;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Druid
{
	[Package("laima")]
	[BuffHandler(BuffId.Seedbomb_Buff)]
	public class Seedbomb_BuffOverride : BuffHandler, IBuffOnHitInfoCreatedHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Vars.SetInt(Druid_SeedBombHelper.DetonatedVariable, 0);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead)
				return;

			if (buff.Vars.GetInt(Druid_SeedBombHelper.DetonatedVariable) != 0)
				return;

			if (buff.Caster is not Character caster || caster.IsDead)
				return;

			var skill = this.GetSkill(caster, buff);
			buff.Vars.SetInt(Druid_SeedBombHelper.DetonatedVariable, 1);
			Druid_SeedBombHelper.Detonate(caster, skill, buff.Target.Position, 1);
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (skillHitInfo == null || skillHitInfo.Target != buff.Target)
				return;

			if (skillHitInfo.HitInfo == null || skillHitInfo.HitInfo.Damage <= 0)
				return;

			if (buff.Vars.GetInt(Druid_SeedBombHelper.DetonatedVariable) != 0)
				return;

			if (buff.Caster is not Character caster || caster.IsDead)
				return;

			var skill = this.GetSkill(caster, buff);
			Druid_SeedBombHelper.Trigger(buff, caster, skill, 1);
		}

		private Skill GetSkill(Character caster, Buff buff)
		{
			var skillLevel = System.Math.Max(1, (int)buff.NumArg1);

			if (caster.TryGetSkill(SkillId.Druid_Seedbomb, out var skill))
				return skill;

			return new Skill(caster, SkillId.Druid_Seedbomb, skillLevel);
		}
	}
}
