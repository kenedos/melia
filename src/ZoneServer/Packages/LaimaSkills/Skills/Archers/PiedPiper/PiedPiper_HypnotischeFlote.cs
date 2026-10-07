using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.PiedPiper
{
	/// <summary>
	/// Handler for the Pied Piper skill Hypnotische Floete, which makes
	/// regular monsters around the Pied Piper follow them while they play,
	/// for up to 10 seconds.
	/// </summary>
	/// <remarks>
	/// With Hypnotische Floete: Elite it affects elite monsters as well.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.PiedPiper_HypnotischeFlote)]
	public class PiedPiper_HypnotischeFloteOverride : IDynamicCasted
	{
		private const string HypnotizedVar = "Melia.PiedPiper.Hypnotized";
		private static readonly TimeSpan MaxDuration = TimeSpan.FromSeconds(10);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var allowElite = caster.IsAbilityActive(AbilityId.PiedPiper6);
			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);

			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, PiedPiperSkillHelper.SongRange)
				.Where(e => e.Rank == MonsterRank.Normal || (allowElite && e.Rank == MonsterRank.Elite))
				.Take(maxTargets)
				.ToList();

			caster.StartBuff(BuffId.Fluting_Buff, skill.Level, 0, MaxDuration, caster, skill.Id);

			foreach (var target in targets)
				target.StartBuff(BuffId.Fluting_DeBuff, skill.Level, 0, MaxDuration, caster, skill.Id);

			skill.Vars.Set(HypnotizedVar, targets);

			PiedPiperSkillHelper.SummonMouse(caster);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopBuff(BuffId.Fluting_Buff);

			if (skill.Vars.TryGet<List<ICombatEntity>>(HypnotizedVar, out var targets))
			{
				foreach (var target in targets)
					target.StopBuff(BuffId.Fluting_DeBuff);
			}

			skill.Vars.Remove(HypnotizedVar);
		}
	}
}
