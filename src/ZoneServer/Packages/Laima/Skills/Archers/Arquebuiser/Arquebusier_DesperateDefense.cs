using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Archers.Arquebusier
{
	/// <summary>
	/// Handler for the Arquebuiser skill Desperate Defense.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Arquebusier_DesperateDefense)]
	public class Arquebusier_DesperateDefense : IGroundSkillHandler
	{
		public const float BarrierRadius = 80;
		public const string BarrierSetProp = "DesperateDefense_SET";

		private const float BarrierBlockingRange = 150;
		private readonly static TimeSpan BarrierLifeTime = TimeSpan.FromSeconds(30);
		private readonly static TimeSpan BarrierUpdateInterval = TimeSpan.FromMilliseconds(500);
		private readonly static TimeSpan FailedCastCooldown = TimeSpan.FromSeconds(5);

		/// <summary>
		/// Handles skill, installing a protective barrier around the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (this.IsBarrierNearby(caster))
			{
				skill.StartCooldown(FailedCastCooldown);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			var pad = Pad.Create(PadName.Arquebusier_DesperateDefense, caster, skill, caster.Position, new Circle(caster.Position, BarrierRadius), new PadOptions
			{
				LifeTime = BarrierLifeTime,
				UpdateInterval = BarrierUpdateInterval,
			});

			caster.Map.AddPad(pad);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target, caster.Position, target?.Position ?? farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, null);

			if (caster is Character character)
				Send.ZC_SEND_PC_EXPROP(character, new MsgParameter(BarrierSetProp, 1));
		}

		/// <summary>
		/// Returns true if there's already a barrier installed close
		/// to the caster.
		/// </summary>
		/// <param name="caster"></param>
		/// <returns></returns>
		private bool IsBarrierNearby(ICombatEntity caster)
		{
			var pads = caster.Map.GetPadsAt(caster.Position, BarrierBlockingRange);

			foreach (var pad in pads)
			{
				if (pad.Name == PadName.Arquebusier_DesperateDefense)
					return true;
			}

			return false;
		}
	}
}
