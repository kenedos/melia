-- Create system button for mixing table.
Melia.Ui.SysMenu.AddButton("BtnMixingTable", "sysmenu_alchemist", "Mixing Table", "ui.ToggleFrame('mixingtable')")

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