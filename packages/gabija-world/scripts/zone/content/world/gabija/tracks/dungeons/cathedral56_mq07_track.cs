//--- Melia Script ----------------------------------------------------------
// The Sealed Door
//--- Description -----------------------------------------------------------
// Two candlesticks hold the barrier on the sealed door, and a Pawndel keeps them.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CHATHEDRAL56_MQ07_TRACK")]
public class Cathedral56Mq07Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHATHEDRAL56_MQ07_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		var candle1 = AddTrackNpc(character, 147358, L("Barrier Candlestick"), "d_cathedral_56", -2093.69, 0, -609.01, 0);
		candle1.SetClickTrigger("DYNAMIC_DIALOG", dialog => DCathedral56QuestNpcsScript.BarrierCandleDialog(dialog, 1));
		actors.Add(candle1);

		var candle2 = AddTrackNpc(character, 147358, L("Barrier Candlestick"), "d_cathedral_56", -2096.10, 0, -372.86, 0);
		candle2.SetClickTrigger("DYNAMIC_DIALOG", dialog => DCathedral56QuestNpcsScript.BarrierCandleDialog(dialog, 2));
		actors.Add(candle2);

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the Pawndel that keeps the barrier candlesticks.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("MON_GEN")
			.Monster(57371, -1825.65, 0, -562.81, -172)
			.On(s => s.Alive(0) <= 0, s => s.Spawn(0, 1));

		game.Start("MON_GEN");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 3:
				StartMinigame(character, track);
				break;
			case 4:
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
