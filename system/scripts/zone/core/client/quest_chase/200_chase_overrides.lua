Melia.Override("QUESTINFOSET_2_QUEST_ANGLE", function(original, frame, msg, argStr, argNum)
	local frm = ui.GetFrame("questinfoset_2")

	if frm ~= nil and frm:IsVisible() == 0 and M_CHASE_GET_TRACKED ~= nil and #M_CHASE_GET_TRACKED() > 0 then
		M_CHASE_UPDATE_VISIBILITY()
	end
end)

Melia.Override("CHASEINFO_CLOSE_FRAME", function(original)
	ui.CloseFrame("chaseinfo")
	ui.CloseFrame("achieveinfoset")
end)

-- The client's rebuild clears the whole list, so append once it has drawn.
Melia.Override("ON_UPDATE_QUESTINFOSET_2", function(original, frame, msg, check, updateQuestID)
	original(frame, msg, check, updateQuestID)

	local frm = frame
	if frm == nil then
		frm = ui.GetFrame("questinfoset_2")
	end

	if frm == nil then
		return
	end

	M_CHASE_HIDE_CUSTOM_OPTION(frm)

	local hasTrackedQuests = M_CHASE_GET_TRACKED ~= nil and #M_CHASE_GET_TRACKED() > 0
	local hasClientQuests = quest.GetCheckQuestCount() > 0

	-- A list the client hides is left uncleared, since it no longer closes the frame.
	if not (hasTrackedQuests or (hasClientQuests and CHASEINFO_IS_SHOW() == 1)) then
		M_CHASE_CLEAR(frm)
		frm:ShowWindow(0)
		return
	end

	if hasTrackedQuests then
		M_CHASE_REDRAW(frm)
	end

	frm:ShowWindow(1)
end)
