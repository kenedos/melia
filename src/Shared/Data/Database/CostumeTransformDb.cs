using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;

namespace Melia.Shared.Data.Database
{
	/// <summary>
	/// Describes a transform-type costume: equipping <see cref="BaseClassName"/>
	/// grants <see cref="SkillName"/>, which casts <see cref="BuffName"/>,
	/// swapping the character's appearance to <see cref="TransformClassName"/>
	/// in <see cref="Slot"/>.
	/// </summary>
	public class CostumeTransformData
	{
		public string BaseClassName { get; set; }
		public string TransformClassName { get; set; }
		public string SkillName { get; set; }
		public string BuffName { get; set; }
		public EquipSlot Slot { get; set; }
	}

	/// <summary>
	/// Hardcoded lookup of transform-type costumes, seeded from
	/// item_skillmake_costume.tsv.
	/// </summary>
	/// <remarks>
	/// The underlying TSV is small (~64 rows) and changes rarely, so we keep
	/// the table in code instead of loading from disk. To add a new transform:
	///  1. Add a row to <see cref="Entries"/>. TransformClassName defaults to
	///     "&lt;base&gt;_af", but pass an explicit 4th argument to <c>New</c>
	///     when the transform item has a different class name (e.g. the
	///     MagicalGirl Mystic set maps 187/188/... -&gt; 194/198/...).
	///  2. If the BuffId is new, add it to <see cref="BuffId"/>.
	///  3. Add a one-line subclass of CostumeTransformBuffHandler.
	/// All other behavior (look broadcast, visibility re-send, equip skill
	/// grant) is driven by this DB.
	/// </remarks>
	public static class CostumeTransformDb
	{
		private static readonly CostumeTransformData[] Entries = new[]
		{
			New("costume_Com_187", "MagicalGirl_MagicalBoyYellow", "change_magicalboy_yellow_Buff", "costume_Com_194"),
			New("costume_Com_188", "MagicalGirl_MagicalGirlYellow", "change_magicalgirl_yellow_Buff", "costume_Com_198"),
			New("costume_Com_189", "MagicalGirl_MagicalBoyRed", "change_magicalboy_red_Buff", "costume_Com_193"),
			New("costume_Com_190", "MagicalGirl_MagicalGirlRed", "change_magicalgirl_red_Buff", "costume_Com_197"),
			New("costume_Com_191", "MagicalGirl_MagicalGirlYellow", "change_magicalgirl_yellow_Buff", "costume_Com_196"),
			New("costume_Com_192", "MagicalGirl_MagicalGirlRed", "change_magicalgirl_red_Buff", "costume_Com_195"),
			New("costume_Com_242", "TosRangerFormChange_RedM", "TosRangerFormChange_Red_Buff", "costume_Com_248"),
			New("costume_Com_243", "TosRangerFormChange_RedF", "TosRangerFormChange_Red_Buff", "costume_Com_248"),
			New("costume_Com_244", "TosRangerFormChange_Pink", "TosRangerFormChange_Pink_Buff", "costume_Com_249"),
			New("costume_Com_245", "TosRangerFormChange_Blue", "TosRangerFormChange_Blue_Buff", "costume_Com_250"),
			New("costume_Com_247", "TosRangerFormChange_Yellow", "TosRangerFormChange_Yellow_Buff", "costume_Com_251"),
			New("costume_Com_246", "TosRangerFormChange_Green", "TosRangerFormChange_Green_Buff", "costume_Com_252"),
			New("costume_Com_288", "MagicalGirl_MagicalBoyMint", "change_magicalboy_mint_Buff", "costume_Com_294"),
			New("costume_Com_289", "MagicalGirl_MagicalGirlMint", "change_magicalgirl_mint_Buff", "costume_Com_295"),
			New("costume_Com_290", "MagicalGirl_MagicalBoyPink", "change_magicalboy_pink_Buff", "costume_Com_296"),
			New("costume_Com_291", "MagicalGirl_MagicalGirlPink", "change_magicalgirl_pink_Buff", "costume_Com_297"),
			New("costume_Com_292", "MagicalGirl_MagicalBoyBlack", "change_magicalboy_black_Buff", "costume_Com_298"),
			New("costume_Com_293", "MagicalGirl_MagicalGirlBlack", "change_magicalgirl_black_Buff", "costume_Com_299"),
			New("costume_Com_301", "MagicalGirl_MagicalGirlBlack", "change_magicalgirl_black_Buff", "costume_Com_300"),
			New("EP12_costume_Com_012", "Catagnan_Transform", "ITEM_BUFF_Catagnan_Transform", "EP12_costume_Com_018"),
			New("EP12_costume_Com_013", "Catagnan_Transform", "ITEM_BUFF_Catagnan_Transform", "EP12_costume_Com_019"),
			New("EP12_costume_Com_014", "Catagnan_Transform", "ITEM_BUFF_Catagnan_Transform", "EP12_costume_Com_020"),
			New("EP12_costume_Com_015", "Catagnan_Transform", "ITEM_BUFF_Catagnan_Transform", "EP12_costume_Com_021"),
			New("EP12_costume_Com_016", "Catagnan_Transform", "ITEM_BUFF_Catagnan_Transform", "EP12_costume_Com_022"),
			New("EP12_costume_Com_017", "Catagnan_Transform", "ITEM_BUFF_Catagnan_Transform", "EP12_costume_Com_023"),
			New("EP12_costume_Com_054", "curtaincall_EvilWithin_Boy", "ITEM_BUFF_Curtaincall_EvilWithin_Boy", "EP12_costume_Com_056"),
			New("EP12_costume_Com_055", "curtaincall_EvilWithin_Girl", "ITEM_BUFF_Curtaincall_EvilWithin_Girl", "EP12_costume_Com_057"),
			New("EP12_costume_Com_060", "curtaincall_EvilWithin_Boy_1", "ITEM_BUFF_Curtaincall_EvilWithin_Boy", "EP12_costume_Com_062"),
			New("EP12_costume_Com_061", "curtaincall_EvilWithin_Girl_1", "ITEM_BUFF_Curtaincall_EvilWithin_Girl", "EP12_costume_Com_063"),
			New("EP12_costume_Com_064", "curtaincall_EvilWithin_Boy_2", "ITEM_BUFF_Curtaincall_EvilWithin_Boy", "EP12_costume_Com_066"),
			New("EP12_costume_Com_065", "curtaincall_EvilWithin_Girl_2", "ITEM_BUFF_Curtaincall_EvilWithin_Girl", "EP12_costume_Com_067"),
			New("EP12_costume_Com_060_global", "curtaincall_EvilWithin_Boy_1", "ITEM_BUFF_Curtaincall_EvilWithin_Boy", "EP12_costume_Com_062"),
			New("EP12_costume_Com_061_global", "curtaincall_EvilWithin_Girl_1", "ITEM_BUFF_Curtaincall_EvilWithin_Girl", "EP12_costume_Com_063"),
			New("EP12_costume_Com_064_global", "curtaincall_EvilWithin_Boy_2", "ITEM_BUFF_Curtaincall_EvilWithin_Boy", "EP12_costume_Com_066"),
			New("EP12_costume_Com_065_global", "curtaincall_EvilWithin_Girl_2", "ITEM_BUFF_Curtaincall_EvilWithin_Girl", "EP12_costume_Com_067"),
			New("EP13_costume_Com_108", "tcance_transfrom_boy", "ITEM_BUFF_TCANCE_BOY"),
			New("EP13_costume_Com_109", "tcance_transfrom_girl", "ITEM_BUFF_TCANCE_GIRL"),
			New("EP13_costume_Com_110", "tcance_transfrom_boy", "ITEM_BUFF_TCANCE_BOY"),
			New("EP13_costume_Com_111", "tcance_transfrom_girl", "ITEM_BUFF_TCANCE_GIRL"),
			New("EP13_costume_Com_112", "tcance_transfrom_boy", "ITEM_BUFF_TCANCE_BOY"),
			New("EP13_costume_Com_113", "tcance_transfrom_girl", "ITEM_BUFF_TCANCE_GIRL"),
			New("EP13_costume_Com_114", "tcance_transfrom_boy", "ITEM_BUFF_TCANCE_BOY"),
			New("EP13_costume_Com_115", "tcance_transfrom_girl", "ITEM_BUFF_TCANCE_GIRL"),
			New("EP13_costume_Com_116", "tcance_transfrom_boy", "ITEM_BUFF_TCANCE_BOY"),
			New("EP13_costume_Com_117", "tcance_transfrom_girl", "ITEM_BUFF_TCANCE_GIRL"),
			New("EP13_costume_Com_118", "tcance_transfrom_boy", "ITEM_BUFF_TCANCE_BOY"),
			New("EP13_costume_Com_119", "tcance_transfrom_girl", "ITEM_BUFF_TCANCE_GIRL"),
			New("EP13_costume_Com_120", "tcance_transfrom_boy", "ITEM_BUFF_TCANCE_BOY"),
			New("EP13_costume_Com_121", "tcance_transfrom_girl", "ITEM_BUFF_TCANCE_GIRL"),
			New("EP13_costume_Com_122", "tcance_transfrom_boy", "ITEM_BUFF_TCANCE_BOY"),
			New("EP13_costume_Com_123", "tcance_transfrom_girl", "ITEM_BUFF_TCANCE_GIRL"),
			New("EP13_costume_Com_124", "tcance_transfrom_boy", "ITEM_BUFF_TCANCE_BOY"),
			New("EP13_costume_Com_125", "tcance_transfrom_girl", "ITEM_BUFF_TCANCE_GIRL"),
			New("EP14_costume_Com_001", "MagicalGirl_MagicalBoyLumen", "change_magicalboy_lumen_Buff"),
			New("EP14_costume_Com_002", "MagicalGirl_MagicalGirlLumen", "change_magicalgirl_lumen_Buff"),
			New("EP14_costume_Com_003", "MagicalGirl_MagicalBoyNox", "change_magicalboy_nox_Buff"),
			New("EP14_costume_Com_004", "MagicalGirl_MagicalGirlNox", "change_magicalgirl_nox_Buff"),
			New("EP13_costume_Com_120_W", "tcance_transfrom_boy", "ITEM_BUFF_TCANCE_BOY"),
			New("EP13_costume_Com_121_W", "tcance_transfrom_girl", "ITEM_BUFF_TCANCE_GIRL"),
			New("EP13_costume_Com_122_W", "tcance_transfrom_boy", "ITEM_BUFF_TCANCE_BOY"),
			New("EP13_costume_Com_123_W", "tcance_transfrom_girl", "ITEM_BUFF_TCANCE_GIRL"),
			New("EP13_costume_Com_124_W", "tcance_transfrom_boy", "ITEM_BUFF_TCANCE_BOY"),
			New("EP13_costume_Com_125_W", "tcance_transfrom_girl", "ITEM_BUFF_TCANCE_GIRL"),
		};

