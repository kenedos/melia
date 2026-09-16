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
