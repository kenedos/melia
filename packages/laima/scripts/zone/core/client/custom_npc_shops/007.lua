-- Socket tooltip hooks for sell shop items
if SET_SHOP_ITEM_TOOLTIP ~= nil and _G._MELIA_SHOP_TOOLTIP_HOOKED == nil then
	_G._MELIA_SHOP_TOOLTIP_HOOKED = true
	_G._MELIA_ORIG_SET_SHOP_ITEM_TOOLTIP = SET_SHOP_ITEM_TOOLTIP
	SET_SHOP_ITEM_TOOLTIP = function(icon, shopItem)
		if shopItem ~= nil and shopItem.type ~= nil then
			local strArg = "inven"
			if shopItem.properties ~= nil and type(shopItem.properties) == "table" then
				local propList = {}
				for k, v in pairs(shopItem.properties) do
					if type(v) == "number" then
						table.insert(propList, k .. "/" .. tostring(math.floor(v)))
					elseif type(v) == "string" and v ~= "None" and v ~= "" then
						table.insert(propList, k .. "/" .. v)
					end
				end
				if #propList > 0 then
					strArg = "copy_prop:" .. table.concat(propList, ";")
				end
			end
			icon:SetTooltipType("wholeitem")
			icon:SetTooltipArg(strArg, shopItem.type, "")
			icon:SetUserValue("SHOPCLSID", shopItem.classID)
			return
		end
		_G._MELIA_ORIG_SET_SHOP_ITEM_TOOLTIP(icon, shopItem)
	end
end
