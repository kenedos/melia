-- The client's WARP state floats the character up and runs the command itself
-- at the end of the animation, about 1.8 seconds in.
function M_QUESTS_WARP_START(questObjectId)
	local quest = Melia.Quests.Get(questObjectId)
	if quest == nil or not quest.Done then
		return
	end

	if control.IsRestSit() == true then
		ui.SysMsg(ClMsg("DontQuestWarpForSit"))
		return
	end

	if world.GetLayer() ~= 0 then
		return
	end

	local isMoveMap = 0
	if quest.WarpMap ~= nil and quest.WarpMap ~= session.GetMapName() then
		isMoveMap = 1
	end

	movie.QuestWarp(session.GetMyHandle(), "/quest warp " .. questObjectId, isMoveMap)
	packet.ClientDirect("QuestWarp")

	if _JANSORI_SET_NOTIFIED ~= nil then
		_JANSORI_SET_NOTIFIED("Quest_Returnable")
	end
end
