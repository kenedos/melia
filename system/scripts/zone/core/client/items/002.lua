-- Override the draw equip function, in case we want custom grades.
Melia.Override('GET_NAME_OWNED', function (original, item)
	local itemName = item.Name
	local legendPrefix = TryGetProp(item, "LegendPrefix")
	if legendPrefix ~= nil then
		itemName = GET_LEGEND_PREFIX_ITEM_NAME(item)
	end

	if TryGetProp(item, "EvolvedItemLv", 0) > TryGetProp(item, "UseLv", 0) then
		itemName = itemName..' '..ClMsg('EvolvedItem')
	end

	if item.ItemType == "Equip" and item.IsPrivate == "YES" and item.Equiped == 0 then
		return ClMsg("LOST_ITEM") .. " " ..itemName;
	end

	local customTooltip = TryGetProp(item, "CustomToolTip");
	if customTooltip ~= nil and customTooltip ~= "None" then
		local nameFunc = _G[customTooltip .. "_NAME"];
		if nameFunc ~= nil then
			return nameFunc(item);
		end
	end

	if GetPropType(item, 'CustomName') ~= nil then
		local customName = item.CustomName;
		if customName ~= "None" then
			return customName..' '..itemName..'';
		end
	end

	return itemName;
end)
