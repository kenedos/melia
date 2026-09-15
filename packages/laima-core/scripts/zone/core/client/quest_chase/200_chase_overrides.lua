Melia.Override("QUESTINFOSET_2_QUEST_ANGLE", function(original, frame, msg, argStr, argNum)
	-- Block the original function completely - it tries to hide/show the chase window
	-- during movement which causes flickering. We manage visibility ourselves.
	-- Do nothing here.
end)

Melia.Override("CHASEINFO_CLOSE_FRAME", function(original)
	-- Block the game from closing questinfoset_2 during dash/skills
	-- The original function closes chaseinfo, achieveinfoset, and questinfoset_2
	-- We manage questinfoset_2 visibility ourselves, so only close the others
	ui.CloseFrame("chaseinfo")
	ui.CloseFrame("achieveinfoset")
	-- Don't close questinfoset_2 - we manage it ourselves
end)
