using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Serial Bullet buff, which adds damage to pistol
	/// attacks and builds a stack with every hit until it runs out.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DoubleBullet_Buff)]
	public class SchwarzerReiter_DoubleBullet_BuffOverride : BuffHandler
	{
		private const int MaxStacks = 60;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.DoubleBullet_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.DoubleBullet_Buff, out var buff))
				return;

			if (skillHitResult.Result != HitResultType.Hit || skillHitResult.Damage <= 0)
				return;

			if (!IsPistolAttack(attacker))
				return;

			skillHitResult.Damage *= 1f + GetCaptionRatio(buff, 1) / 100f;

			buff.OverbuffCounter++;
			buff.NotifyUpdate();

			if (buff.OverbuffCounter < MaxStacks)
				return;

			attacker.StopBuff(BuffId.DoubleBullet_Buff);

			if (attacker.TryGetSkill(SkillId.Schwarzereiter_DoubleBullet, out var doubleBullet))
				attacker.Components.Get<CooldownComponent>().Start(doubleBullet);
		}

		/// <summary>
		/// Returns whether the attacker is wielding a pistol.
		/// </summary>
		/// <param name="attacker"></param>
		private static bool IsPistolAttack(ICombatEntity attacker)
		{
			if (attacker.TryGetEquipItem(EquipSlot.RightHand, out var rightHand) && rightHand.Data.EquipType1 == EquipType.Pistol)
				return true;

			return attacker.TryGetEquipItem(EquipSlot.LeftHand, out var leftHand) && leftHand.Data.EquipType1 == EquipType.Pistol;
		}
	}
}
