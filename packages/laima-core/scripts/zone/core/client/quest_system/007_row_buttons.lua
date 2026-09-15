function M_QUESTS_SET_BUTTONS(questCtrl, quest)
	local shareParty = GET_CHILD_RECURSIVELY(questCtrl, "shareParty")
	local questPositionCheck = GET_CHILD_RECURSIVELY(questCtrl, "questPositionCheck")
	local abandonquest_try = GET_CHILD_RECURSIVELY(questCtrl, "abandonquest_try")
	local dialogReplay = GET_CHILD_RECURSIVELY(questCtrl, "dialogReplay")
	local abandon = GET_CHILD_RECURSIVELY(questCtrl, "abandon")

	shareParty:ShowWindow(0)
	questPositionCheck:ShowWindow(0)
	abandonquest_try:ShowWindow(0)
	dialogReplay:ShowWindow(0)
	abandon:ShowWindow(0)
end
