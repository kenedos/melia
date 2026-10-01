-- Gem tooltip check hooks for sell shop items
if IS_NEED_DRAW_GEM_TOOLTIP ~= nil and _G._MELIA_GEM_TOOLTIP_HOOKED == nil then
	_G._MELIA_GEM_TOOLTIP_HOOKED = true
	_G._MELIA_ORIG_IS_NEED_DRAW_GEM_TOOLTIP = IS_NEED_DRAW_GEM_TOOLTIP
	IS_NEED_DRAW_GEM_TOOLTIP = function(itemObj)
		local invitem = GET_INV_ITEM_BY_ITEM_OBJ(itemObj)
		if invitem ~= nil then
			return _G._MELIA_ORIG_IS_NEED_DRAW_GEM_TOOLTIP(itemObj)
		end
		-- For shop items, just check if MaxSocket > 0
		local maxSocket = TryGetProp(itemObj, "MaxSocket", 0)
		return maxSocket > 0 and maxSocket <= 100
	end
end

if IS_NEED_DRAW_AETHER_GEM_TOOPTIP ~= nil and _G._MELIA_AETHER_TOOLTIP_HOOKED == nil then
	_G._MELIA_AETHER_TOOLTIP_HOOKED = true
	_G._MELIA_ORIG_IS_NEED_DRAW_AETHER_GEM_TOOPTIP = IS_NEED_DRAW_AETHER_GEM_TOOPTIP
	IS_NEED_DRAW_AETHER_GEM_TOOPTIP = function(itemObj)
		local invitem = GET_INV_ITEM_BY_ITEM_OBJ(itemObj)
		if invitem ~= nil then
			return _G._MELIA_ORIG_IS_NEED_DRAW_AETHER_GEM_TOOPTIP(itemObj)
		end
		if itemObj.ItemGrade < 6 then return false end
		local start_index = GET_AETHER_GEM_INDEX_RANGE(TryGetProp(itemObj, 'UseLv', 0))
		return TryGetProp(itemObj, "Socket_" .. start_index, 0) ~= 0
	end
end
