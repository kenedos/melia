Melia.Override("NOTICE_CTRL_SET", function(original, parentCtrl, noticeName, point)
	local topFrame = parentCtrl and parentCtrl:GetTopParentFrame()
	if topFrame == nil then
		topFrame = ui.GetFrame("sysmenu")
	end
	if topFrame == nil then
		return
	end

	local notice = GET_CHILD_RECURSIVELY(topFrame, noticeName .. "notice")
	if notice == nil then
		return
	end

	local noticeText = notice:GetChild(noticeName .. "noticetext")
	if point > 0 then
		notice:ShowWindow(1)
		noticeText:ShowWindow(1)
		noticeText:SetText('{ol}{b}{s14}' .. tostring(point))
		SYSMENU_NOTICE_TEXT_RESIZE(notice, point)
	else
		notice:ShowWindow(0)
		noticeText:ShowWindow(0)
	end
end)
