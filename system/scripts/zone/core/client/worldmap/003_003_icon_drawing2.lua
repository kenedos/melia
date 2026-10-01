Melia.World.Icons.MinimapControls = {}
Melia.World.Icons.MapControls = {}

local function HideRemoved(parent, previous, current)
	local keep = {}
	for i = 1, #current do
		keep[current[i]] = true
	end

	for i = 1, #previous do
		if not keep[previous[i]] then
			local ctrl = parent:GetChild(previous[i])
			if ctrl ~= nil then
				ctrl:ShowWindow(0)
			end
		end
	end
end

Melia.Hook("UPDATE_MINIMAP", function(original, result, frame)
	local icons = Melia.World.Icons.GetAll()
	if not icons then
		return result
	end

	local mapName = session.GetMapName()
	local mapProp = geMapTable.GetMapProp(mapName)
	local lstNpcs = frame:GetChild("npclist")

	local previous = Melia.World.Icons.MinimapControls
	Melia.World.Icons.MinimapControls = M_DRAW_MAP_ICONS(lstNpcs, icons, mapName, mapProp, "_M_MINIMAP_ICON_", minimapw, minimaph, 0, 0)
	HideRemoved(lstNpcs, previous, Melia.World.Icons.MinimapControls)

	return result
end)

Melia.Hook("MAKE_MAP_NPC_ICONS", function(original, result, frame, mapName, mapWidth, mapHeight, offsetX, offsetY)
	local icons = Melia.World.Icons.GetAll()
	if not icons then
		return result
	end

	local mapProp = geMapTable.GetMapProp(mapName);

	local previous = Melia.World.Icons.MapControls
	Melia.World.Icons.MapControls = M_DRAW_MAP_ICONS(frame, icons, mapName, mapProp, "_M_MAP_ICON_", mapWidth, mapHeight, offsetX, offsetY)
	HideRemoved(frame, previous, Melia.World.Icons.MapControls)
	MAKE_MY_CURSOR_TOP(frame)

	return result
end)
