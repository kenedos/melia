-- The client builds this list from its own quest tables, which know nothing about ours.
Melia.Override("OPEN_QUESTWARP_FRAME", function(original, frame)
	local quests = M_CHASE_GET_RETURNABLE()

	if #quests == 0 then
		return original(frame)
	end

	M_CHASE_WARP_FILL(frame, quests)

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

-- The client's list and ours share the frame, so only object-id entries are ours.
local function M_CHASE_WARP_IS_OURS(frame, index)
	if frame == nil then
		return false
	end

	local classId = frame:GetUserValue("QUEST_WARP_CLASSID_" .. index)
	return classId ~= nil and string.find(tostring(classId), "^0x") ~= nil
end

Melia.Override("QUESTWARP_QUESTID", function(original, frame, control, argStr, index)
	local warpFrame = ui.GetFrame("questwarp")

	if not M_CHASE_WARP_IS_OURS(warpFrame, index) then
		return original(frame, control, argStr, index)
	end

	M_CHASE_WARP_BY_INDEX(warpFrame, index)
end)

Melia.Override("QUESTWARP_ON_MSG", function(original, frame, msg, argStr, argNum)
	if msg ~= "QUESTWARPSELECT_SELECT" then
		return original(frame, msg, argStr, argNum)
	end

	if not M_CHASE_WARP_IS_OURS(frame, QuestWarpSelect_index) then
		return original(frame, msg, argStr, argNum)
	end

	M_CHASE_WARP_BY_INDEX(frame, QuestWarpSelect_index)
end)
