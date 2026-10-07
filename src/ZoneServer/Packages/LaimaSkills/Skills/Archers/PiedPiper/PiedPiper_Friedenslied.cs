using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.PiedPiper
{
	/// <summary>
	/// Handler for the Pied Piper skill Friedenslied, which makes allies
	/// and enemies dance while the Pied Piper plays, for up to 5 seconds.
	/// Dancing allies take no damage, dancing enemies lose a buff.
	/// </summary>
	/// <remarks>
	/// With [Arts] Friedenslied: SmileClap only enemies are affected, each
	/// with a 50% chance to dance.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.PiedPiper_Friedenslied)]
	public class PiedPiper_FriedensliedOverride : IDynamicCasted
	{
		private const int SmileClapChance = 50;
		private const string DancersVar = "Melia.PiedPiper.Dancers";
		private static readonly TimeSpan MaxDuration = TimeSpan.FromSeconds(5);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);
			var isSmileClap = caster.IsAbilityActive(AbilityId.PiedPiper22);
			var dancers = new List<(ICombatEntity Target, BuffId BuffId)>();

			foreach (var enemy in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, PiedPiperSkillHelper.SongRange).Take(maxTargets))
			{
				if (isSmileClap && GameRandom.Get().Next(100) >= SmileClapChance)
					continue;

				var buffId = isSmileClap ? BuffId.Friedenslied_AbilDance_Debuff : BuffId.Friedenslied_Debuff;
				enemy.StartBuff(buffId, skill.Level, 0, MaxDuration, caster, skill.Id);
				dancers.Add((enemy, buffId));
			}

			if (!isSmileClap)
			{
				foreach (var ally in PartySkillHelper.GetAlliesInRange(caster, caster.Position, PiedPiperSkillHelper.SongRange).Where(a => a != caster).Take(maxTargets))
				{
					ally.StartBuff(BuffId.Friedenslied_Buff, skill.Level, 0, MaxDuration, caster, skill.Id);
					dancers.Add((ally, BuffId.Friedenslied_Buff));
				}
			}

			skill.Vars.Set(DancersVar, dancers);

			PiedPiperSkillHelper.SummonMouse(caster);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (skill.Vars.TryGet<List<(ICombatEntity Target, BuffId BuffId)>>(DancersVar, out var dancers))
			{
				foreach (var (target, buffId) in dancers)
					target.StopBuff(buffId);
			}

			skill.Vars.Remove(DancersVar);
		}
	}
}
