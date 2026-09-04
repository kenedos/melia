using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Versioning;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	/// <summary>
	/// Skill database, indexed by skill id.
	/// </summary>
	public class SkillOverrideDb : DatabaseJsonIndexed<SkillId, SkillData>
	{
		private readonly SkillDb _skillDb;
		public SkillOverrideDb(SkillDb skillDb)
		{
			this._skillDb = skillDb;
		}

		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			var skillId = (SkillId)entry.ReadInt("skillId");
			var className = entry.ReadString("className");

			if (!_skillDb.TryFind(skillId, out var data) || className != data.ClassName)
				_skillDb.TryFind(className, out data);

			if (data == null)
				return;

			if (entry.ContainsKey("basicSp"))
				data.BasicSp = entry.ReadFloat("basicSp");
			if (entry.ContainsKey("lvUpSpendSp"))
				data.LvUpSpendSp = entry.ReadFloat("lvUpSpendSp");
			if (entry.ContainsKey("basicCast"))
				data.BasicCast = entry.ReadFloat("basicCast");
			if (entry.ContainsKey("shootTime"))
				data.ShootTime = entry.ReadTimeSpan("shootTime");
			if (entry.ContainsKey("cancelTime"))
				data.CancelTime = entry.ReadTimeSpan("cancelTime");
			if (entry.ContainsKey("holdTime"))
				data.HoldTime = entry.ReadList<int>("holdTime").Select(a => TimeSpan.FromMilliseconds(a)).ToList();
			if (entry.ContainsKey("splashType"))
				data.SplashType = entry.ReadEnum<SplashType>("splashType");
			if (entry.ContainsKey("splashRange"))
				data.SplashRange = entry.ReadFloat("splashRange");
			if (entry.ContainsKey("splashHeight"))
				data.SplashHeight = entry.ReadFloat("splashHeight");
			if (entry.ContainsKey("splashAngle"))
				data.SplashAngle = entry.ReadFloat("splashAngle");
			if (entry.ContainsKey("splashRate"))
				data.SplashRate = entry.ReadFloat("splashRate");
			if (entry.ContainsKey("maxRange"))
				data.MaxRange = entry.ReadFloat("maxRange");
			if (entry.ContainsKey("factor"))
				data.Factor = entry.ReadFloat("factor");
			if (entry.ContainsKey("factorByLevel"))
				data.FactorByLevel = entry.ReadFloat("factorByLevel");
			if (entry.ContainsKey("atkAdd"))
				data.AtkAdd = entry.ReadFloat("atkAdd");
			if (entry.ContainsKey("atkAddByLevel"))
				data.AtkAddByLevel = entry.ReadFloat("atkAddByLevel");
			if (entry.ContainsKey("captionRatio1"))
				data.CaptionRatio1 = entry.ReadFloat("captionRatio1");
			if (entry.ContainsKey("captionRatio1ByLevel"))
				data.CaptionRatio1ByLevel = entry.ReadFloat("captionRatio1ByLevel");
			if (entry.ContainsKey("captionRatio1Max"))
				data.CaptionRatio1Max = entry.ReadFloat("captionRatio1Max");
			if (entry.ContainsKey("captionRatio2"))
				data.CaptionRatio2 = entry.ReadFloat("captionRatio2");
			if (entry.ContainsKey("captionRatio2ByLevel"))
				data.CaptionRatio2ByLevel = entry.ReadFloat("captionRatio2ByLevel");
			if (entry.ContainsKey("captionRatio2Max"))
				data.CaptionRatio2Max = entry.ReadFloat("captionRatio2Max");
			if (entry.ContainsKey("captionRatio3"))
				data.CaptionRatio3 = entry.ReadFloat("captionRatio3");
			if (entry.ContainsKey("captionRatio3ByLevel"))
				data.CaptionRatio3ByLevel = entry.ReadFloat("captionRatio3ByLevel");
			if (entry.ContainsKey("captionRatio3Max"))
				data.CaptionRatio3Max = entry.ReadFloat("captionRatio3Max");
			if (entry.ContainsKey("captionTime"))
				data.CaptionTime = entry.ReadFloat("captionTime");
			if (entry.ContainsKey("captionTimeByLevel"))
				data.CaptionTimeByLevel = entry.ReadFloat("captionTimeByLevel");
			if (entry.ContainsKey("cooldownTime"))
				data.CooldownTime = entry.ReadTimeSpan("cooldownTime");
			if (entry.ContainsKey("overheatCount"))
				data.OverheatCount = entry.ReadInt("overheatCount");
			if (entry.ContainsKey("overheatDelay"))
				data.OverHeatDelay = entry.ReadTimeSpan("overheatDelay");
			if (entry.ContainsKey("overheatGroup"))
			{
				if (Versions.Protocol > 500)
					data.OverheatGroup = entry.ReadEnum("overheatGroup", CooldownId.Default);
				else
					data.OverheatGroup = entry.ReadEnum("overheatGroup", CooldownId.Global);
			}
			if (entry.ContainsKey("enableCastMove"))
				data.EnableCastMove = entry.ReadBool("enableCastMove");
			if (entry.ContainsKey("enableCastRotate"))
				data.EnableCastRotate = entry.ReadBool("enableCastRotate");
			if (entry.ContainsKey("castInterruptible"))
				data.CastInterruptible = entry.ReadBool("castInterruptible");
			if (entry.ContainsKey("useType"))
				data.UseType = entry.ReadEnum<SkillUseType>("useType");
			if (entry.ContainsKey("classType"))
				data.ClassType = entry.ReadEnum<SkillClassType>("classType");
			if (entry.ContainsKey("attackType"))
			{
				data.AttackType = entry.ReadEnum<SkillAttackType>("attackType");
				data.HitAttackType = entry.ReadEnum<HitAttackType>("attackType");
			}
			if (entry.ContainsKey("activationType"))
				data.ActivationType = entry.ReadEnum<SkillActivationType>("activationType");
			if (entry.ContainsKey("speedRateAffectedByDex"))
				data.SpeedRateAffectedByDex = entry.ReadBool("speedRateAffectedByDex");
			if (entry.ContainsKey("speedRateAffectedByBuff"))
				data.SpeedRateAffectedByBuff = entry.ReadBool("speedRateAffectedByBuff");

			this.AddOrReplace(data.Id, data);
		}
	}
}
