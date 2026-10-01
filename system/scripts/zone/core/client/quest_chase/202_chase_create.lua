M_CHASE_ICON_SIZE = 32
M_CHASE_TITLE_X = 40

function M_CHASE_REDRAW(frame)
	local ctrl = GET_CHILD(frame, "member", "ui::CGroupBox")

	M_CHASE_HIDE_CUSTOM_OPTION(frame)
	M_CHASE_REMOVE_TRACKED(ctrl)

	if not Melia.Conf.GetBool("display_quest_objectives") then
		return
	end

	local quests = M_CHASE_GET_TRACKED()
	local y = 0

	for i = 1, #quests do
		y = y + M_CHASE_CREATE_QUEST(frame, ctrl, quests[i], 0, y)
	end

	QUESTINFOSET_2_AUTO_ALIGN(frame, ctrl)
end

-- Entries this system draws are named after the quest's object id, so the
-- client's own tracker controls are left untouched.
function M_CHASE_REMOVE_TRACKED(ctrl)
	local index = 0

	while index < ctrl:GetChildCount() do
		local child = ctrl:GetChildByIndex(index)
		local name = child ~= nil and child:GetName() or nil

		if name ~= nil and string.find(name, "^_Q_0x") ~= nil then
			ctrl:RemoveChild(name)
		else
			index = index + 1
		end
	end
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
