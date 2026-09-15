M_QUESTS_SECTION_PREFIX = "_QSection_"

-- Quests ready to hand in float to the top of their section, as in the game.
function M_QUESTS_SORT_SECTION(quests)
	table.sort(quests, function(a, b)
		if a.Done ~= b.Done then
			return a.Done
		end
		if a.Level ~= b.Level then
			return a.Level < b.Level
		end
		return (a.Name or "") < (b.Name or "")
	end)
end

-- The click lands on whichever part of the header the control set forwards.
function M_QUESTS_TOGGLE_SECTION(ctrl)
	local prefixLength = string.len(M_QUESTS_SECTION_PREFIX)

	while ctrl ~= nil do
		local name = ctrl:GetName()

		if name ~= nil and string.sub(name, 1, prefixLength) == M_QUESTS_SECTION_PREFIX then
			local typeName = string.sub(name, prefixLength + 1)
			M_QUESTS_COLLAPSED[typeName] = not M_QUESTS_COLLAPSED[typeName]
			M_QUESTS_UPDATE_LIST()
			return
		end

		ctrl = ctrl:GetParent()
	end
end
