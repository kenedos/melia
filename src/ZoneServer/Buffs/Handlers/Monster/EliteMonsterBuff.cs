using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Elite Buff, An enormous monster, like the leader of the group..
	/// </summary>
	[BuffHandler(BuffId.EliteMonsterBuff)]
	public class EliteMonsterBuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is Character)
				return;
			var monster = (Mob)buff.Target;
			var size = monster.EffectiveSize;

			if (size == SizeType.S)
			{
				monster.Properties.SetString(PropertyName.Size, SizeType.L);
				monster.ChangeScale(2f, 1f);
			}

			if (size == SizeType.M)
			{
				monster.Properties.SetString(PropertyName.Size, SizeType.L);
				monster.ChangeScale(1.5f, 1f);
			}

			if (monster.Rank == MonsterRank.Normal)
				monster.Properties.SetString(PropertyName.MonRank, MonsterRank.Elite);

			var worldConf = ZoneServer.Instance.Conf.World;
			var prevMaxHP = monster.Properties.GetFloat(PropertyName.MHP);
			var newMaxHP = monster.Properties.GetFloat(PropertyName.MHP) * worldConf.EliteHPSPRate / 100f;
			var propertyOverrides = new PropertyOverrides();
			propertyOverrides.Add(PropertyName.MHP, monster.Properties.GetFloat(PropertyName.MHP) * worldConf.EliteHPSPRate / 100f);
			propertyOverrides.Add(PropertyName.MSP, monster.Properties.GetFloat(PropertyName.MSP) * worldConf.EliteHPSPRate / 100f);
			propertyOverrides.Add(PropertyName.MINPATK, monster.Properties.GetFloat(PropertyName.MINPATK) * worldConf.EliteStatRate / 100f);
			propertyOverrides.Add(PropertyName.MAXPATK, monster.Properties.GetFloat(PropertyName.MAXPATK) * worldConf.EliteStatRate / 100f);
			propertyOverrides.Add(PropertyName.MINMATK, monster.Properties.GetFloat(PropertyName.MINMATK) * worldConf.EliteStatRate / 100f);
			propertyOverrides.Add(PropertyName.MAXMATK, monster.Properties.GetFloat(PropertyName.MAXMATK) * worldConf.EliteStatRate / 100f);
			propertyOverrides.Add(PropertyName.DEF, monster.Properties.GetFloat(PropertyName.DEF) * worldConf.EliteStatRate / 100f);
			propertyOverrides.Add(PropertyName.MDEF, monster.Properties.GetFloat(PropertyName.MDEF) * worldConf.EliteStatRate / 100f);

			// Make them nimbler
			var runSpeed = monster.Properties.GetFloat(PropertyName.RunMSPD);
			var baseRunSpeed = 60;
			var additionalRunSpeed = Math.Max(3, runSpeed - baseRunSpeed);
			propertyOverrides.Add(PropertyName.RunMSPD, runSpeed + (additionalRunSpeed * worldConf.EliteStatRate / 100f));

			monster.ApplyOverrides(propertyOverrides);
			monster.Heal(newMaxHP - prevMaxHP, 0);
			monster.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
