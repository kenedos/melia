using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Monster
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
				monster.InvalidateSizeCache();
				monster.ChangeScale(2f, 1f);
			}

			if (size == SizeType.M)
			{
				monster.Properties.SetString(PropertyName.Size, SizeType.L);
				monster.InvalidateSizeCache();
				monster.ChangeScale(1.5f, 1f);
			}

			if (monster.Rank == MonsterRank.Normal)
				monster.Rank = MonsterRank.Elite;

			var worldConf = ZoneServer.Instance.Conf.World;
			var prevMaxHP = monster.Properties.GetFloat(PropertyName.MHP);
			var newMaxHP = monster.Properties.GetFloat(PropertyName.MHP) * worldConf.EliteHPSPRate / 100f;
			var propertyOverrides = new PropertyOverrides();
			propertyOverrides.Add(PropertyName.MHP, monster.Properties.GetFloat(PropertyName.MHP) * worldConf.EliteHPSPRate / 100f);
			propertyOverrides.Add(PropertyName.MSP, monster.Properties.GetFloat(PropertyName.MSP) * worldConf.EliteHPSPRate / 100f);
			var atkRate = worldConf.EliteAtkRate / 100f;
			var defRate = worldConf.EliteDefRate / 100f;
			var secondaryRate = worldConf.EliteSecondaryStatRate / 100f;
			propertyOverrides.Add(PropertyName.MINPATK, monster.Properties.GetFloat(PropertyName.MINPATK) * atkRate);
			propertyOverrides.Add(PropertyName.MAXPATK, monster.Properties.GetFloat(PropertyName.MAXPATK) * atkRate);
			propertyOverrides.Add(PropertyName.MINMATK, monster.Properties.GetFloat(PropertyName.MINMATK) * atkRate);
			propertyOverrides.Add(PropertyName.MAXMATK, monster.Properties.GetFloat(PropertyName.MAXMATK) * atkRate);
			propertyOverrides.Add(PropertyName.DEF, monster.Properties.GetFloat(PropertyName.DEF) * defRate);
			propertyOverrides.Add(PropertyName.MDEF, monster.Properties.GetFloat(PropertyName.MDEF) * defRate);
			propertyOverrides.Add(PropertyName.CRTHR, monster.Properties.GetFloat(PropertyName.CRTHR) * secondaryRate);
			propertyOverrides.Add(PropertyName.CRTATK, monster.Properties.GetFloat(PropertyName.CRTATK) * secondaryRate);
			propertyOverrides.Add(PropertyName.CRTDR, monster.Properties.GetFloat(PropertyName.CRTDR) * secondaryRate);
			propertyOverrides.Add(PropertyName.DR, monster.Properties.GetFloat(PropertyName.DR) * secondaryRate);
			propertyOverrides.Add(PropertyName.HR, monster.Properties.GetFloat(PropertyName.HR) * secondaryRate);
			propertyOverrides.Add(PropertyName.BLK, monster.Properties.GetFloat(PropertyName.BLK) * secondaryRate);
			propertyOverrides.Add(PropertyName.BLK_BREAK, monster.Properties.GetFloat(PropertyName.BLK_BREAK) * secondaryRate);

			// Make them nimbler
			var runSpeed = monster.Properties.GetFloat(PropertyName.RunMSPD);
			var baseRunSpeed = 60;
			var additionalRunSpeed = Math.Max(3, runSpeed - baseRunSpeed);
			propertyOverrides.Add(PropertyName.RunMSPD, runSpeed + (additionalRunSpeed * worldConf.EliteStatRate / 100f * 0.5f));

			monster.ApplyOverrides(propertyOverrides);

			// +1 SDR for elite monsters
			monster.Properties.Modify(PropertyName.SDR_BM, 1);

			// 20% faster skill usage
			monster.Vars.Set("Melia.ShootTimeMultiplier", 0.8f);
			monster.Heal(newMaxHP - prevMaxHP, 0);
			monster.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
