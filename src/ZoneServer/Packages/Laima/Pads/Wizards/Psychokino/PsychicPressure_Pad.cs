using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Util;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.HandlersOverride
{
	[Package("laima")]
	[PadHandler(PadName.PsychicPressure_Pad)]
	public class PsychicPressure_PadOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			var offset = creator.Position.GetRelative(creator.Direction, 15f);
			pad.Position = offset;
			pad.SetRange(35f);
			pad.SetUpdateInterval(500);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);
			pad.Trigger.MaxUseCount = 1;

			PsychicPressureFireball(pad, creator, skill);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			var offset = creator.Position.GetRelative(creator.Direction, 15f);
			pad.Position = offset;
			pad.SetRange(35f);

			PsychicPressureFireball(pad, creator, skill);
		}
		public void PsychicPressureFireball(Pad pad, ICombatEntity creator, Skill skill)
		{
			var pads = pad.Trigger.GetActors<Pad>();
			var icewallsHit = 0;
			foreach (var checkPad in pads)
			{
				if (checkPad.Name == PadName.Cryomancer_IceWall)
				{
					var iceWallForceSkill = new Skill(creator, SkillId.Mon_pcskill_icewall_Skill_1);
					MonsterSkillPadDirMissile(creator, iceWallForceSkill, checkPad.Position, PadName.IceWall_Force, 200f, 1, 400f, 0f, 0f, 0f);
					checkPad.Monster?.TakeDamage(1, null);
					icewallsHit++;
				}
			}

			if (icewallsHit > 0)
				UseMonsterSkillToDir(creator, SkillId.Mon_pcskill_icewall_Skill_1, creator.Direction);

			var allTargets = pad.Map.GetAttackableEnemiesIn(creator, pad.Area);
			var maxTargets = 4 + skill.Level;
			var targets = allTargets.Take(maxTargets);

			foreach (var target in targets)
			{
				this.Attack(skill, creator, target);
			}


			if (!creator.TrySpendSp(skill))
			{
				creator.ServerMessage(Localization.Get("Not enough SP."));
				pad.Destroy();
			}
		}

		/// <summary>
		/// Attacks the target one time.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		private void Attack(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			var damageDelay = TimeSpan.Zero;
			var skillHitDelay = TimeSpan.Zero;

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);

			Send.ZC_SKILL_HIT_INFO(caster, skillHit);

			var rand = RandomProvider.Get();
			if (rand.NextDouble() >= .5)
				target.StartBuff(BuffId.Stun, TimeSpan.FromSeconds(3), caster);
		}
	}
}
