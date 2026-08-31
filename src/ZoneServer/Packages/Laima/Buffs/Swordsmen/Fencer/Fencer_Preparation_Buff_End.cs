using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the buff Preparation leaves behind once it turned an
	/// attack away, which empowers every hit of the next rapier skill and is
	/// then consumed.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Preparation_Buff_End)]
	public class Fencer_Preparation_Buff_EndOverride : BuffHandler, IBuffOnSkillUseHandler
	{
		private const string EmpoweredSkillVar = "Melia.Preparation.EmpoweredSkill";

		public override void WhileActive(Buff buff)
		{
			Fencer_RapierGuard.EndWithoutRapier(buff);
		}

		public void OnSkillUse(Buff buff, ICombatEntity caster, Skill skill)
		{
			if (buff.Vars.TryGetInt(EmpoweredSkillVar, out _))
			{
				caster.StopBuff(BuffId.Preparation_Buff_End);
				return;
			}

			buff.Vars.SetInt(EmpoweredSkillVar, (int)skill.Id);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.Preparation_Buff_End)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Preparation_Buff_End, out var buff))
				return;

			if (!Fencer_RapierGuard.HasRapier(attacker))
				return;

			if (!buff.Vars.TryGetInt(EmpoweredSkillVar, out var empoweredSkillId) || empoweredSkillId != (int)skill.Id)
				return;

			skillHitResult.Damage *= 1f + GetCaptionRatio(buff, 1) / 100f;
		}
	}
}
