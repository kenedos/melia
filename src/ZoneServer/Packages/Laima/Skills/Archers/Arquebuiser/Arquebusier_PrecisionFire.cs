using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Arquebusier
{
	/// <summary>
	/// Handler for the Arquebuiser skill Precision Fire.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Arquebusier_PrecisionFire)]
	public class Arquebusier_PrecisionFire : IGroundSkillHandler, IDynamicCasted
	{
		private const float ShotStartDistance = 20;
		public const float ShotFlyDistance = 230;

		private const string ReleaseTimeVar = "Melia.PrecisionFire.ReleaseTime";
		private const int TotalShots = 3;
		private const int HitsPerShot = 2;
		private const float ShotWidth = 30;
		private readonly static TimeSpan FirstShotDelay = TimeSpan.FromMilliseconds(400);
		private readonly static TimeSpan DelayBetweenShots = TimeSpan.FromMilliseconds(700);
		private readonly static TimeSpan ChannelEndDelay = TimeSpan.FromMilliseconds(200);
		private readonly static TimeSpan ReleaseGrace = TimeSpan.FromMilliseconds(150);
		private readonly static TimeSpan ShotLifeTime = TimeSpan.FromMilliseconds(650);

		/// <summary>
		/// Called when the user stops channeling the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			skill.Vars.Set(ReleaseTimeVar, DateTime.Now);
		}

		/// <summary>
		/// Handles skill, firing in a straight line while channeling.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.Vars.Remove(ReleaseTimeVar);
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target, originPos, target?.Position ?? farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Channel(skill, caster));
		}

		/// <summary>
		/// Fires the shots for as long as the caster keeps channeling.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		private async Task Channel(Skill skill, ICombatEntity caster)
		{
			for (var i = 0; i < TotalShots; ++i)
			{
				await skill.Wait(i == 0 ? FirstShotDelay : DelayBetweenShots);

				if (this.WasReleased(skill))
					break;

				this.Shoot(skill, caster);
			}

			await skill.Wait(ChannelEndDelay);
			Send.ZC_SKILL_DISABLE(caster);
		}

		/// <summary>
		/// Returns true if the caster stopped channeling. A release that
		/// only just happened still lets the current shot go out, as the
		/// end of a full channel arrives right around the last shot.
		/// </summary>
		/// <param name="skill"></param>
		/// <returns></returns>
		private bool WasReleased(Skill skill)
		{
			if (!skill.Vars.TryGet<DateTime>(ReleaseTimeVar, out var releaseTime))
				return false;

			return DateTime.Now - releaseTime > ReleaseGrace;
		}

		/// <summary>
		/// Fires a single shot in the direction the caster is facing.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		private void Shoot(Skill skill, ICombatEntity caster)
		{
			var startPos = caster.Position.GetRelative2D(caster.Direction, ShotStartDistance);
			var splashArea = new Square(startPos, caster.Direction, ShotFlyDistance, ShotWidth);

			// The pad is just visual, the skill itself handles the damaging
			var pad = Pad.Create(PadName.shootpad_PrecisionFire, caster, skill, startPos, splashArea, new PadOptions
			{
				LifeTime = ShotLifeTime,
				MaxActorCount = 0,
			});

			caster.Map.AddPad(pad);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(HitsPerShot));
				target.TakeDamage(skillHitResult.Damage, caster);

				var hit = new HitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero);
				Send.ZC_HIT_INFO(caster, target, hit);
			}
		}
	}

	/// <summary>
	/// Handler for the Precision Fire shot pad.
	/// </summary>
	[PadHandler(PadName.shootpad_PrecisionFire)]
	public class shootpad_PrecisionFire : ICreatePadHandler, IDestroyPadHandler
	{
		private const float FlySpeed = 800;

		/// <summary>
		/// Called when the pad is created.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;

			var dest = pad.Position.GetRelative2D(caster.Direction, Arquebusier_PrecisionFire.ShotFlyDistance);

			pad.Movement.Speed = FlySpeed;
			pad.Movement.MoveTo(dest);
		}

		/// <summary>
		/// Called when the pad is destroyed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}
	}
}
