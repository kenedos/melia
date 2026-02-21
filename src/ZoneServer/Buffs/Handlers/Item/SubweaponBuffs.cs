using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for subweapon_metaldetector, which scans for hidden triggers nearby.
	/// </summary>
	[BuffHandler(BuffId.subweapon_metaldetector)]
	public class subweapon_metaldetector : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character target || target.Map == null) return;

			// Find all actors within 60 units of the target.
			var nearbyActors = target.Map.GetActorsInRange<IActor>(target.Position, 60);

			foreach (var actor in nearbyActors)
			{
				// The original script checks for a specific monster class name.
				if (actor is MonsterInName monster && monster.Id == MonsterId.SearchTrigger)
				{
					var npcState = target.GetMapNPCState(monster);
					if (npcState >= 0)
					{
						// Run a specific script function if the trigger is found and in a valid state.
						//ScriptableFunctions.Run("SCR_SCOUT_SCAN_TX", target, monster, monster.GenType);
					}
				}
			}
		}
	}

	/// <summary>
	/// Handle for subweapon_dumbbell, which provides STR but reduces Max Stamina.
	/// </summary>
	[BuffHandler(BuffId.subweapon_dumbbell)]
	public class subweapon_dumbbell : BuffHandler
	{
		private const string VarAddStr = "Melia.Subweapon.Dumbbell.AddStr";
		private const string VarAddSta = "Melia.Subweapon.Dumbbell.AddSta";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			if (target is not Character character) return;

			var overbuffCount = buff.OverbuffCounter;
			var strBonus = 3 * overbuffCount;
			var staPenalty = 2 * overbuffCount;

			// If the SwellRightArm buff is active, the stamina penalty is halved.
			if (target.IsBuffActive(BuffId.SwellRightArm_Buff))
			{
				staPenalty /= 2;
			}

			buff.Vars.SetFloat(VarAddStr, strBonus);
			buff.Vars.SetFloat(VarAddSta, staPenalty);

			character.Properties.Modify(PropertyName.STR_BM, strBonus);
			character.Properties.Modify(PropertyName.MaxSta_BM, -staPenalty);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			if (target is not Character) return;

			if (buff.Vars.TryGetFloat(VarAddStr, out var strBonus))
			{
				target.Properties.Modify(PropertyName.STR_BM, -strBonus);
			}

			if (buff.Vars.TryGetFloat(VarAddSta, out var staPenalty))
			{
				target.Properties.Modify(PropertyName.MaxSta_BM, staPenalty);
			}
		}
	}
}
