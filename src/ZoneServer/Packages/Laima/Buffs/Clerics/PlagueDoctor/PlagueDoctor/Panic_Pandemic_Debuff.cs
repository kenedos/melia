using System;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.PlagueDoctor
{
	/// <summary>
	/// Increases damage received from the Pandemic caster by 20% per active debuff, up to 100%.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Panic_Pandemic_Debuff)]
	public class Panic_Pandemic_DebuffOverride : BuffHandler, IBuffOnHitInfoCreatedHandler
	{
		private const float DamageBonusPerDebuff = 0.20f;
		private const float MaximumDamageBonus = 1f;

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (skillHitInfo.Attacker == null || buff.Caster == null)
				return;

			if (skillHitInfo.Attacker.Handle != buff.Caster.Handle)
				return;

			var buffComponent = buff.Target.Components.Get<BuffComponent>();

			if (buffComponent == null)
				return;

			var debuffCount = buffComponent.GetList().Count(activeBuff => activeBuff.Data.Type == BuffType.Debuff);
			var damageBonus = Math.Min(debuffCount * DamageBonusPerDebuff, MaximumDamageBonus);

			if (damageBonus <= 0)
				return;

			skillHitInfo.HitInfo.Damage *= 1f + damageBonus;
		}
	}
}
