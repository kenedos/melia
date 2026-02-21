using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Skills.Handlers.Pyromancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Pyromancer_HellBreath)]
	public class Pyromancer_HellBreathOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const float MaxDistance = 200f;
		private const float PadSpeed = 150f;
		private const int PadDelayMs = 200;
		private const int MaxSkillTimeMiliseconds = 10000;

		private bool _isCasting;

		/// <summary>
		/// Starts the dynamic casting of the skill.
		/// </summary>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
			caster.SetCastingState(true, skill);
			_isCasting = true;
		}

		/// <summary>
		/// Ends the dynamic casting of the skill.
		/// </summary>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			_isCasting = false;
			caster.SetCastingState(false, skill);
			Send.ZC_SKILL_DISABLE(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
			caster.StopSound("skl_eff_pyromancer_hellbreath_abil");
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, 0);
		}

		/// <summary>
		/// Handles the execution of the skill.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);

			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, caster.Position);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			var maxCreatedPads = MaxSkillTimeMiliseconds / PadDelayMs;
			var createdPads = 0;
			while (_isCasting && createdPads < maxCreatedPads)
			{
				if (!caster.TrySpendSp(skill))
				{
					caster.ServerMessage(Localization.Get("Not enough SP."));
					Send.ZC_SKILL_CAST_CANCEL(caster);
					return;
				}

				_ = Task.Run(() => this.CreateAndMovePad(caster, skill));
				await skill.Wait(TimeSpan.FromMilliseconds(PadDelayMs));
				createdPads++;
			}
		}

		/// <summary>
		/// Creates a Hell Breath pad.
		/// </summary>
		private Pad CreateHellBreathPad(ICombatEntity caster, Skill skill)
		{
			if (caster == null)
				return null;

			var pad = new Pad(PadName.Pyromancer_HellBreath, caster, skill, new Circle(caster.Position, 45));
			pad.Position = caster.Position;
			pad.Direction = caster.Direction;
			pad.Movement.Speed = PadSpeed;
			caster.Map.AddPad(pad);
			return pad;
		}

		/// <summary>
		/// Creates a pad, moves it forward, and destroys it after it
		/// reaches destination.
		/// </summary>
		private async Task CreateAndMovePad(ICombatEntity caster, Skill skill)
		{
			if (caster == null)
				return;

			var pad = this.CreateHellBreathPad(caster, skill);
			var destination = pad.Position.GetRelative(caster.Direction, MaxDistance);
			var moveTime = pad.Movement.MoveTo(destination);
			await skill.Wait(moveTime);
			pad.Destroy();
		}
	}
}
