local function M_REMOVE_ELEMENT(name, frame, child)
	local frm = ui.GetFrame(frame)
	if frm == nil then
		Melia.Log.Warning("Unclutter: Frame '{0}' not found", frame)
		return
	end

	frm:RemoveChild(child)
end

M_REMOVE_ELEMENT("Open Shops", "minimized_total_shop_button", "openTotalShopBtn")
M_REMOVE_ELEMENT("TP Shop", "minimized_tp_button", "openTPshopBtn")
M_REMOVE_ELEMENT("Open Boards", "minimized_total_board_button", "openTotalBoardBtn")
M_REMOVE_ELEMENT("Open Party", "minimized_total_party_button", "openTotalPartyBtn")
M_REMOVE_ELEMENT("Grow Up", "minimized_guidequest_button", "openBtn")
M_REMOVE_ELEMENT("TP", "openingameshopbtn", "open_openingameshopbtn")
M_REMOVE_ELEMENT("Popo Boost", "minimized_popoboost_2023", "openPoPoBoost")
M_REMOVE_ELEMENT("Popo Boost Notice", "minimized_popoboost_2023", "notice_bg")
M_REMOVE_ELEMENT("Fullscreen Menu", "minimized_fullscreen_navigation_menu_button", "btn_open")
M_REMOVE_ELEMENT("Fullscreen Menu", "minimized_fullscreen_navigation_menu_button", "text_name")
M_REMOVE_ELEMENT("Invite Your Friends Event", "minimized_friend", "openFriend")
