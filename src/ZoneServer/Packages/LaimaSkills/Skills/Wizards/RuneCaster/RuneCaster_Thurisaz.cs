using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for the Rune Caster skill Rune of Repulsion, a channel of up
	/// to 2 seconds that fires a Psychokinesis sphere forward every 0.2
	/// seconds.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.RuneCaster_Thurisaz)]
	public class RuneCaster_ThurisazOverride : IDynamicCasted
	{
		private const int SphereCount = 10;
		private const float SphereRange = 30f;
		private const float SphereSpeed = 400f;
		private static readonly TimeSpan SphereInterval = TimeSpan.FromMilliseconds(200);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			skill.Run(this.Fire(skill, caster));
		}

		/// <summary>
		/// Fires a sphere forward every 0.2 seconds while the caster keeps
		/// channeling.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Fire(Skill skill, ICombatEntity caster)
		{
			for (var i = 0; i < SphereCount; i++)
			{
				if (i > 0)
					await skill.Wait(SphereInterval);

				if (caster.IsDead || !caster.IsCasting(skill))
					return;

				var pad = new Pad(PadName.RuneCaster_Eihwaz_Pad, caster, skill, new Circle(caster.Position, SphereRange));
				pad.Position = caster.Position;
				pad.Direction = caster.Direction;
				pad.Movement.Speed = SphereSpeed;
				caster.Map.AddPad(pad);

				var destination = caster.Map.Ground.GetLastValidPosition(caster.Position, caster.Position.GetRelative(caster.Direction, skill.Data.MaxRange));
				skill.RunFree(pad.Movement.MoveToAndDestroy(destination));
			}
		}
	}
}
