using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler for the Mackangdal buff.
	/// Redirects damage taken to caster's summons.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Mackangdal_Buff)]
	public class Mackangdal_BuffOverride : BuffHandler, IBuffCombatDefenseAfterCalcHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public override void WhileActive(Buff buff)
		{
		}

		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!(target is Character character))
				return;

			if (!character.Skills.TryGet(SkillId.Bokor_Mackangdal, out var mackangdalSkill))
				return;

			var summonsList = character.Summons.GetSummons().Where(s => !s.IsDead).ToList();
			if (summonsList.Count == 0)
				return;

			var redirectRate = 0.40f + (0.03f * mackangdalSkill.Level);
			var damageToRedirect = skillHitResult.Damage * redirectRate;
			var damagePerSummon = damageToRedirect / summonsList.Count;
			var totalRedirectedDamage = 0f;

			foreach (var summon in summonsList)
			{
				var summonCurrentHp = summon.Hp;
				var damageToSummon = Math.Min(damagePerSummon, summonCurrentHp);

				summon.TakeDamage(damageToSummon, attacker);

				var hitInfo = new HitInfo(attacker, summon, damageToSummon, HitResultType.Hit);
				Send.ZC_HIT_INFO(attacker, summon, hitInfo);

				totalRedirectedDamage += damageToSummon;
			}

			skillHitResult.Damage -= totalRedirectedDamage;
		}
	}
}
