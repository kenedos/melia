Melia.Hook("MAP_OPEN", function(original, result, frame)
	if frame ~= nil and UPDATE_MAP ~= nil then
		UPDATE_MAP(frame)
	end
	return result
end)

local function M_REFRESH_MAP_FRAMES()
	local minimapFrame = ui.GetFrame("minimap")
	if minimapFrame ~= nil and UPDATE_MINIMAP ~= nil then
		UPDATE_MINIMAP(minimapFrame)
	end

	local mapFrame = ui.GetFrame("map")
	if mapFrame ~= nil and mapFrame:IsVisible() == 1 and UPDATE_MAP ~= nil then
		UPDATE_MAP(mapFrame)
	end
end

local _MeliaWorldIconsLoadOriginal = Melia.World.Icons.Load
Melia.World.Icons.Load = function(icons)
	_MeliaWorldIconsLoadOriginal(icons)
	M_REFRESH_MAP_FRAMES()
end

local _MeliaWorldIconsLoadMoreOriginal = Melia.World.Icons.LoadMore
Melia.World.Icons.LoadMore = function(icons)
	_MeliaWorldIconsLoadMoreOriginal(icons)
	M_REFRESH_MAP_FRAMES()
end
