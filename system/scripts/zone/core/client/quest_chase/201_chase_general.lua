function M_QUESTS_SET_CHASE(questCtrl, questInfo)
	local chkChase = GET_CHILD(questCtrl, "chase", "ui::CCheckBox")
	
	if not Melia.Conf.GetBool("display_quest_objectives") then
		chkChase:ShowWindow(0)
		return
	end

	-- Quests the client knows resolve through its own tracker.
	if questInfo.ClientId ~= nil then
		if quest.IsCheckQuest(questInfo.ClientId) then
			chkChase:SetCheck(1)
		end

		chkChase:SetEventScript(ui.LBUTTONDOWN, "M_CHASE_UPDATE_CLIENT")
		chkChase:SetEventScriptArgString(ui.LBUTTONDOWN, questInfo.ObjectId)
		return
	end

	if questInfo.Tracked then
		chkChase:ToggleCheck()
	end

	chkChase:SetEventScript(ui.LBUTTONDOWN, "M_CHASE_UPDATE")
	chkChase:SetEventScriptArgString(ui.LBUTTONDOWN, questInfo.ObjectId)
end

function M_CHASE_UPDATE(frame, ctrl, argStr, argNum, notUpdateRightUI)
	local questObjectId = argStr
	local questInfo = Melia.Quests.Get(questObjectId)

	tolua.cast(ctrl, "ui::CCheckBox")
	if ctrl:IsChecked() == 1 then
		questInfo.Tracked = true
	else
		questInfo.Tracked = false
	end

	Melia.Quests.RequestTrack(questObjectId, questInfo.Tracked)

	M_CHASE_UPDATE_VISIBILITY()
end

function M_CHASE_UPDATE_VISIBILITY()
	local frmQuestInfo = ui.GetFrame("questinfoset_2")
	M_CHASE_HIDE_CUSTOM_OPTION(frmQuestInfo)

	local hasTrackedQuests = #M_CHASE_GET_TRACKED() > 0

	if hasTrackedQuests then
		M_CHASE_REDRAW(frmQuestInfo)
	end

	if hasTrackedQuests or quest.GetCheckQuestCount() > 0 then
		frmQuestInfo:ShowWindow(1)
	else
		M_CHASE_CLEAR(frmQuestInfo)
		frmQuestInfo:ShowWindow(0)
	end
end
