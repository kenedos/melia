function M_QUESTS_DRAW_SECTION(frame, typeName, quests, y)
	local style = M_QUEST_TYPE_STYLE[typeName]
	local name = M_QUESTS_SECTION_PREFIX .. typeName

	local sectionCtrl = frame:CreateOrGetControlSet("quest_list_title", name, 0, y)
	sectionCtrl = tolua.cast(sectionCtrl, "ui::CControlSet")

	local nameText = GET_CHILD_RECURSIVELY(sectionCtrl, "questMapNameText")
	nameText:SetTextByKey("mapName", style.label)
	nameText:SetTextByKey("count", #quests)

	local collapsed = M_QUESTS_COLLAPSED[typeName] == true
	local markKey = "CLOSED_CTRL_IMAGE"
	if collapsed then
		markKey = "OPENED_CTRL_IMAGE"
	end

	local openMark = GET_CHILD_RECURSIVELY(sectionCtrl, "openMark")
	openMark:SetImage(sectionCtrl:GetUserConfig(markKey))

	local listGbox = GET_CHILD_RECURSIVELY(sectionCtrl, "questListGbox")
	local listHeight = 0

	if not collapsed then
		for i = 1, #quests do
			listHeight = listHeight + M_QUESTS_DRAW_QUEST(listGbox, quests[i], i, 5, listHeight)
		end
	end

	listGbox:Resize(listGbox:GetWidth(), listHeight)
	sectionCtrl:Resize(sectionCtrl:GetWidth(), sectionCtrl:GetHeight() + listHeight)
	sectionCtrl:Invalidate()

	return sectionCtrl:GetHeight()
end
