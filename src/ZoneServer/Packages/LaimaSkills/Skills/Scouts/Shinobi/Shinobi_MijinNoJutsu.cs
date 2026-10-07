using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Shinobi
{
	/// <summary>
	/// Handler for the Shinobi skill Mijin no Jutsu, which hides the
	/// Shinobi and blows up the gunpowder they leave behind, 3% harder per
	/// stack of Ninjutsu: Baku, which it spends.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Shinobi_Mijin_no_jutsu)]
	public class Shinobi_MijinNoJutsuOverride : IGroundSkillHandler
	{
		private const int HitCount = 6;
		private const float Range = 70f;
		private const float DamagePerBaku = 0.03f;
		private static readonly TimeSpan HideDelay = TimeSpan.FromMilliseconds(1000);
		private static readonly TimeSpan FuseTime = TimeSpan.FromMilliseconds(1500);
		private static readonly TimeSpan StealthDuration = TimeSpan.FromSeconds(8);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(Detonate(skill, caster));
			ShinobiSkillHelper.ReplicateOnClones(caster, skill.Id, Detonate);
		}

		/// <summary>
		/// Hides the attacker and blows up the gunpowder where they stood.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="attacker"></param>
		/// <returns></returns>
		public static async Task Detonate(Skill skill, ICombatEntity attacker)
		{
			var position = attacker.Position;

			await skill.Wait(HideDelay);

			if (attacker is not DummyCharacter)
				attacker.StartBuff(BuffId.ShinobiCloaking_Buff, skill.Level, 0, StealthDuration, attacker, skill.Id);

			await skill.Wait(FuseTime);

			Explode(skill, attacker, position);
		}

		/// <summary>
		/// Damages the enemies around the position with the attacker's
		/// gunpowder.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="attacker"></param>
		/// <param name="position"></param>
		public static void Explode(Skill skill, ICombatEntity attacker, Position position)
		{
			if (attacker.Map == null)
				return;

			var owner = attacker is DummyCharacter clone ? clone.Owner : attacker;
			var baku = owner?.GetOverbuffCount(BuffId.Bunshin_Stack_Buff) ?? 0;
			var hits = new List<SkillHitInfo>();

			foreach (var target in attacker.Map.GetAttackableEnemiesInPosition(attacker, position, Range).LimitBySDR(attacker, skill))
			{
				var modifier = SkillModifier.MultiHit(HitCount);
				modifier.DamageMultiplier += baku * DamagePerBaku;

				var skillHitResult = SCR_SkillHit(attacker, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, attacker);

				hits.Add(new SkillHitInfo(attacker, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(attacker, hits);

			if (attacker is not DummyCharacter)
				attacker.StopBuff(BuffId.Bunshin_Stack_Buff);
		}
	}
}
