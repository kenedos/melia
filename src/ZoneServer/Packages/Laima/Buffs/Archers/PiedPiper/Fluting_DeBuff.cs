using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Packages.Laima.Buffs.Archers.PiedPiper
{
	[Package("laima")]
	[BuffHandler(BuffId.Fluting_DeBuff)]
	public class Fluting_DeBuffOverride : BuffHandler
	{
		private const float MinimumFollowDistance = 20f;
		private const float MovementSpeedPerAbilityLevel = 2f;
		private const int UpdateIntervalMilliseconds = 500;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(UpdateIntervalMilliseconds);
			buff.Target.SetAttackState(false);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Mob monster || monster.IsDead)
				return;

			if (buff.Caster is not ICombatEntity caster || caster.IsDead || caster.Map != monster.Map)
				return;

			monster.SetAttackState(false);

			if (monster.Position.Get2DDistance(caster.Position) <= MinimumFollowDistance)
				return;

			var movementSpeed = monster.Properties.GetFloat(PropertyName.RunMSPD);

			if (caster is Character character && character.TryGetAbility(AbilityId.PiedPiper4, out var movementAbility) && movementAbility.Level > 0)
				movementSpeed += movementAbility.Level * MovementSpeedPerAbilityLevel;

			monster.MoveTo(caster.Position, movementSpeed, ignoreHoldMove: true, suspendAI: true);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead)
				return;

			var confusionSeconds = Math.Max(buff.NumArg2, 0f);

			if (confusionSeconds <= 0)
				return;

			buff.Target.StartBuff(BuffId.Confuse, buff.NumArg1, 0, TimeSpan.FromSeconds(confusionSeconds), buff.Caster, buff.SkillId);
		}
	}
}
