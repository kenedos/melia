M_CHASE_WARP_MAX_BTN = 10

function M_CHASE_WARP(frame, ctrl, argStr, argNum)
	Melia.Quests.RequestWarp(argStr)
end

function M_CHASE_WARP_BY_INDEX(frame, index)
	local questObjectId = frame:GetUserValue("QUEST_WARP_CLASSID_" .. index)

	frame:ShowWindow(0)

	if questObjectId == nil or questObjectId == "None" then
		return
	end

	M_CHASE_WARP(frame, nil, questObjectId, 0)
end

function M_CHASE_WARP_FILL(frame, quests)
	for i = 0, M_CHASE_WARP_MAX_BTN - 1 do
		frame:SetUserValue("QUEST_WARP_CLASSNAME_" .. i, "None")
		frame:SetUserValue("QUEST_WARP_CLASSID_" .. i, "None")

		local button = frame:GetChild("warp" .. i .. "btn")
		if button ~= nil then
			button:ShowWindow(0)
		end
	end

	for i = 1, #quests do
		local quest = quests[i]
		local index = i - 1

		frame:SetUserValue("QUEST_WARP_CLASSNAME_" .. index, quest.Name)
		frame:SetUserValue("QUEST_WARP_CLASSID_" .. index, quest.ObjectId)

		local button = frame:GetChild("warp" .. index .. "btn")
		button:SetText("{@st66b}" .. M_QUESTS_GET_STYLE(quest).color .. "{ol}" .. quest.Name .. "{/}")
		button:ShowWindow(1)
	end
end
