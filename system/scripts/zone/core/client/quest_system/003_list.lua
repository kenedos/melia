-- Colors match the quest_list_oneline control set's own userconfig.
M_QUEST_TYPE_STYLE = {
	Main    = { icon = "MAIN",     color = "{#ff9b0d}", name = "Main Quest",     label = "Main Quests" },
	Sub     = { icon = "SUB",      color = "{#09bcff}", name = "Side Quest",     label = "Side Quests" },
	Repeat  = { icon = "REPEAT",   color = "{#55EE55}", name = "Repeatable",     label = "Repeatable" },
	Party   = { icon = "PARTY",    color = "{#FF8800}", name = "Party Quest",    label = "Party Quests" },
	KeyItem = { icon = "KEYQUEST", color = "{#ff6fa2}", name = "Key Item Quest", label = "Key Item Quests" },
}

M_QUEST_TYPE_ORDER = { "Main", "Sub", "Repeat", "Party", "KeyItem" }

function M_QUESTS_GET_STYLE(quest)
	return M_QUEST_TYPE_STYLE[quest.Type] or M_QUEST_TYPE_STYLE.Sub
end

-- minimap_2_* is in progress, minimap_3_* is ready to hand in.
function M_QUESTS_GET_ICON(quest)
	local state = quest.Done and 3 or 2
	return "minimap_" .. state .. "_" .. M_QUESTS_GET_STYLE(quest).icon
end
