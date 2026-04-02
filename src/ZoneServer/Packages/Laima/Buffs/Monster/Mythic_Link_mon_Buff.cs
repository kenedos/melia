using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Laima.Monster
{
	/// <summary>
	/// Handler for Mythic_Link_mon_Buff.
	/// Shares damage received among all linked monsters.
	/// Works like Physical Link - full damage shared to all linked targets.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Mythic_Link_mon_Buff)]
	public class Mythic_Link_mon_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		/// <summary>
		/// Shares damage among all linked monsters.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Mythic_Link_mon_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Mythic_Link_mon_Buff, out var buff))
				return;

			if (!buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
				return;

			if (target.Map == null)
				return;

			var sharedDamage = skillHitResult.Damage;

			foreach (var handle in memberHandles)
			{
				if (!target.Map.TryGetCombatEntity(handle, out var member))
					continue;

				if (member.IsDead || member.Handle == target.Handle)
					continue;

				member.TakeDamage(sharedDamage, attacker);

				var skillId = skill?.Id ?? SkillId.Normal_Attack;
				var hitType = skill != null ? skill.Data.HitType : SkillHitType.Melee;
				var hitInfo = new HitInfo(attacker, member, skillId, sharedDamage, hitType, HitResultType.Hit);
				Send.ZC_HIT_INFO(attacker, member, hitInfo);
			}
		}
	}
}
