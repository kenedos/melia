using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.Appraiser
{
	/// <summary>
	/// Handler for the Appraiser skill Triplet Lense, which gives the
	/// Appraiser five lenses that attach to nearby enemies.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Appraiser_TripletLens)]
	public class Appraiser_TripletLensOverride : IGroundSkillHandler
	{
		private const int LensCount = 5;

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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, originPos.GetDirection(farPos), farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			caster.StopBuff(BuffId.TripletLens_Buff);

			for (var i = 0; i < LensCount; i++)
				caster.StartBuff(BuffId.TripletLens_Buff, skill.Level, 0, TimeSpan.Zero, caster, skill.Id);
		}
	}
}
