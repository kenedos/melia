using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for the Bullet Marker skill Outrage, which turns every 2
	/// stacks of Overheating into a stack of Outrage, 30 from a full 40.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Bulletmarker_Outrage)]
	public class Bulletmarker_OutrageOverride : IGroundSkillHandler
	{
		private const int MinOverheating = 4;
		private const int MaxOverheating = 40;
		private const int FullOutrage = 30;
		private static readonly TimeSpan Duration = TimeSpan.FromSeconds(35);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!BulletMarkerSkillHelper.CheckDoubleGunStance(caster))
				return;

			var overheating = caster.GetOverbuffCount(BuffId.Overheating_Buff);
			if (overheating < MinOverheating)
			{
				caster.ServerMessage(Localization.Get("At least 4 stacks of Overheating are required."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var stacks = overheating >= MaxOverheating ? FullOutrage : overheating / 2;

			caster.StopBuff(BuffId.Overheating_Buff, BuffId.Outrage_Buff);

			for (var i = 0; i < stacks; i++)
				caster.StartBuff(BuffId.Outrage_Buff, skill.Level, 0, Duration, caster, skill.Id);
		}
	}
}
