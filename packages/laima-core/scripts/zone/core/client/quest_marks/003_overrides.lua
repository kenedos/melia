Melia.Override("UPDATE_QUESTMARK", function(original, frame, msg, argStr, argNum)
	-- The client destroys and recreates NPC actors as they leave and
	-- re-enter view, taking their attached effects with them.
	if msg == "NPC_ENTER" then
		Melia.QuestMarks.Applied = {}
	end

	M_QUESTMARKS_APPLY()
end)
