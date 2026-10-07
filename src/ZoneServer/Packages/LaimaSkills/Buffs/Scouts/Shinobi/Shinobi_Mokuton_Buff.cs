using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Shinobi
{
	/// <summary>
	/// Handler for Mokuton no Jutsu, which raises movement speed by 10 and
	/// lowers damage taken by the skill's ratio for as long as the Shinobi
	/// stays hidden.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Mokuton_no_jutsu)]
	public class Shinobi_Mokuton_BuffOverride : BuffHandler
	{
		private const float MoveSpeedBonus = 10f;
		private const float MaxDamageReduction = 0.9f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			UpdatePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, MoveSpeedBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Mokuton_no_jutsu)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target.TryGetBuff(BuffId.Mokuton_no_jutsu, out var buff))
				skillHitResult.Damage *= Math.Max(1 - MaxDamageReduction, 1 - GetCaptionRatio(buff, 1) / 100f);
		}
	}

	/// <summary>
	/// Handler for the Shinobi's stealth, which breaks when the Shinobi is
	/// hit and takes Mokuton no Jutsu with it.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.ShinobiCloaking_Buff)]
	public class Shinobi_ShinobiCloaking_BuffOverride : BuffHandler
	{
		public override void OnEnd(Buff buff)
		{
			buff.Target.StopBuff(BuffId.Mokuton_no_jutsu);
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.ShinobiCloaking_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Damage > 0)
				target.StopBuff(BuffId.ShinobiCloaking_Buff);
		}
	}
}
