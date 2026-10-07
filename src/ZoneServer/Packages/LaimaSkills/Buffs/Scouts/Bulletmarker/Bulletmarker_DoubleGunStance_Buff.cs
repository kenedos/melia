using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for Double Gun Stance, which swaps the basic attack to the
	/// double pistol shot, whose hits build Overheating.
	/// </summary>
	/// <remarks>
	/// The double pistol shot's factor is the stance's, see
	/// SCR_Get_SkillFactor_DoubleGun_Attack.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.DoubleGunStance_Buff)]
	public class Bulletmarker_DoubleGunStance_BuffOverride : BuffHandler
	{
		private const int BasicAttackOverheating = 1;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			if (!character.Skills.Has(SkillId.DoubleGun_Attack))
				character.Skills.Add(new Skill(character, SkillId.DoubleGun_Attack));

			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.DoubleGun_Attack);
			Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.None);

			if (character.Components.TryGet<SkillComponent>(out var skillComponent))
				skillComponent.InvalidateAll();
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.None);
			Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.None);

			character.StopBuff(BuffId.Overheating_Buff);
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.DoubleGunStance_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Id != SkillId.DoubleGun_Attack || skillHitResult.Damage <= 0)
				return;

			BulletMarkerSkillHelper.AddOverheating(attacker, BasicAttackOverheating);
		}
	}
}
