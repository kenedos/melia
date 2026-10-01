-- Fallbacks for when the chase window scripts have not been sent yet.
if M_QUESTS_SET_CHASE == nil then
	function M_QUESTS_SET_CHASE(questCtrl, quest)
		local chkChase = GET_CHILD(questCtrl, "chase", "ui::CCheckBox")
		if chkChase ~= nil then
			chkChase:ShowWindow(0)
		end
	end
end

if M_CHASE_UPDATE_VISIBILITY == nil then
	function M_CHASE_UPDATE_VISIBILITY()
	end
end
