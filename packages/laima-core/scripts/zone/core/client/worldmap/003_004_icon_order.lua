function M_IS_QUEST_MAP_ICON(icon)
	return icon.Image ~= nil and string.find(icon.Image, "^minimap_[123]_") ~= nil
end

function M_DRAW_MAP_ICONS(parent, icons, mapName, mapProp, prefix, width, height, offsetX, offsetY)
	local names = {}
	local quests = {}
	local lowerMapName = string.lower(mapName)

	for pass = 1, 2 do
		for i, icon in ipairs(icons) do
			local isQuest = M_IS_QUEST_MAP_ICON(icon)

			if string.lower(icon.Map) == lowerMapName and isQuest == (pass == 2) then
				local ctrlName = prefix .. i
				local ctrl = M_CREATE_MAP_ICON(parent, ctrlName, icon, mapProp, width, height, offsetX, offsetY)

				if ctrl ~= nil then
					ctrl:ShowWindow(1)
					if isQuest then
						quests[#quests + 1] = ctrl
					end
				end

				names[#names + 1] = ctrlName
			end
		end
	end

	for i = 1, #quests do
		quests[i]:MakeTopBetweenChild()
	end

	return names
end
