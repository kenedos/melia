using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Pet_Dead buff, which puts a companion in an
	/// incapacitated and safe state.
	/// </summary>
	[BuffHandler(BuffId.Pet_Dead)]
	public class Pet_Dead : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Companion pet)
				return;

			pet.SetHittable(false);
			pet.AddState(StateType.Stunned);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Companion pet)
				return;

			pet.SetHittable(true);
			pet.RemoveState(StateType.Stunned);
		}
	}

	/// <summary>
	/// Handle for the Pet_Heal buff, which restores a percentage of a
	/// companion's health over time and applies an after-effect buff.
	/// </summary>
	[BuffHandler(BuffId.Pet_Heal)]
	public class Pet_Heal : BuffHandler
	{
		private const float HealRate = 0.02f;
		private const string VarHealAmount = "Melia.Pet.HealAmount";
		private static readonly TimeSpan AfterBuffDuration = TimeSpan.FromMinutes(10);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var maxHp = buff.Target.Properties.GetFloat(PropertyName.MHP);
			var healAmount = (float)Math.Floor(maxHp * HealRate);
			buff.Vars.SetFloat(VarHealAmount, healAmount);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Vars.TryGetFloat(VarHealAmount, out var healAmount))
			{
				buff.Target.Heal(healAmount, 0);
			}
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.StartBuff(BuffId.Pet_Heal_After, AfterBuffDuration, buff.Target);
		}
	}
}
