Melia.QuestMarks = {}

Melia.QuestMarks.Scale = 2.0
Melia.QuestMarks.Height = 45
Melia.QuestMarks.Marks = {}
Melia.QuestMarks.Applied = {}

Melia.QuestMarks.Set = function(marks, resets)
	Melia.QuestMarks.Marks = marks or {}
	M_QUESTMARKS_RESET(resets)
	M_QUESTMARKS_APPLY()
end

Melia.QuestMarks.MaskIcons = {}

for _, state in ipairs({ "possible", "progress", "success" }) do
	for _, kind in ipairs({ "", "_sub", "_repeat", "_period", "_party", "_key", "_key_old" }) do
		table.insert(Melia.QuestMarks.MaskIcons, "I_quest_mask_" .. state .. kind)
	end
end

function M_QUESTMARKS_DETACH_MASKS(actor)
	local icons = Melia.QuestMarks.MaskIcons
	for i = 1, #icons do
		effect.DetachActorEffect(actor, icons[i], 0)
	end
end
