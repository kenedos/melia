function M_QUESTS_DETAILS_ADD_BUTTONS(frame, x, y, quest)
	local height = 0

	x = frame:GetWidth() / 2 - (160 / 2)

	if quest.Done then
		if Melia.Conf.GetBool("quest_completion_from_ui") then
			height = height + M_QUESTS_DETAILS_ADD_BUTTON(frame, x, y + height, "QuestCompleteButton", "Complete", { skin = "test_red_button", tooltip = "Complete the quest.", onLBtnDown = "M_QUESTS_COMPLETE", onLBtnDownArgStr = quest.ObjectId })
		elseif Melia.Conf.GetBool("quest_return_button") then
			height = height + M_QUESTS_DETAILS_ADD_BUTTON(frame, x, y + height, "QuestWarpButton", "Return", { skin = "test_gray_button", tooltip = "Warp back to the quest's NPC.", onLBtnDown = "M_QUESTS_WARP", onLBtnDownArgStr = quest.ObjectId })
		end
	elseif quest.Cancelable then
		height = height + M_QUESTS_DETAILS_ADD_BUTTON(frame, x, y + height, "QuestCancelButton", "Abandon", { skin = "test_gray_button", tooltip = "Abandon the quest.", onLBtnDown = "M_QUESTS_CANCEL", onLBtnDownArgStr = quest.ObjectId })
	end

	return height
end
