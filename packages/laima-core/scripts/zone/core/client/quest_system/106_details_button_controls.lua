function M_QUESTS_DETAILS_ADD_BUTTON(frame, x, y, name, text, args)
	if args.skin == nil then args.skin = "test_gray_button" end

	local button = frame:CreateControl("button", name, x, y, 160, 50)
	AUTO_CAST(button);

	button:SetSkinName(args.skin)
	button:SetOverSound("button_over")
	button:SetClickSound("button_click_big")
	button:SetText("{@st41b}" .. text .. "{/}")

	if args.tooltip ~= nil then
		button:SetTextTooltip("{@st59}" .. args.tooltip .. "{/}")
	end

	if args.onLBtnDown ~= nil then
		button:SetEventScript(ui.LBUTTONDOWN, args.onLBtnDown)
	end
	if args.onLBtnDownArgNum ~= nil then
		button:SetEventScriptArgNumber(ui.LBUTTONDOWN, args.onLBtnDownArgNum)
	end
	if args.onLBtnDownArgStr ~= nil then
		button:SetEventScriptArgString(ui.LBUTTONDOWN, args.onLBtnDownArgStr)
	end

	return button:GetHeight()
end

function M_QUESTS_COMPLETE(frame, control, argStr, argNum)
	local questObjectId = argStr
	Melia.Quests.RequestComplete(questObjectId)
end

function M_QUESTS_WARP(frame, control, argStr, argNum)
	local questObjectId = argStr
	Melia.Quests.RequestWarp(questObjectId)
end

function M_QUESTS_CANCEL(frame, control, argStr, argNum)
	local questObjectId = argStr
	Melia.Quests.RequestCancel(questObjectId)
end
