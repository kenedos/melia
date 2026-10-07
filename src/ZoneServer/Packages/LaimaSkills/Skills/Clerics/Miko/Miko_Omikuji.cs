using System;
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

namespace Melia.Zone.Skills.Handlers.Clerics.Miko
{
	/// <summary>
	/// Handler for the Miko skill Omikuji, which draws a lot for each of up
	/// to 5 allies around the Miko, blessing them with Honor, Hope, Safety or
	/// Health.
	/// </summary>
	/// <remarks>
	/// [Arts] Omikuji: Financial Fortune adds Financial Fortune to the lots,
	/// which comes with Great Blessing: Lucky Draw.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Miko_Omikuji)]
	public class Miko_OmikujiOverride : IGroundSkillHandler
	{
		private const float Range = 160f;
		private const int MaxTargets = 5;
		private static readonly TimeSpan FortuneDuration = TimeSpan.FromMinutes(30);
		private static readonly BuffId[] Blessings = [BuffId.Honor_Buff, BuffId.Wish_Buff, BuffId.Safety_Buff, BuffId.Healthy_Buff];
		private static readonly BuffId[] BlessingsWithFortune = [BuffId.Honor_Buff, BuffId.Wish_Buff, BuffId.Safety_Buff, BuffId.Healthy_Buff, BuffId.Money_Buff];

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

			var lots = caster.IsAbilityActive(AbilityId.Miko10) ? BlessingsWithFortune : Blessings;

			foreach (var ally in PartySkillHelper.GetAlliesInRange(caster, caster.Position, Range).Take(MaxTargets))
			{
				var blessing = lots[GameRandom.Get().Next(lots.Length)];

				if (blessing == BuffId.Money_Buff)
				{
					ally.StartBuff(BuffId.Money_Buff, skill.Level, 0, FortuneDuration, caster, skill.Id);
					ally.StartBuff(BuffId.Omikuji_Durability_Buff, skill.Level, 0, FortuneDuration, caster, skill.Id);
					continue;
				}

				ally.StartBuff(blessing, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);
			}
		}
	}
}
