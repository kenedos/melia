-- Override the draw equip function, in case we want custom grades.
Melia.Override('DRAW_EQUIP_COMMON_TOOLTIP_SMALL_IMG', function (original, tooltipframe, invitem, mainframename, isForgery)
    local result = original(tooltipframe, invitem, mainframename, isForgery)

    local gBox = GET_CHILD(tooltipframe, mainframename,'ui::CGroupBox')
    --Melia.Log.Info('{0}', gBox)

    local equipCommonCSet = GET_CHILD_RECURSIVELY(tooltipframe, 'equip_common_cset', 'ui::CControlSet')
    --Melia.Log.Info('{0}', equipCommonCSet)
	tolua.cast(equipCommonCSet, "ui::CControlSet");

	local grade = GET_ITEM_GRADE(invitem)
    local score = GET_GEAR_SCORE(invitem)
	local score_text = ''
	if score > 0 then
		score_text = ' (' .. score .. ')'
	end
	--Melia.Log.Info('Grade: {0} Score {1}', grade, score);
    local gradeText = equipCommonCSet:GetUserConfig("GRADE_TEXT_FONT")
	if grade == 1 then
		gradeText = gradeText .. equipCommonCSet:GetUserConfig("NORMAL_GRADE_TEXT")
	elseif grade == 2 then
		gradeText = gradeText .. equipCommonCSet:GetUserConfig("MAGIC_GRADE_TEXT")
	elseif grade == 3 then
		gradeText = gradeText .. equipCommonCSet:GetUserConfig("RARE_GRADE_TEXT")
	elseif grade == 4 then
		gradeText = gradeText .. equipCommonCSet:GetUserConfig("UNIQUE_GRADE_TEXT")
	elseif grade == 5 then
		gradeText = gradeText .. equipCommonCSet:GetUserConfig("LEGEND_GRADE_TEXT")		
	elseif grade == 6 then
		gradeText = gradeText .. equipCommonCSet:GetUserConfig("GODDESS_GRADE_TEXT")
	end
    gradeText = gradeText .. score_text

    local gradeName = GET_CHILD_RECURSIVELY(equipCommonCSet, 'gradeName')
    if 0 < grade then 
		gradeName:SetText(gradeText)
	else
		gradeName:ShowWindow(0);
	end

    return result
end)