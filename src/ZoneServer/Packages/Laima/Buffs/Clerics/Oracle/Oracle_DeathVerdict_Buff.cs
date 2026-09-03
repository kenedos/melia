using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Death Sentence buff, which kills the target outright
	/// once it runs out.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DeathVerdict_Buff)]
	public class Oracle_DeathVerdict_BuffOverride : BuffHandler
	{
		private const float MspdReduceRatePerLevel = 0.15f;
		private const string GaugeSkinName = "gauge_red";
		private const string GaugeScript = "MAKE_GAUGE_BALLOON({0}, 0, {1}, 1, \"{2}\")";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			if (buff.Caster is ICombatEntity caster)
			{
				var abilLevel = caster.GetAbilityLevel(AbilityId.Oracle8);
				if (abilLevel > 0)
				{
					var mspdReduce = target.Properties.GetFloat(PropertyName.MSPD) * MspdReduceRatePerLevel * abilLevel;
					AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -mspdReduce);
				}
			}

			this.UpdateGauge(buff, buff.Duration.TotalSeconds);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);
			this.UpdateGauge(buff, 0);

			if (target.IsDead || buff.Caster is not ICombatEntity caster)
				return;

			if (target is Mob)
			{
				target.TakeSimpleHit(target.Properties.GetFloat(PropertyName.HP), caster, SkillId.Oracle_DeathVerdict);
				return;
			}

			// A player is not sentenced outright, only struck once
			if (!caster.TryGetSkill(buff.SkillId, out var skill))
				return;

			var hitResult = SCR_SkillHit(caster, target, skill);
			target.TakeSimpleHit(hitResult.Damage, caster, SkillId.Oracle_DeathVerdict);
		}

		/// <summary>
		/// Shows the countdown bar above the target for the given number of
		/// seconds, removing it when that is zero.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="seconds"></param>
		private void UpdateGauge(Buff buff, double seconds)
		{
			if (buff.Caster is not Character character || character.Connection == null)
				return;

			var script = string.Format(GaugeScript, buff.Target.Handle, (int)Math.Round(seconds), GaugeSkinName);
			Send.ZC_EXEC_CLIENT_SCP(character.Connection, script);
		}
	}
}
