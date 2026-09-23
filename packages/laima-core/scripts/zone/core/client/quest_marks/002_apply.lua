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

function M_QUESTMARKS_APPLY(force)
	local marks = Melia.QuestMarks.Marks
	local applied = Melia.QuestMarks.Applied

	local wanted = {}

	for i = 1, #marks do
		wanted[marks[i].Handle] = marks[i].Icon
	end

	for handle, icon in pairs(applied) do
		if wanted[handle] ~= icon then
			local actor = world.GetActor(handle)

			if actor ~= nil then
				effect.DetachActorEffect(actor, icon, 0)
			end

			applied[handle] = nil
		end
	end

	for i = 1, #marks do
		local mark = marks[i]
		local icon = mark.Icon

		if icon ~= nil and (force or applied[mark.Handle] ~= icon) then
			local actor = world.GetActor(mark.Handle)

			if actor ~= nil then
				M_QUESTMARKS_DETACH_MASKS(actor)

				local pos = actor:GetPos()
				effect.AddActorEffect(actor, icon, Melia.QuestMarks.Scale, pos.x, pos.y + Melia.QuestMarks.Height, pos.z, -1)
				applied[mark.Handle] = icon
			end
		end
	end
end
