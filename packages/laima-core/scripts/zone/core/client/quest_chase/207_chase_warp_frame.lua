-- The client builds this list from its own quest tables, which know nothing about ours.
Melia.Override("OPEN_QUESTWARP_FRAME", function(original, frame)
	local quests = M_CHASE_GET_RETURNABLE()

	M_CHASE_WARP_FILL(frame, quests)

	if #quests == 0 then
		frame:ShowWindow(0)
		return
	end

	QuestWarpSelect_index = 0
	QuestWarpMaxCount = #quests

	if #quests == 1 then
		M_CHASE_WARP_BY_INDEX(frame, 0)
		return
	end

	QUESTWARP_ITEM_SELECT(frame)
	frame:Resize(frame:GetWidth(), #quests * 40 + 130)
	frame:ShowWindow(1)
end)

Melia.Override("QUESTWARP_QUESTID", function(original, frame, control, argStr, index)
	M_CHASE_WARP_BY_INDEX(ui.GetFrame("questwarp"), index)
end)

Melia.Override("QUESTWARP_ON_MSG", function(original, frame, msg, argStr, argNum)
	if msg ~= "QUESTWARPSELECT_SELECT" then
		return original(frame, msg, argStr, argNum)
	end

	M_CHASE_WARP_BY_INDEX(frame, QuestWarpSelect_index)
end)
