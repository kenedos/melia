using System;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizard.Cryomancer
{
	/// <summary>
	/// Handler for the Subzero buff, which changes the target's attribute to
	/// Ice/Water and has a chance to apply Cryomancer_Freeze debuff to 
	/// attackers.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Subzero_Buff)]
	public class Subzero_BuffOverride : BuffHandler, IBuffCombatDefenseAfterCalcHandler, IBuffCombatAttackBeforeBonusesHandler
	{
		private const float BaseFreezeChance = 20f;
		private const float FreezeChancePerLevel = 6f;

		/// <summary>
		/// Changes weapon to ice element
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		public void OnAttackBeforeBonuses(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if ((skill.Data.Attribute == AttributeType.None) || (skill.Data.Attribute == AttributeType.Melee) || (skill.Data.Attribute == AttributeType.Magic))
				modifier.AttackAttribute = AttributeType.Ice;
		}

		/// <summary>
		/// Applies the buff's effect after combat calculations, potentially freezing the attacker.
		/// </summary>
		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Data.ClassType == SkillClassType.Magic)
				return;

			var freezeChance = BaseFreezeChance + (buff.NumArg1 * FreezeChancePerLevel);
			if (RandomProvider.Get().Next(100) < freezeChance)
			{
				var freezeDuration = buff.NumArg2;

				attacker.StartBuff(BuffId.Cryomancer_Freeze, buff.NumArg1, 0, TimeSpan.FromMilliseconds(freezeDuration), buff.Target);
			}
		}
	}
}
