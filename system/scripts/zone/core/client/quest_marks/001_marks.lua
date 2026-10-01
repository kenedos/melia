Melia.QuestMarks = { Marks = {} }

Melia.QuestMarks.Begin = function()
	Melia.QuestMarks.Pending = {}
end

Melia.QuestMarks.Add = function(marks)
	for name, icon in pairs(marks) do
		Melia.QuestMarks.Pending[name] = icon
	end
end

Melia.QuestMarks.Commit = function()
	local marks = Melia.QuestMarks.Pending or {}

	for name, _ in pairs(Melia.QuestMarks.Marks) do
		if marks[name] == nil then
			quest.QuestUpdate(name, "None")
		end
	end

	Melia.QuestMarks.Marks = marks
	Melia.QuestMarks.Pending = nil

	M_QUESTMARKS_APPLY()
end

function M_QUESTMARKS_APPLY()
	for name, icon in pairs(Melia.QuestMarks.Marks) do
		quest.QuestUpdate(name, icon)
	end
end
