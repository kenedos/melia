-- Named "statepicture" so the client's own return-warp list and hint find it.
function M_CHASE_SET_STATE_ICON(ctrlQuest, quest)
	local size = M_CHASE_ICON_SIZE
	local picture = ctrlQuest:CreateOrGetControl("picture", "statepicture", 2, 0, size, size)
	AUTO_CAST(picture)

	picture:SetEnableStretch(1)

	if not quest.Done then
		picture:SetImage(M_QUESTS_GET_ICON(quest))
		picture:EnableHitTest(0)
		picture:SetUserValue("RETURN_QUEST_NAME", "None")
		return picture
	end

	picture:SetImage("questinfo_return")
	picture:SetAngleLoop(-3)
	picture:EnableHitTest(1)
	picture:SetTextTooltip("{@st59}Warp back to the quest's NPC.{nl}Backspace works too.{/}")
	picture:SetEventScript(ui.LBUTTONUP, "M_CHASE_WARP")
	picture:SetEventScriptArgString(ui.LBUTTONUP, quest.ObjectId)
	picture:SetUserValue("RETURN_QUEST_NAME", quest.ObjectId)

	if JANSORI_SET_VALUE ~= nil then
		JANSORI_SET_VALUE("Quest_Returnable", 1)
	end

	return picture
end
