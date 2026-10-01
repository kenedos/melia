function M_CHASE_CLEAR(frame)
	local ctrl = GET_CHILD(frame, "member", "ui::CGroupBox")
	if ctrl ~= nil then
		ctrl:DeleteAllControl()
	end
end

-- The game's custom-quest toggle is unused here.
function M_CHASE_HIDE_CUSTOM_OPTION(frame)
	local txt = GET_CHILD(frame, "quest_custom_name")
	local chk = GET_CHILD(frame, "quest_custom")
	if txt ~= nil then txt:ShowWindow(0) end
	if chk ~= nil then chk:ShowWindow(0) end
end

-- Client quests toggle the tracker the game's own scripts draw.
function M_CHASE_UPDATE_CLIENT(frame, ctrl, argStr, argNum)
	local questInfo = Melia.Quests.Get(argStr)
	if questInfo == nil or questInfo.ClientId == nil then
		return
	end

	tolua.cast(ctrl, "ui::CCheckBox")

	if ctrl:IsChecked() == 1 then
		quest.AddCheckQuest(questInfo.ClientId)

		if quest.GetCheckQuestCount() > 5 then
			ctrl:SetCheck(0)
			quest.RemoveCheckQuest(questInfo.ClientId)
			ui.SysMsg(ClMsg('OutOfQuestCheckCount'))
		end
	else
		quest.RemoveCheckQuest(questInfo.ClientId)
	end

	ON_UPDATE_QUESTINFOSET_2()
end
