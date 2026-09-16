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
