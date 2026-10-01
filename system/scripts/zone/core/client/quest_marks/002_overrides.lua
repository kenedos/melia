-- Replaces the client's own evaluator, which resets every quest NPC it knows to no marker.
Melia.Override("UPDATE_QUESTMARK", function(original, frame, msg, argStr, argNum)
	M_QUESTMARKS_APPLY()
end)
