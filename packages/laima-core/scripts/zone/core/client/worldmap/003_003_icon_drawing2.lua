Melia.World.Icons.MinimapControls = {}
Melia.World.Icons.MapControls = {}

local function DrawIcons(parent, icons, mapName, mapProp, prefix, width, height, offsetX, offsetY)
	local names = {}

	for i, icon in ipairs(icons) do
		if string.lower(icon.Map) == string.lower(mapName) then
			local ctrlName = prefix .. i
			local ctrl = M_CREATE_MAP_ICON(parent, ctrlName, icon, mapProp, width, height, offsetX, offsetY)

			if ctrl ~= nil then
				ctrl:ShowWindow(1)
			end

			names[#names + 1] = ctrlName
		end
	end

	return names
end

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
	Melia.World.Icons.MinimapControls = DrawIcons(lstNpcs, icons, mapName, mapProp, "_M_MINIMAP_ICON_", minimapw, minimaph, 0, 0)
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
	Melia.World.Icons.MapControls = DrawIcons(frame, icons, mapName, mapProp, "_M_MAP_ICON_", mapWidth, mapHeight, offsetX, offsetY)
	HideRemoved(frame, previous, Melia.World.Icons.MapControls)

	return result
end)
