using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.PiedPiper
{
	/// <summary>
	/// Handler for the Pied Piper skill Stegreifspiel, which plays one of
	/// the Pied Piper's learned songs at random without touching its
	/// cooldown.
	/// </summary>
	/// <remarks>
	/// Each Stegreifspiel arts locks the song it plays to one of them.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.PiedPiper_Improvisation)]
	public class PiedPiper_ImprovisationOverride : ISelfSkillHandler
	{
		private static readonly (AbilityId Arts, SkillId SkillId, Action<ICombatEntity, Skill> Play)[] Songs =
		[
			(AbilityId.PiedPiper20, SkillId.PiedPiper_Dissonanz, PiedPiperSkillHelper.PlayDissonanz),
			(AbilityId.PiedPiper19, SkillId.PiedPiper_Wiegenlied, PiedPiperSkillHelper.PlayWiegenlied),
			(AbilityId.PiedPiper17, SkillId.PiedPiper_Marschierendeslied, PiedPiperSkillHelper.PlayMarschierendeslied),
			(AbilityId.PiedPiper18, SkillId.PiedPiper_LiedDerWeltbaum, PiedPiperSkillHelper.PlayLiedDerWeltbaum),
		];

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			var learned = Songs.Where(s => caster.TryGetSkill(s.SkillId, out _)).ToList();
			var forced = learned.Where(s => caster.IsAbilityActive(s.Arts)).ToList();
			var candidates = forced.Count > 0 ? forced : learned;

			if (candidates.Count == 0)
			{
				caster.ServerMessage(Localization.Get("You haven't learned any songs to improvise."));
				Send.ZC_SKILL_CAST_CANCEL(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, ForceId.GetNew(), null);

			var song = candidates[GameRandom.Get().Next(candidates.Count)];
			caster.TryGetSkill(song.SkillId, out var songSkill);

			song.Play(caster, songSkill);
		}
	}
}
