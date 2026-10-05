using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Scouts.Shinobi
{
	/// <summary>
	/// Handler for Mokuton no Jutsu.
	/// Move Speed: +10.
	/// Damage Reduction: 18% at Lv1 to 50% at Lv10.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Mokuton_no_jutsu)]
	public class Shinobi_MokutonNoJutsu_Buff : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		private const float MoveSpeedBonus = 10f;
		private const float BaseDamageReduction = 0.18f;
		private const float MaxDamageReduction = 0.50f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			character.Properties.Modify(PropertyName.MSPD_BM, MoveSpeedBonus);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			character.Properties.Modify(PropertyName.MSPD_BM, -MoveSpeedBonus);
		}

		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target != buff.Target)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, 10);
			var damageReduction = this.GetDamageReduction(skillLevel);

			modifier.DamageMultiplier -= damageReduction;

			if (modifier.DamageMultiplier < 0f)
				modifier.DamageMultiplier = 0f;
		}

		private float GetDamageReduction(int skillLevel)
		{
			if (skillLevel >= 10)
				return MaxDamageReduction;

			return BaseDamageReduction + ((skillLevel - 1) * ((MaxDamageReduction - BaseDamageReduction) / 9f));
		}
	}
}
