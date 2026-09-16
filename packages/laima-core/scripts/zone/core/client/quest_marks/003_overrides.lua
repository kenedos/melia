Melia.Override("UPDATE_QUESTMARK", function(original, frame, msg, argStr, argNum)

	M_QUESTMARKS_APPLY(msg == "NPC_ENTER")
end)
