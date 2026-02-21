using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu skill Enira (Anila).
	/// Creates a pad that moves forward, damaging enemies on contact.
	/// Requires spirit form.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_Anila)]
	public class Sadhu_AnilaOverride : IMeleeGroundSkillHandler
	{
		private const float MaxDistance = 200f;
		private const float PadSpeed = 185f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.IsBuffActive(BuffId.OOBE_Soulmaster_Buff))
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position, ForceId.GetNew(), null);

			skill.Run(this.CreateAndMovePad(caster, skill));
		}

		/// <summary>
		/// Creates a pad and moves it forward, destroying it on arrival.
		/// </summary>
		private async Task CreateAndMovePad(ICombatEntity caster, Skill skill)
		{
			if (caster == null)
				return;

			var pad = new Pad(PadName.Sadhu_Anila_Effect_Pad, caster, skill, new Circle(caster.Position, 50));
			pad.Position = caster.Position;
			pad.Direction = caster.Direction;
			pad.Movement.Speed = PadSpeed;
			caster.Map.AddPad(pad);

			var destination = pad.Position.GetRelative(caster.Direction, MaxDistance);
			// Effect seems to teleport back to caster's position at end
			// if we don't decrease the milliseconds
			var moveTime = pad.Movement.MoveTo(destination) - TimeSpan.FromMilliseconds(200);
			await skill.Wait(moveTime);
			pad.Destroy();
		}
	}
}
