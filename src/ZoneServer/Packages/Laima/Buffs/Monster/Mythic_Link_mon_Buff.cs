using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
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
	public class Mythic_Link_mon_BuffOverride : BuffHandler, IBuffCombatDefenseAfterCalcHandler
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
		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
				return;

			if (target.Map == null)
				return;

			var sharedDamage = (int)skillHitResult.Damage;

			foreach (var handle in memberHandles)
			{
				if (!target.Map.TryGetCombatEntity(handle, out var member))
					continue;

				if (member.IsDead || member.Handle == target.Handle)
					continue;

				member.TakeSimpleHit(sharedDamage, attacker, skill?.Id ?? SkillId.Normal_Attack);
			}
		}
	}
}
