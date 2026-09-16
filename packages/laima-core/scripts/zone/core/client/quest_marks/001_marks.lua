Melia.QuestMarks = {}

Melia.QuestMarks.Scale = 1.5
Melia.QuestMarks.Node = "Dummy_bufficon"
Melia.QuestMarks.Marks = {}
Melia.QuestMarks.Applied = {}

Melia.QuestMarks.Set = function(marks, resets)
	Melia.QuestMarks.Marks = marks or {}
	M_QUESTMARKS_RESET(resets)
	M_QUESTMARKS_APPLY()
end

function M_QUESTMARKS_GET_ICON(markType)
	if markType == 1 then
		return "I_quest_mask_progress"
	elseif markType == 2 then
		return "I_quest_mask_possible"
	elseif markType == 3 then
		return "I_quest_mask_success"
	end

	return nil
end
