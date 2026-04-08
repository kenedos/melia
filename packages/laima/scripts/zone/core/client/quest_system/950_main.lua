function M_QUESTS_UPDATE_LIST()
	local questFrame = ui.GetFrame("quest")
	local gbBody = GET_CHILD_RECURSIVELY(questFrame, "gb_body", "ui::CGroupBox")
	local quests = Melia.Quests.GetAll()

	M_QUESTS_DRAW_LIST(gbBody, quests)
	M_CHASE_UPDATE_VISIBILITY()
end

M_QUESTS_UPDATE_LIST()
