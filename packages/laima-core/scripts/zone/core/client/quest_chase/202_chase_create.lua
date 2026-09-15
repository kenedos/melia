M_CHASE_ICON_SIZE = 32
M_CHASE_TITLE_X = 40

function M_CHASE_REDRAW(frame)
	local ctrl = GET_CHILD(frame, "member", "ui::CGroupBox")

	local txtCustomName = GET_CHILD(frame, "quest_custom_name")
	local chkCustomName = GET_CHILD(frame, "quest_custom")
	txtCustomName:ShowWindow(0)
	chkCustomName:ShowWindow(0)

	ctrl:DeleteAllControl();

	if not Melia.Conf.GetBool("display_quest_objectives") then
		return
	end

	local quests = M_CHASE_GET_TRACKED()
	local y = 0

	for i = 1, #quests do
		y = y + M_CHASE_CREATE_QUEST(frame, ctrl, quests[i], 0, y)
	end

	frame:Invalidate()
end

function M_CHASE_CREATE_QUEST(frame, ctrl, quest, x, y)
	local width = frame:GetWidth() - x - SCROLL_WIDTH
	local titleX = M_CHASE_TITLE_X

	local ctrlQuest = ctrl:CreateOrGetControlSet("emptyset2", "_Q_" .. quest.ObjectId, x, y)
	ctrlQuest = tolua.cast(ctrlQuest, "ui::CControlSet")

	ctrlQuest:SetSkinName(frame:GetUserConfig("CTRLSETSKINNAME"))
	ctrlQuest:Resize(width, 30)

	M_CHASE_SET_STATE_ICON(ctrlQuest, quest)

	local lblTitle = ctrlQuest:CreateOrGetControl("richtext", "title", titleX, 0, width - titleX, 30)
	lblTitle:SetText(QUEST_TITLE_FONT .. M_QUESTS_GET_STYLE(quest).color .. quest.Name)
	lblTitle:EnableHitTest(0)

	local titleHeight = lblTitle:GetHeight()
	if titleHeight < M_CHASE_ICON_SIZE then
		titleHeight = M_CHASE_ICON_SIZE
	end

	local objectivesHeight = M_CHASE_CREATE_OBJECTIVES(ctrlQuest, quest, titleX - 10, titleHeight + 5)
	local height = titleHeight + objectivesHeight + 3

	ctrlQuest:Resize(width, height)

	return height
end
