MELIA_COLLECTION_RESISTANCE_NAMES = {
    ResFire_BM = "Fire Property Resistance",
    ResIce_BM = "Ice Property Resistance",
    ResLightning_BM = "Lightning Property Resistance",
    ResEarth_BM = "Earth Property Resistance",
    ResPoison_BM = "Poison Property Resistance",
    ResDark_BM = "Dark Property Resistance",
    ResHoly_BM = "Holy Property Resistance",
    ResSoul_BM = "Soul Property Resistance"
}

function MELIA_COLLECTION_PROPERTY_NAME(propertyName)
    local customName = MELIA_COLLECTION_RESISTANCE_NAMES[propertyName]

    if customName ~= nil then
        return customName
    end

    local translatedName = ClMsg(propertyName)

    if translatedName == nil or translatedName == "None" then
        return propertyName
    end

    return translatedName
end

function MELIA_COLLECTION_EFFECT_DESC(collectionId)
    local cls = GetClassByType("Collection", collectionId)

    if cls == nil or cls.PropList == nil or cls.PropList == "None" then
        return ""
    end

    local values = {}

    for propertyName, propertyValue in string.gmatch(cls.PropList, "([^/]+)/([^/]+)") do
        local value = tonumber(propertyValue) or 0
        local name = MELIA_COLLECTION_PROPERTY_NAME(propertyName)

        if value > 0 then
            values[#values + 1] = string.format("%s +%d", name, value)
        elseif value < 0 then
            values[#values + 1] = string.format("%s %d", name, value)
        else
            values[#values + 1] = name
        end
    end

    return table.concat(values, " ")
end
