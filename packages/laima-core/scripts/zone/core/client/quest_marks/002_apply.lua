local QuestMaskIcons = {
	"I_quest_mask_possible", "I_quest_mask_possible_sub", "I_quest_mask_possible_repeat",
	"I_quest_mask_possible_period", "I_quest_mask_possible_party", "I_quest_mask_possible_key",
	"I_quest_mask_possible_key_old",
	"I_quest_mask_progress", "I_quest_mask_progress_sub", "I_quest_mask_progress_repeat",
	"I_quest_mask_progress_period", "I_quest_mask_progress_party", "I_quest_mask_progress_key",
	"I_quest_mask_progress_key_old",
	"I_quest_mask_success", "I_quest_mask_success_sub", "I_quest_mask_success_repeat",
	"I_quest_mask_success_period", "I_quest_mask_success_party", "I_quest_mask_success_key",
	"I_quest_mask_success_key_old",
}

local function DetachQuestMasks(actor)
	for i = 1, #QuestMaskIcons do
		effect.DetachActorEffect(actor, QuestMaskIcons[i], 0)
	end
end

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
				DetachQuestMasks(actor)

				local pos = actor:GetPos()
				effect.AddActorEffect(actor, icon, Melia.QuestMarks.Scale, pos.x, pos.y + Melia.QuestMarks.Height, pos.z, -1)
				applied[mark.Handle] = icon
			end
		end
	end
end
