//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Khonot Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_bracken_42_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Khonot Forest to Salvia Forest
		AddWarp(1, "BRACKEN42_1_TO_PILGRIM41_2", -24, From("f_bracken_42_1", -1014.497, -700.0676), To("f_pilgrimroad_41_2", 123, 1540));

		// Khonot Forest to Knidos Jungle
		AddWarp(2, "BRACKEN42_1_TO_BRACKEN63_2", 181, From("f_bracken_42_1", 83.4843, 1045.963), To("f_bracken_63_2", 453, -1681));

		// Khonot Forest to Mishekan Forest
		AddWarp(3, "BRACKEN42_1_TO_BRACKEN42_2", 79, From("f_bracken_42_1", 2084.651, -198.6541), To("f_whitetrees_56_1", -1201.4257, 312.18533));

		// Khonot Forest to Tevhrin Stalactite Cave Section 1
		AddWarp(4, "BRACKEN42_1_TO_LIMESTONECAVE52_1", 245, From("f_bracken_42_1", -1597.763, 222.3342), To("d_limestonecave_52_1", 212, -1104));
	}
}
