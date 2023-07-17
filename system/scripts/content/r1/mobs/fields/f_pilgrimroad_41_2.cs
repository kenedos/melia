//--- Melia Script -----------------------------------------------------------
// f_pilgrimroad_41_2
//
//--- Description -----------------------------------------------------------
// Sets up the f_pilgrimroad_41_2 mobs.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Tos.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad412MobScript : GeneralScript
{
	public override void Load()
	{
		// Property Overrides
		AddPropertyOverrides("f_pilgrimroad_41_2", MonsterId.Spion_Red, Properties("MHP", 458205, "MINPATK", 6393, "MAXPATK", 7759, "MINMATK", 6393, "MAXMATK", 7759, "DEF", 63461, "MDEF", 63461));
		AddPropertyOverrides("f_pilgrimroad_41_2", MonsterId.Tiny_Mage_Green, Properties("MHP", 459169, "MINPATK", 6405, "MAXPATK", 7774, "MINMATK", 6405, "MAXMATK", 7774, "DEF", 63848, "MDEF", 63848));
		AddPropertyOverrides("f_pilgrimroad_41_2", MonsterId.Spion_Bow_Red, Properties("MHP", 460263, "MINPATK", 6419, "MAXPATK", 7792, "MINMATK", 6419, "MAXMATK", 7792, "DEF", 64287, "MDEF", 64287));
		AddPropertyOverrides("f_pilgrimroad_41_2", MonsterId.Defender_Spider_Red, Properties("MHP", 461472, "MINPATK", 6435, "MAXPATK", 7811, "MINMATK", 6435, "MAXMATK", 7811, "DEF", 64772, "MDEF", 64772));

		// Monster Spawners --------------------------------

		AddSpawner("f_pilgrimroad_41_2.Id1", MonsterId.Rootcrystal_02, 15, TimeSpan.FromMilliseconds(5000), TendencyType.Peaceful);
		AddSpawner("f_pilgrimroad_41_2.Id2", MonsterId.Spion_Red, 70, TimeSpan.FromMilliseconds(0), TendencyType.Peaceful);
		AddSpawner("f_pilgrimroad_41_2.Id3", MonsterId.Tiny_Mage_Green, 20, TimeSpan.FromMilliseconds(0), TendencyType.Peaceful);
		AddSpawner("f_pilgrimroad_41_2.Id4", MonsterId.Spion_Bow_Red, 10, TimeSpan.FromMilliseconds(0), TendencyType.Peaceful);
		AddSpawner("f_pilgrimroad_41_2.Id5", MonsterId.Defender_Spider_Red, 10, TimeSpan.FromMilliseconds(0), TendencyType.Peaceful);
		AddSpawner("f_pilgrimroad_41_2.Id6", MonsterId.Tiny_Mage_Green, 30, TimeSpan.FromMilliseconds(0), TendencyType.Peaceful);
		AddSpawner("f_pilgrimroad_41_2.Id7", MonsterId.Spion_Red, 6, TimeSpan.FromMilliseconds(0), TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// Rootcrystal_02 Spawn Points
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-1670.7682, 329.7815, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-1488.3881, 566.0125, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-1224.5886, 416.37317, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-814.5619, 306.7561, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-812.29395, 648.3942, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-586.54047, 478.7791, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-71.02604, 723.13434, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-91.64652, 376.40158, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(197.03047, 390.98758, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(209.21352, 681.67163, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(186.6979, 1080.193, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(627.2356, 977.218, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(669.07886, 496.14932, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(962.27716, 614.6889, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-15.106125, -69.03851, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-90.7071, -428.0874, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(287.2421, -360.89572, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-975.3148, -615.77594, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-656.59503, -544.1736, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-393.37067, -1207.0701, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-53.409096, -1304.3795, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(-181.03537, -1009.2533, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(233.10165, -1089.3829, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(811.6029, -747.30634, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(901.8728, -1029.5823, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(1132.6127, -855.72375, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(1554.4082, -1012.1646, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(1620.3235, -814.54376, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(1868.8419, -966.3634, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(686.48126, -231.57434, 50));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id1", "f_pilgrimroad_41_2", Spot(1035.3124, -156.03981, 50));

		// Spion_Red Spawn Points
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-1471.5566, 246.05994, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-1665.4564, 544.21594, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-1446.7976, 479.92737, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-1369.2383, 386.77682, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-855.4333, 383.28552, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-703.8189, 651.5733, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-896.99664, 631.4742, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-644.8233, 302.42972, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-295.19772, 392.51096, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-111.10767, 806.3206, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-25.216587, 613.0547, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(96.33925, 340.0165, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(570.84467, 547.24066, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(630.4958, 928.80786, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(406.46237, 937.3325, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(114.57957, 1103.5856, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-266.6182, 1278.5487, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(24.41511, 1230.3849, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1549.1516, -1096.7822, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1048.739, -263.6272, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-1033.5317, 407.39743, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-1565.2981, 371.99908, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-299.8293, -515.1692, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-580.7771, -459.66437, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-991.4245, -489.27524, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-698.4489, -805.8975, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-988.84674, -752.30835, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1136.7373, -198.68887, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(905.7097, -401.8419, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(959.16864, -1044.877, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(720.9178, -1072.3549, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1004.6575, -1100.4719, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(871.9045, -1024.7054, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1144.2174, -955.9863, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(326.4054, -1110.6729, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(161.0278, -1017.0886, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-343.01788, -1069.6122, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-45.05844, -1319.6151, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-102.32437, -1084.9741, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1402.2433, -932.2621, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1699.7435, -781.79663, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1534.2288, -867.62146, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1658.138, -896.5046, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(758.57056, -855.24536, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(247.83372, 266.48444, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(413.80222, 420.81995, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-188.95137, 367.40338, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-760.5701, 210.7638, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-65.06914, 1268.306, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(142.74554, 1541.264, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(66.480316, 1552.2128, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(128.28676, 1390.9845, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(210.89793, 1363.7955, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(128.40121, 1207.6111, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-185.10374, 1413.84, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(217.7807, 1084.6246, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(800.5922, 1029.399, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(730.55023, 1055.3463, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(729.7301, 964.2094, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(716.46356, 859.69116, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(628.14954, 829.17236, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(598.14355, 994.5145, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(507.9314, 1012.8509, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(488.89612, 890.0678, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(590.3778, 1065.0519, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1032.2859, 713.8748, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1088.9661, 618.90454, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(840.3579, 523.81824, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1002.6559, 338.03946, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(940.60516, 304.9881, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(854.5827, 283.4762, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(789.36035, 410.28354, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(687.09174, 495.48444, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(695.00696, 369.5379, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1025.3762, 503.03183, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(853.2134, 443.55408, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(813.33136, 603.9299, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(842.19464, 717.2203, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(418.7144, 504.6021, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(334.07327, 262.5794, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(207.73456, 187.30176, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(143.54185, 217.00778, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(251.82468, 427.5459, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(306.8844, 659.8198, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(311.74432, 760.1316, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(311.84937, 833.6397, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(156.73969, 883.95715, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(178.1828, 656.2705, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(109.70869, 589.6072, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-114.71558, 559.62714, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(888.6051, -91.75964, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1054.03, -130.97856, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1106.9143, -47.11058, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(987.77985, -205.45639, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(887.7751, 28.770956, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(796.74854, 1.0474386, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(845.5109, -228.43335, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(856.4158, -360.62384, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(639.80585, -283.20215, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(626.2872, -153.13748, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(489.15668, -293.25952, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(279.72614, -338.77533, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-106.40291, -320.72565, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-48.316612, -531.3264, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(239.26645, -489.77914, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(84.21748, -66.33516, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(201.62555, -168.89366, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1958.1759, -989.389, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1906.3148, -1049.4329, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1786.692, -1074.6189, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1630.4935, -1067.8062, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1484.7883, -1015.9053, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1566.1393, -986.71594, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1901.2196, -908.078, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1810.442, -866.58044, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1739.2722, -1002.2713, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1108.1329, -665.37823, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1012.3336, -830.6617, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(900.1568, -601.7386, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(804.5891, -732.7149, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(965.3509, -671.92065, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1007.9303, -927.0324, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(888.7243, -893.7913, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(1121.4457, -817.2681, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(402.46695, -1102.1027, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(383.47946, -1187.8816, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(178.6919, -1284.6838, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-4.0912514, -1419.8612, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-5.0909195, -1511.2955, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-55.85026, -1191.089, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(15.081051, -1077.4604, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-213.55756, -1264.7078, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-211.35774, -1385.8774, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-388.45752, -1375.2385, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-487.09143, -1288.8687, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-458.93594, -1162.7411, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-249.5996, -1089.9309, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-160.80122, -999.4515, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-897.33295, -767.6719, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-1010.924, -646.13837, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-1011.9622, -534.7138, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-816.863, -444.52362, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-649.4022, -379.19302, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-713.77545, -479.20657, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-681.50397, -660.85364, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(92.35387, -573.5842, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(-39.52948, -163.19257, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(97.71949, -417.72394, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(436.43427, -238.49362, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(232.06198, 544.3733, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id2", "f_pilgrimroad_41_2", Spot(188.34137, 813.7944, 25));

		// Tiny_Mage_Green Spawn Points
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-519.8321, -1175.0718, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-240.25348, -1217.8875, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-295.93378, -1313.0371, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(81.64754, -1366.6693, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(44.20135, -1018.8702, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-285.69778, -990.3458, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-878.23944, -695.3463, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-1074.2216, -516.20703, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-893.32654, -336.85193, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-715.52673, -377.39075, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-780.60986, -705.5492, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-857.3717, -830.92615, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(784.0287, -241.49673, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(865.8402, -75.0632, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(1034.9142, -7.109215, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(942.8345, -144.14645, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(954.1928, -309.4952, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(1152.5199, -94.111786, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(713.92633, -986.91254, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(848.3188, -848.9559, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(999.2764, -729.1777, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(1030.8083, -945.61597, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(920.94116, -1131.8389, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(1109.7185, -1017.3293, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(1177.789, -714.741, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(680.4569, -832.60864, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(608.17365, -115.16011, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-497.7933, -1040.6404, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-327.87973, -541.55505, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-456.10602, -518.1753, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-535.24207, -1344.53, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(106.04343, -484.56723, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(403.6643, -394.4933, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-150.20825, -1222.2998, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(822.88324, -1065.2925, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(156.99341, -1122.5776, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(391.62228, -195.33815, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(252.48944, -508.23688, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(912.34143, -521.19617, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(878.1961, -615.12476, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(583.07245, -1006.2371, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(410.10965, -1039.1924, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(318.2342, -1203.682, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(157.86903, -1222.0017, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(22.66558, -1142.6394, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id3", "f_pilgrimroad_41_2", Spot(-121.35364, -1449.0533, 25));

		// Spion_Bow_Red Spawn Points
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(755.783, 411.6392, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(997.1337, 692.9042, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(1028.6875, 491.2278, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(1056.5391, 576.2807, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(468.66898, 530.5205, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(88.29474, 252.34843, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(-94.68484, 463.6948, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(242.60455, 480.99567, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(-48.863976, 546.1774, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(84.11013, 662.0541, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(-112.41797, 623.78534, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(220.67557, 849.0459, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(475.6434, 958.63165, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(652.2458, 1051.6909, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(727.12006, 959.9663, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(616.3071, 907.30975, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(563.26135, 841.8781, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(-160.33218, 1280.9509, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(69.83041, 1065.2256, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(128.1295, 1446.3961, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id4", "f_pilgrimroad_41_2", Spot(-102.24643, 1365.8533, 25));

		// Defender_Spider_Red Spawn Points
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-1685.9312, 224.2774, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-1696.9899, 405.5481, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-1613.2443, 612.1589, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-1369.4407, 533.3016, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-1448.3867, 351.64017, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-939.269, 435.38895, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-964.6052, 721.58545, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-633.1919, 731.8022, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-618.9247, 607.54694, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-763.6399, 503.0994, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-649.9668, 190.40193, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-558.1486, 276.63617, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(-393.54208, 383.835, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(1530.9056, -918.7797, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(431.596, -314.35083, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(270.15363, -121.23534, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(332.38596, -428.2957, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(19.98494, -618.10016, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(561.9059, -313.2956, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(460.73657, -159.76875, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id5", "f_pilgrimroad_41_2", Spot(178.09183, -576.7509, 25));

		// Tiny_Mage_Green Spawn Points
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-1590.4182, 276.9493, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-1560.1865, 489.21838, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-1763.9574, 513.72705, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-1340.851, 304.80878, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-1059.0358, 519.1095, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-1313.3768, 435.99957, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-836.0992, 203.69531, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-644.9945, 388.2304, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-771.25494, 404.30032, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-872.13074, 506.6847, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-740.5831, 854.66437, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-841.97125, 778.8861, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-501.69287, 466.7643, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-492.59207, 352.73386, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-153.49777, 523.8577, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-32.216797, 271.46866, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(187.01639, 321.91205, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(70.72558, 450.14386, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(52.148, 752.8157, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-192.53027, 631.52783, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-235.10387, 751.4029, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(332.6468, 542.3363, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(503.12158, 609.5084, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(769.54645, 555.5989, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(725.824, 345.75146, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(908.4002, 336.39844, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(1090.2146, 650.36975, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(810.5123, 751.74536, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(703.3178, 583.4205, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(781.5405, 657.65735, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(8.344716, 360.94794, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(96.617355, 840.3935, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-151.11166, 868.33136, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(85.74794, 1170.4025, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-163.73, 1216.8717, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-201.23149, 1350.1722, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-12.101133, 1260.1989, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(137.46898, 1330.8611, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(21.102757, 1475.1327, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(197.1707, 1494.6978, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(244.25116, 763.02716, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(244.78493, 603.05096, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(550.04364, 453.10846, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(977.97797, 443.21924, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(283.10406, 182.71643, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-954.5212, 289.0394, 25));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id6", "f_pilgrimroad_41_2", Spot(-1562.2148, 197.30771, 25));

		// Spion_Red Spawn Points
		AddSpawnPoint( "f_pilgrimroad_41_2.Id7", "f_pilgrimroad_41_2", Spot(-727.87946, 464.16986, 250));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id7", "f_pilgrimroad_41_2", Spot(159.9913, 479.83084, 250));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id7", "f_pilgrimroad_41_2", Spot(-807.52893, -609.2866, 250));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id7", "f_pilgrimroad_41_2", Spot(-326.38278, -1176.274, 250));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id7", "f_pilgrimroad_41_2", Spot(864.2962, -890.82275, 250));
		AddSpawnPoint( "f_pilgrimroad_41_2.Id7", "f_pilgrimroad_41_2", Spot(887.29767, -175.85577, 250));

	}
}