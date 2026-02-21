-- Create system button to toggle buy-in shop creation window.
Melia.Ui.SysMenu.AddButton("BtnBuyInShop", "sysmenu_wugushi", "Create Buy-in Shop", "M_TOGGLE_PERSONAL_SHOP()")
Melia.Ui.SysMenu.AddButton("BtnSellShop", "sysmenu_wugushi", "Create Sell Shop", "M_TOGGLE_PERSONAL_SELL_SHOP()")

function M_TOGGLE_PERSONAL_SHOP()

	local frame = ui.GetFrame("personal_shop_register")
	if frame:IsVisible() == 1 then
		frame:ShowWindow(false)
	else
		OPEN_PERSONAL_SHOP_REGISTER()
	end
	
end

function M_TOGGLE_PERSONAL_SELL_SHOP()

	local frame = ui.GetFrame("personal_sell_shop_register")
	if frame:IsVisible() == 1 then
		frame:ShowWindow(false)
	else
		OPEN_PERSONAL_SELL_SHOP_REGISTER()
	end
	
end