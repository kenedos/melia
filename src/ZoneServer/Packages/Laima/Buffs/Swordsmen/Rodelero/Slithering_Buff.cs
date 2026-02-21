using System;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for the Slithering Buff.
	/// Increases block and provides a chance for enemy attacks to miss.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Slithering_Buff)]
	public class Slithering_BuffOverride : BuffHandler, IBuffCombatDefenseAfterCalcHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			if (buff.Caster is not ICombatEntity caster)
				return;

			var skillLevel = 1;
			if (caster.TryGetSkill(SkillId.Rodelero_Slithering, out var skill))
				skillLevel = skill.Level;

			buff.NumArg1 = skillLevel;

			var flatBlock = 300f + 15f * skillLevel;
			var blockRate = 0.20f + 0.01f * skillLevel;

			AddPropertyModifier(buff, target, PropertyName.BLK_BM, flatBlock);
			AddPropertyModifier(buff, target, PropertyName.BLK_RATE_BM, blockRate);
			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -10);

			target.Properties.Modify(PropertyName.Jumpable, -1);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is Character character)
			{
				character.ModifyStamina(-500);
			}
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.BLK_BM);
			RemovePropertyModifier(buff, target, PropertyName.BLK_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);

			target.Properties.Modify(PropertyName.Jumpable, 1);
		}

		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Data.ClassType != SkillClassType.Magic)
				return;

			var skillLevel = buff.NumArg1;
			var missChance = 40f + 4f * skillLevel;

			var roll = RandomProvider.Next(1, 100);
			if (roll <= missChance)
			{
				skillHitResult.Damage = 0;
				skillHitResult.Result = HitResultType.Miss;
			}
		}
	}
}
