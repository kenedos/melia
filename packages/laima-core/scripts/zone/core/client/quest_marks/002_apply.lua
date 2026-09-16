function M_QUESTMARKS_RESET(resets)
	if resets == nil then
		return
	end

	local applied = Melia.QuestMarks.Applied

	for i = 1, #resets do
		local handle = resets[i]
		local icon = applied[handle]
		local actor = world.GetActor(handle)

		if actor ~= nil and icon ~= nil then
			effect.DetachActorEffect(actor, icon, 0)
		end

		applied[handle] = nil
	end
end

function M_QUESTMARKS_APPLY()
	local marks = Melia.QuestMarks.Marks
	local applied = Melia.QuestMarks.Applied

	for i = 1, #marks do
		local mark = marks[i]
		local icon = M_QUESTMARKS_GET_ICON(mark.Type)

		if icon ~= nil and applied[mark.Handle] ~= icon then
			local actor = world.GetActor(mark.Handle)

			if actor ~= nil then
				effect.PlayActorEffect(actor, icon, Melia.QuestMarks.Node, 0, Melia.QuestMarks.Scale)
				applied[mark.Handle] = icon
			end
		end
	end
end