		private static readonly Dictionary<string, CostumeTransformData> _byBase =
			BuildIndex(x => x.BaseClassName);
		private static readonly Dictionary<string, CostumeTransformData> _byBuff =
			BuildIndex(x => x.BuffName);

		private static CostumeTransformData New(string baseClassName, string skillName, string buffName, string transformClassName = null)
		{
			return new CostumeTransformData
			{
				BaseClassName = baseClassName,
				TransformClassName = transformClassName ?? baseClassName + "_af",
				SkillName = skillName,
				BuffName = buffName,
				Slot = EquipSlot.Outer1,
			};
		}

		private static Dictionary<string, CostumeTransformData> BuildIndex(Func<CostumeTransformData, string> keySelector)
		{
			var map = new Dictionary<string, CostumeTransformData>(StringComparer.OrdinalIgnoreCase);
			foreach (var entry in Entries)
			{
				var key = keySelector(entry);
				if (!map.ContainsKey(key))
					map.Add(key, entry);
			}
			return map;
		}

		/// <summary>
		/// Returns the transform data for the given base costume class name
		/// (e.g. "EP14_costume_Com_004"), or false if not a transform costume.
		/// </summary>
		public static bool TryFindByBase(string baseClassName, out CostumeTransformData data)
		{
			if (string.IsNullOrEmpty(baseClassName))
			{
				data = null;
				return false;
			}
			return _byBase.TryGetValue(baseClassName, out data);
		}

		/// <summary>
		/// Returns the transform data for the given buff class name
		/// (e.g. "change_magicalgirl_nox_Buff"), or false if no match.
		/// </summary>
		public static bool TryFindByBuff(string buffClassName, out CostumeTransformData data)
		{
			if (string.IsNullOrEmpty(buffClassName))
			{
				data = null;
				return false;
			}
			return _byBuff.TryGetValue(buffClassName, out data);
		}
	}
}
