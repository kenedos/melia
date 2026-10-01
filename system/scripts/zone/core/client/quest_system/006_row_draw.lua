function M_QUESTS_DRAW_QUEST(parent, quest, i, x, y)
	local questCtrl = parent:CreateOrGetControlSet("quest_list_oneline", "_Q_" .. quest.ObjectId, x, y)
	AUTO_CAST(questCtrl)

	if i % 2 == 0 then
		questCtrl:SetSkinName("chat_window_2")
	else
		questCtrl:SetSkinName("None")
	end

	M_QUESTS_SET_NAME(questCtrl, quest)
	M_QUESTS_SET_ICON(questCtrl, quest)
	M_QUESTS_SET_BUTTONS(questCtrl, quest)
	M_QUESTS_SET_CHASE(questCtrl, quest)

	questCtrl:SetEventScript(ui.LBUTTONDOWN, "M_QUESTS_CLICK_INFO")
	questCtrl:SetEventScriptArgString(ui.LBUTTONDOWN, quest.ObjectId)

	return questCtrl:GetHeight()
end

function M_QUESTS_SET_NAME(questCtrl, quest)
	local txtName = GET_CHILD(questCtrl, "name", "ui::CRichText")
	local lvlText = GET_CHILD(questCtrl, "level", "ui::CRichText")

	txtName:SetText("{@s16}" .. M_QUESTS_GET_STYLE(quest).color .. quest.Name)
	lvlText:SetText("{#ffffff}Lv " .. quest.Level)
end

function M_QUESTS_SET_ICON(questCtrl, quest)
	local questmark = GET_CHILD(questCtrl, "questmark", "ui::CPicture")
	local style = M_QUESTS_GET_STYLE(quest)
	local state = " - in progress.{/}"

	if quest.Done then
		state = " - ready to hand in.{/}"
	end

	questmark:EnableHitTest(1)
	questmark:SetTextTooltip("{@st59}" .. style.name .. state)
	questmark:SetImage(M_QUESTS_GET_ICON(quest))
	questmark:ShowWindow(1)
end
